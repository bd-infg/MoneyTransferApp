using ApplicationServices.DTOs;
using ApplicationServices.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services.External.BankService;
using Enums;
using System;
using System.Collections.Generic;
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

        public async Task<bool> AccountPayIn(AccountRequestDTO accountBankTransferDTO)
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

            decimal monthlyIncomeLimit = (await CoreUnitOfWork.SystemParameterRepository.GetFirstOrDefaultWithIncludes(sp => sp.Name == "MonthlyIncomeLimit")).Value;

            account.PayIn(accountBankTransferDTO.Amount, TransactionType.BankWithdrawalToWallet, "BankAccount", monthlyIncomeLimit);

            var isValid = await BankService.Withdraw(account.Id, account.Pin);
            if (isValid)
            {
                await CoreUnitOfWork.AccountRepository.Update(account);
                await CoreUnitOfWork.SaveChangesAsync();
            }

            return isValid;
        }

        public async Task<bool> AccountPayOut(AccountRequestDTO accountBankTransferDTO)
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

            decimal monthlyOutcomeLimit = (await CoreUnitOfWork.SystemParameterRepository.GetFirstOrDefaultWithIncludes(sp => sp.Name == "MonthlyOutcomeLimit")).Value;
            int bonusDaysOnCreate = Decimal.ToInt32((await CoreUnitOfWork.SystemParameterRepository.GetFirstOrDefaultWithIncludes(sp => sp.Name == "BonusDaysOnCreate")).Value);
            int bonusTransfersPerMonth = Decimal.ToInt32((await CoreUnitOfWork.SystemParameterRepository.GetFirstOrDefaultWithIncludes(sp => sp.Name == "BonusTransfersPerMonth")).Value);
            account.PayOut(accountBankTransferDTO.Amount, TransactionType.BankDepositFromWallet, "BankAccount", monthlyOutcomeLimit, bonusDaysOnCreate, bonusTransfersPerMonth);

            var isValid = await BankService.Deposit(account.Id, account.Pin);
            if (isValid)
            {
                await CoreUnitOfWork.AccountRepository.Update(account);
                await CoreUnitOfWork.SaveChangesAsync();
            }

            return isValid;
        }

        public async Task ChangePassword(AccountPasswordRequestDTO accountPasswordRequestDTO)
        {
            Account account = await CoreUnitOfWork.AccountRepository.GetById(accountPasswordRequestDTO.Id);
            if (account == null)
            {
                throw new ArgumentException("Account for this person doesn't exist!");
            }
            if (account.Password != accountPasswordRequestDTO.OldPassword)
            {
                throw new ArgumentException("Passwords do not match!");
            }
            if (account.Blocked)
            {
                throw new ArgumentException("This account is blocked!");
            }

            account.SetPassword(accountPasswordRequestDTO.NewPassword);

            await CoreUnitOfWork.AccountRepository.Update(account);
            await CoreUnitOfWork.SaveChangesAsync();
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

        public async Task<AccountBalanceOverviewDTO> GetAccountBalance(AccountOverviewRequestDTO accountRequestDTO)
        {
            Account account = await CoreUnitOfWork.AccountRepository.GetById(accountRequestDTO.Id);
            if (account == null)
            {
                throw new ArgumentException("Account for this person doesn't exist!");
            }
            if (account.Password != accountRequestDTO.Password)
            {
                throw new ArgumentException("Passwords do not match!");
            }

            var result = new AccountBalanceOverviewDTO();

            result.Balance = account.Balance;
            result.Blocked = account.Blocked;
            result.MonthlyIncome = account.MonthlyIncome;
            result.MonthlyOutcome = account.MonthlyOutcome;

            return result;
        }

        public async Task<ICollection<TransactionDTO>> GetAccountTransactions(AccountOverviewRequestDTO accountRequestDTO)
        {
            Account account = await CoreUnitOfWork.AccountRepository.GetById(accountRequestDTO.Id);
            if (account == null)
            {
                throw new ArgumentException("Account for this person doesn't exist!");
            }
            if (account.Password != accountRequestDTO.Password)
            {
                throw new ArgumentException("Passwords do not match!");
            }

            var result = new List<TransactionDTO>();

            var transactions = await CoreUnitOfWork.TransactionRepository.GetFilteredList(t =>
            (
            (
            (t.FromAccountId == accountRequestDTO.Id && t.ToAccountId == "BankAccount") ||
            (t.FromAccountId == "BankAccount" && t.ToAccountId == accountRequestDTO.Id) ||
            (t.FromAccountId == accountRequestDTO.Id && t.ToAccountId == "System") ||
            (t.FromAccountId == accountRequestDTO.Id && t.Flow == TransactionFlowType.Out) ||
            (t.ToAccountId == accountRequestDTO.Id && t.Flow == TransactionFlowType.In)
            )
            &&
            t.DateTime.Date == accountRequestDTO.Date.Date
            )
            );


            foreach(var transaction in transactions)
            {
                result.Add(new TransactionDTO() { Amount = transaction.Amount, Flow = (byte)transaction.Flow, Type = (byte)transaction.Type, FromAccountId = transaction.FromAccountId, ToAccountId = transaction.ToAccountId, DateTime = transaction.DateTime });
            }
            return result;
        }

        public async Task<bool> IntraWalletTransfer(IntraWalletTransferDTO intraWalletTransferDTO)
        {
            if(intraWalletTransferDTO.IdFrom == intraWalletTransferDTO.IdTo)
            {
                throw new ArgumentException("Bruh, don't transfer to yourself, mkay?");
            }
            Account accountFrom = await CoreUnitOfWork.AccountRepository.GetById(intraWalletTransferDTO.IdFrom);
            if (accountFrom == null)
            {
                throw new ArgumentException("Account for person from doesn't exist!");
            }
            if (accountFrom.Password != intraWalletTransferDTO.Password)
            {
                throw new ArgumentException("Passwords do not match!");
            }
            if (accountFrom.Blocked)
            {
                throw new ArgumentException("This account is blocked!");
            }

            Account accountTo = await CoreUnitOfWork.AccountRepository.GetById(intraWalletTransferDTO.IdTo);
            if (accountTo == null)
            {
                throw new ArgumentException("Account for person to doesn't exist!");
            }
            if (accountTo.Blocked)
            {
                throw new ArgumentException("Receiving account is blocked!");
            }

            decimal monthlyIncomeLimit = (await CoreUnitOfWork.SystemParameterRepository.GetFirstOrDefaultWithIncludes(sp => sp.Name == "MonthlyIncomeLimit")).Value;
            decimal monthlyOutcomeLimit = (await CoreUnitOfWork.SystemParameterRepository.GetFirstOrDefaultWithIncludes(sp => sp.Name == "MonthlyOutcomeLimit")).Value;
            decimal provisionLimit = (await CoreUnitOfWork.SystemParameterRepository.GetFirstOrDefaultWithIncludes(sp => sp.Name == "ProvisionLimit")).Value;
            decimal provisionUnderLimitCost = (await CoreUnitOfWork.SystemParameterRepository.GetFirstOrDefaultWithIncludes(sp => sp.Name == "ProvisionUnderLimitCost")).Value;
            decimal provisionOverLimitCostPercent = (await CoreUnitOfWork.SystemParameterRepository.GetFirstOrDefaultWithIncludes(sp => sp.Name == "ProvisionOverLimitCostPercent")).Value;
            int bonusDaysOnCreate = Decimal.ToInt32((await CoreUnitOfWork.SystemParameterRepository.GetFirstOrDefaultWithIncludes(sp => sp.Name == "BonusDaysOnCreate")).Value);
            int bonusTransfersPerMonth = Decimal.ToInt32((await CoreUnitOfWork.SystemParameterRepository.GetFirstOrDefaultWithIncludes(sp => sp.Name == "BonusTransfersPerMonth")).Value);
            decimal actualProvision;
            
            if (intraWalletTransferDTO.Amount < provisionLimit)
            {
                actualProvision = provisionUnderLimitCost;
            }
            else
            {
                actualProvision = intraWalletTransferDTO.Amount * provisionOverLimitCostPercent / 100.00m;
            }

            accountFrom.PayOut(intraWalletTransferDTO.Amount, TransactionType.IntraWallet, accountTo.Id, monthlyOutcomeLimit, bonusDaysOnCreate, bonusTransfersPerMonth, actualProvision);
            accountTo.PayIn(intraWalletTransferDTO.Amount, TransactionType.IntraWallet, accountFrom.Id, monthlyIncomeLimit);

            await CoreUnitOfWork.AccountRepository.Update(accountFrom);
            await CoreUnitOfWork.AccountRepository.Update(accountFrom);
            await CoreUnitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
