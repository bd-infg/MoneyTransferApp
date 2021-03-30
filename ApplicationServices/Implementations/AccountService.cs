using ApplicationServices.DTOs;
using ApplicationServices.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services.External.BankService;
using Enums;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace ApplicationServices
{
    public class AccountService : IAccountService
    {
        private readonly ICoreUnitOfWork CoreUnitOfWork;
        private readonly IBankService BankService;

        public AccountService(
            ICoreUnitOfWork coreUnitOfWork,
            IBankService bankService
            )
        {
            CoreUnitOfWork = coreUnitOfWork;
            BankService = bankService;
        }

        public async Task<bool> AccountPayIn(AccountBankTransferDTO accountBankTransferDTO)
        {
            Account account = await CoreUnitOfWork.AccountRepository.GetById(accountBankTransferDTO.Id);
            if (account == null)
            {
                throw new ArgumentException("Account for this person doesn't exist!");
            }
            if(account.Password != accountBankTransferDTO.Password)
            {
                throw new ArgumentException("Passwords do not match!");
            }
            if (account.Blocked)
            {
                throw new ArgumentException("This account is blocked!");
            }

            var systemParameter = await CoreUnitOfWork.SystemParameterRepository.GetFirstOrDefaultWithIncludes(sp => sp.Name == "MonthlyIncomeLimit");
            decimal monthlyIncomeLimit = systemParameter.Value;
            account.PayIn(accountBankTransferDTO.Amount, TransactionType.BankWithdrawalToWallet, "BankAccount", monthlyIncomeLimit);

            var isValid = await BankService.Withdraw(account.Id, account.Pin);
            if (isValid)
            {
                await CoreUnitOfWork.AccountRepository.Update(account);
                await CoreUnitOfWork.SaveChangesAsync();
            }

            return isValid;
        }

        public async Task<bool> AccountPayOut(AccountBankTransferDTO accountBankTransferDTO)
        {
            Account account = await CoreUnitOfWork.AccountRepository.GetById(accountBankTransferDTO.Id);
            if (account == null)
            {
                throw new ArgumentException("Account for this person doesn't exist!");
            }
            if (account.Password != accountBankTransferDTO.Password)
            {
                throw new ArgumentException("Passwords do not match!");
            }
            if (account.Blocked)
            {
                throw new ArgumentException("This account is blocked!");
            }

            var systemParameter = await CoreUnitOfWork.SystemParameterRepository.GetFirstOrDefaultWithIncludes(sp => sp.Name == "MonthlyOutcomeLimit");
            decimal monthlyOutcomeLimit = systemParameter.Value;
            account.PayOut(accountBankTransferDTO.Amount, TransactionType.BankDepositFromWallet, "BankAccount", monthlyOutcomeLimit);

            var isValid = await BankService.Deposit(account.Id, account.Pin);
            if (isValid)
            {
                await CoreUnitOfWork.AccountRepository.Update(account);
                await CoreUnitOfWork.SaveChangesAsync();
            }

            return isValid;
        }

        public async Task<string> CreateAccount(AccountDTO accountDTO)
        {
            Account account = await CoreUnitOfWork.AccountRepository.GetById(accountDTO.Id);
            if(account != null)
            {
                throw new ArgumentException("Account for this person already exists!");
            }

            string dateOfBirthString = accountDTO.Id.Substring(0, 7);
            if(dateOfBirthString[4] == '9')
            {
                dateOfBirthString = dateOfBirthString.Insert(4, "1");
            }
            else
            {
                dateOfBirthString = dateOfBirthString.Insert(4, "2");
            }

            var dateOfBirth = new DateTime(int.Parse(dateOfBirthString.Substring(4, 4)), int.Parse(dateOfBirthString.Substring(2, 2)), int.Parse(dateOfBirthString.Substring(0, 2)));
            var eighteenplus = dateOfBirth.AddYears(18);
            if(DateTime.Now < eighteenplus)
            {
                throw new ArgumentException("You are not at least 18 years old!");
            }

            string bankPassword = await BankService.CheckStatus(accountDTO.Id, accountDTO.Pin);
            if(bankPassword == "ERROR!")
            {
                throw new ArgumentException("Your bank credentials are wrong!");
            }

            account = new Account(
                accountDTO.Id,
                accountDTO.FirstName,
                accountDTO.LastName,
                (BankType)accountDTO.Bank,
                accountDTO.Pin,
                accountDTO.AccountNumber,
                bankPassword
                );
            await CoreUnitOfWork.AccountRepository.Insert(account);
            await CoreUnitOfWork.SaveChangesAsync();

            return account.Password;
            
        }
    }
}
