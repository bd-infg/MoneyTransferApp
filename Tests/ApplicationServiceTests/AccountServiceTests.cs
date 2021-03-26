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
                    Id = "2705996887794",
                    FirstName = "Dejan",
                    LastName = "Boskovic",
                    Bank = 105,
                    Pin = "1447",
                    AccountNumber = "105147852369878985"
                };


                //Act
                string password = await accountService.CreateAccount(accountDTO);

                //Assert
                Account account = await _coreUnitOfWork.AccountRepository.GetById("2705996887794");
       
                Assert.AreNotEqual(null, account, "Account must not be null");
                Assert.AreEqual("2705996887794", account.Id, "Id must be '2705996887794'");
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
                    Id = "2705996887795",
                    FirstName = "Dejan",
                    LastName = "Boskovic",
                    Bank = 105,
                    Pin = "1447",
                    AccountNumber = "105147852369878985"
                };

                string password = await accountService.CreateAccount(accountDTO);

                //Act

                var success = await accountService.AccountPayIn("2705996887795", password, 100.00m);

                //Assert
                Account account = await _coreUnitOfWork.AccountRepository.GetById("2705996887795");
                Assert.AreEqual(success, true, "PayIn must be successful");
                Assert.AreEqual(100.0m, account.Balance, "Balance must be 100");
                Assert.AreEqual(100.0m, account.MonthlyIncome, "MonthlyIncome must be 100");
                Assert.AreEqual(DateTime.Now.Date, account.LastTransactionDate.Date, "LastTransactionDate must be last month");


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
                    Id = "2705996887796",
                    FirstName = "Dejan",
                    LastName = "Boskovic",
                    Bank = 105,
                    Pin = "1447",
                    AccountNumber = "105147852369878985"
                };

                string password = await accountService.CreateAccount(accountDTO);

                //Act

                await accountService.AccountPayIn("2705996887796", password, 1000001.00m);

                //Assert
                Account account = await _coreUnitOfWork.AccountRepository.GetById("2705996887796");

                Assert.AreEqual(0.0m, account.Balance, "Balance must be 100");
                Assert.AreEqual(0.0m, account.MonthlyIncome, "MonthlyIncome must be 100");
                Assert.AreEqual(DateTime.Now.Date, account.LastTransactionDate.Date, "LastTransactionDate must be last month");


            }
            catch (MonthlyIncomeExceededException exceed)
            {
                Assert.AreEqual(exceed.Message, "This account would exceed the monthly income limit");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }
    }
}
