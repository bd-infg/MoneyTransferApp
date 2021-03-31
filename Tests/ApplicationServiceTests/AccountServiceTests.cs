using ApplicationServices;
using ApplicationServices.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Services.External.BankService;
using EFCoreDataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockBankService;
using System;
using System.Threading.Tasks;
using Tests.CommandAppServicesIntegrationTests;

namespace ApplicationServiceTests
{
    [TestClass]
    public class AccountServiceTests
    {
        private ICoreUnitOfWork _coreUnitOfWork;
        private CoreEFCoreDbContext _dbContext;
        private IBankService _bankService;

        [TestInitialize]
        public void Setup()
        {
            var dbContextFactory = new SampleDbContextFactory();
            _dbContext = dbContextFactory.CreateDbContext(new string[] { });
            _coreUnitOfWork = new CoreEFCoreUnitOfWork(_dbContext);
            _bankService = new BankService();
        }

        [TestCleanup()]
        public async Task Cleanup()
        {
            await _dbContext.DisposeAsync();
            _coreUnitOfWork = null;
        }

        [TestMethod]
        public async Task TestCreateAccount()
        {
            try
            {
                //Arrange
                AccountService accountService = new AccountService(_coreUnitOfWork, _bankService);
                var accountDTO = new AccountDTO() {
                    Id = "2705996887790",
                    FirstName = "Dejan",
                    LastName = "Boskovic",
                    Bank = 105,
                    Pin = "1447",
                    AccountNumber = "105147852369878985"
                };


                //Act
                string password = await accountService.CreateAccount(accountDTO);

                //Assert
                Account account = await _coreUnitOfWork.AccountRepository.GetById("2705996887790");
       
                Assert.AreNotEqual(null, account, "Account must not be null");
                Assert.AreEqual("2705996887790", account.Id, "Id must be '2705996887790'");
                Assert.AreEqual("Dejan", account.FirstName, "FirstName must be Dejan");
                Assert.AreEqual("Boskovic", account.LastName, "LastName must be Boskovic");
                Assert.AreEqual(105, (int)account.Bank, "Bank must be 105");
                Assert.AreEqual("1447", account.Pin, "Pin must be '1447'");
                Assert.AreEqual("105147852369878985", account.AccountNumber, "AccountNumber must be '105147852369878985'");
                Assert.AreEqual(password, account.Password, $"Passwords must match");
                Assert.AreEqual(0.0m, account.Balance, "Balance must be 0");
                Assert.AreEqual(0.0m, account.MonthlyIncome, "MonthlyIncome must be 0");
                Assert.AreEqual(0.0m, account.MonthlyOutcome, "MonthlyOutcome must be 0");
                Assert.AreEqual(false, account.Blocked, "Blocked must be false");
                Assert.AreEqual(DateTime.Now.Date, account.OpeningDate.Date, "OpeningDate must be today");
                Assert.AreEqual(DateTime.Now.AddMonths(-1).Date, account.LastTransactionDate.Date, "LastTransactionDate must be last month");


            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }

        [TestMethod]
         public async Task TestAccountPayIn()
        {
            try
            {
                //Arrange
                AccountService accountService = new AccountService(_coreUnitOfWork, _bankService);
                var accountDTO = new AccountDTO()
                {
                    Id = "2705996887791",
                    FirstName = "Dejan",
                    LastName = "Boskovic",
                    Bank = 105,
                    Pin = "1447",
                    AccountNumber = "105147852369878985"
                };

                string password = await accountService.CreateAccount(accountDTO);

                //Act

                var success = await accountService.AccountPayIn(new AccountRequestDTO() {Id = "2705996887791", Password = password, Amount = 100.00m });

                //Assert
                Account account = await _coreUnitOfWork.AccountRepository.GetById("2705996887791");
                Assert.AreEqual(success, true, "PayIn must be successful");
                Assert.AreEqual(100.0m, account.Balance, "Balance must be 100");
                Assert.AreEqual(100.0m, account.MonthlyIncome, "MonthlyIncome must be 100");
                Assert.AreEqual(DateTime.Now.Date, account.LastTransactionDate.Date, "LastTransactionDate must be today");


            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }

        [TestMethod]
        public async Task TestAccountMultiplePayIn()
        {
            try
            {
                //Arrange
                AccountService accountService = new AccountService(_coreUnitOfWork, _bankService);
                var accountDTO = new AccountDTO()
                {
                    Id = "2705996887792",
                    FirstName = "Dejan",
                    LastName = "Boskovic",
                    Bank = 105,
                    Pin = "1447",
                    AccountNumber = "105147852369878985"
                };

                string password = await accountService.CreateAccount(accountDTO);

                //Act

                var success = await accountService.AccountPayIn(new AccountRequestDTO() { Id = "2705996887792", Password = password, Amount = 100.00m });
                success = await accountService.AccountPayIn(new AccountRequestDTO() { Id = "2705996887792", Password = password, Amount = 50.00m });

                //Assert
                Account account = await _coreUnitOfWork.AccountRepository.GetById("2705996887792");
                Assert.AreEqual(success, true, "PayIn must be successful");
                Assert.AreEqual(150.0m, account.Balance, "Balance must be 150");
                Assert.AreEqual(150.0m, account.MonthlyIncome, "MonthlyIncome must be 150");
                Assert.AreEqual(DateTime.Now.Date, account.LastTransactionDate.Date, "LastTransactionDate must be today");


            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }

        [TestMethod]
        public async Task TestAccountPayInExceed()
        {
            try
            {
                //Arrange
                AccountService accountService = new AccountService(_coreUnitOfWork, _bankService);
                var accountDTO = new AccountDTO()
                {
                    Id = "2705996887793",
                    FirstName = "Dejan",
                    LastName = "Boskovic",
                    Bank = 105,
                    Pin = "1447",
                    AccountNumber = "105147852369878985"
                };

                string password = await accountService.CreateAccount(accountDTO);

                //Act
                var success = await accountService.AccountPayIn(new AccountRequestDTO() { Id = "2705996887793", Password = password, Amount = 1000001.00m });

                //Assert
                

            }
            catch (MonthlyIncomeExceededException exceed)
            {
                Account account = await _coreUnitOfWork.AccountRepository.GetById("2705996887793");
                Assert.AreEqual(0.0m, account.Balance, "Balance must be 0");
                Assert.AreEqual(0.0m, account.MonthlyIncome, "MonthlyIncome must be 0");
                Assert.AreEqual(DateTime.Now.Date.AddMonths(-1), account.LastTransactionDate.Date, "LastTransactionDate must be last month");
                Assert.AreEqual(exceed.Message, "This account would exceed the monthly income limit");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }

        [TestMethod]
        public async Task TestAccountPayOut()
        {
            try
            {
                //Arrange
                AccountService accountService = new AccountService(_coreUnitOfWork, _bankService);
                var accountDTO = new AccountDTO()
                {
                    Id = "2705996887794",
                    FirstName = "Dejan",
                    LastName = "Boskovic",
                    Bank = 105,
                    Pin = "1447",
                    AccountNumber = "105147852369878985"
                };

                string password = await accountService.CreateAccount(accountDTO);
                var success = await accountService.AccountPayIn(new AccountRequestDTO() { Id = "2705996887794", Password = password, Amount = 100.00m });

                //Act
                success = await accountService.AccountPayOut(new AccountRequestDTO() { Id = "2705996887794", Password = password, Amount = 50.00m });

                //Assert
                Account account = await _coreUnitOfWork.AccountRepository.GetById("2705996887794");
                Assert.AreEqual(success, true, "PayOut must be successful");
                Assert.AreEqual(50.0m, account.Balance, "Balance must be 50");
                Assert.AreEqual(100.0m, account.MonthlyIncome, "MonthlyIncome must be 100");
                Assert.AreEqual(50.0m, account.MonthlyOutcome, "MonthlyOutcome must be 50");
                Assert.AreEqual(DateTime.Now.Date, account.LastTransactionDate.Date, "LastTransactionDate must be today");


            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }

        [TestMethod]
        public async Task TestAccountPayOutMultiple()
        {
            try
            {
                //Arrange
                AccountService accountService = new AccountService(_coreUnitOfWork, _bankService);
                var accountDTO = new AccountDTO()
                {
                    Id = "2705996887895",
                    FirstName = "Dejan",
                    LastName = "Boskovic",
                    Bank = 105,
                    Pin = "1447",
                    AccountNumber = "105147852369878985"
                };

                string password = await accountService.CreateAccount(accountDTO);
                var success = await accountService.AccountPayIn(new AccountRequestDTO() { Id = "2705996887895", Password = password, Amount = 100.00m });

                //Act
                success = await accountService.AccountPayOut(new AccountRequestDTO() { Id = "2705996887895", Password = password, Amount = 50.00m });
                success = await accountService.AccountPayOut(new AccountRequestDTO() { Id = "2705996887895", Password = password, Amount = 25.00m });

                //Assert
                Account account = await _coreUnitOfWork.AccountRepository.GetById("2705996887895");
                Assert.AreEqual(success, true, "PayOut must be successful");
                Assert.AreEqual(25.0m, account.Balance, "Balance must be 25");
                Assert.AreEqual(100.0m, account.MonthlyIncome, "MonthlyIncome must be 100");
                Assert.AreEqual(75.0m, account.MonthlyOutcome, "MonthlyOutcome must be 75");
                Assert.AreEqual(DateTime.Now.Date, account.LastTransactionDate.Date, "LastTransactionDate must be today");


            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }

        [TestMethod]
        public async Task TestAccountPayOutExceed()
        {
            try
            {
                //Arrange
                AccountService accountService = new AccountService(_coreUnitOfWork, _bankService);
                var accountDTO = new AccountDTO()
                {
                    Id = "2705996887796",
                    FirstName = "Dejan",
                    LastName = "Boskovic",
                    Bank = 105,
                    Pin = "1447",
                    AccountNumber = "105147852369878985"
                };

                string password = await accountService.CreateAccount(accountDTO);
                var success = await accountService.AccountPayIn(new AccountRequestDTO() { Id = "2705996887796", Password = password, Amount = 100.00m });

                //Act
                success = await accountService.AccountPayOut(new AccountRequestDTO() { Id = "2705996887796", Password = password, Amount = 1000001.00m });             

            }
            catch (MonthlyOutcomeExceededException exceed)
            {
                Account account = await _coreUnitOfWork.AccountRepository.GetById("2705996887796");
                Assert.AreEqual(100.0m, account.Balance, "Balance must be 100");
                Assert.AreEqual(0.0m, account.MonthlyOutcome, "MonthlyOutcome must be 0");
                Assert.AreEqual(DateTime.Now.Date, account.LastTransactionDate.Date, "LastTransactionDate must be today");
                Assert.AreEqual(exceed.Message, "This account would exceed the monthly outcome limit");

            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }

        [TestMethod]
        public async Task TestAccountIntraWalletTransferBelowLimit()
        {
            try
            {
                //Arrange
                AccountService accountService = new AccountService(_coreUnitOfWork, _bankService);
                var accountDTO1 = new AccountDTO()
                {
                    Id = "2705996887797",
                    FirstName = "Dejan",
                    LastName = "Boskovic",
                    Bank = 105,
                    Pin = "1447",
                    AccountNumber = "105147852369878985"
                };

                var accountDTO2 = new AccountDTO()
                {
                    Id = "2705996887798",
                    FirstName = "Dejan",
                    LastName = "Boskovic",
                    Bank = 105,
                    Pin = "1447",
                    AccountNumber = "105147852369878985"
                };

                string password1 = await accountService.CreateAccount(accountDTO1);
                string password2 = await accountService.CreateAccount(accountDTO2);
                var success = await accountService.AccountPayIn(new AccountRequestDTO() { Id = "2705996887797", Password = password1, Amount = 200.00m });

                //Act
                await accountService.IntraWalletTransfer(new IntraWalletTransferDTO() { Amount = 100.00m, IdFrom = "2705996887797", IdTo = "2705996887798", Password = password1 }) ;

                //Assert
                Account account1 = await _coreUnitOfWork.AccountRepository.GetById("2705996887797");
                Account account2 = await _coreUnitOfWork.AccountRepository.GetById("2705996887798");
                Assert.AreEqual(0.0m, account1.Balance, "Balance must be 0");
                Assert.AreEqual(200.0m, account1.MonthlyIncome, "MonthlyIncome must be 200");
                Assert.AreEqual(200.0m, account1.MonthlyOutcome, "MonthlyOutcome must be 200");
                Assert.AreEqual(100.0m, account2.Balance, "Balance must be 100");
                Assert.AreEqual(100.0m, account2.MonthlyIncome, "MonthlyIncome must be 100");
                Assert.AreEqual(0.0m, account2.MonthlyOutcome, "MonthlyOutcome must be 0");

            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }

        [TestMethod]
        public async Task TestAccountIntraWalletTransferOverLimit()
        {
            try
            {
                //Arrange
                AccountService accountService = new AccountService(_coreUnitOfWork, _bankService);
                var accountDTO1 = new AccountDTO()
                {
                    Id = "2705996887799",
                    FirstName = "Dejan",
                    LastName = "Boskovic",
                    Bank = 105,
                    Pin = "1447",
                    AccountNumber = "105147852369878985"
                };

                var accountDTO2 = new AccountDTO()
                {
                    Id = "2705996887700",
                    FirstName = "Dejan",
                    LastName = "Boskovic",
                    Bank = 105,
                    Pin = "1447",
                    AccountNumber = "105147852369878985"
                };

                string password1 = await accountService.CreateAccount(accountDTO1);
                string password2 = await accountService.CreateAccount(accountDTO2);
                var success = await accountService.AccountPayIn(new AccountRequestDTO() { Id = "2705996887799", Password = password1, Amount = 20000.00m });

                //Act
                await accountService.IntraWalletTransfer(new IntraWalletTransferDTO() { Amount = 15000.00m, IdFrom = "2705996887799", IdTo = "2705996887700", Password = password1 });

                //Assert
                Account account1 = await _coreUnitOfWork.AccountRepository.GetById("2705996887799");
                Account account2 = await _coreUnitOfWork.AccountRepository.GetById("2705996887700");
                Assert.AreEqual(4850.0m, account1.Balance, "Balance must be 4850");
                Assert.AreEqual(20000.0m, account1.MonthlyIncome, "MonthlyIncome must be 20000");
                Assert.AreEqual(15150, account1.MonthlyOutcome, "MonthlyOutcome must be 15150");
                Assert.AreEqual(15000.0m, account2.Balance, "Balance must be 15000");
                Assert.AreEqual(15000.0m, account2.MonthlyIncome, "MonthlyIncome must be 15000");
                Assert.AreEqual(0.0m, account2.MonthlyOutcome, "MonthlyOutcome must be 0");

            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }
    }
}
