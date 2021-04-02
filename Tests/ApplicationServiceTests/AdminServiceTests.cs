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
    public class AdminServiceTests
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
        public async Task TestBlockAccount()
        {
            try
            {
                //Arrange
                AdminService adminService = new AdminService(_coreUnitOfWork);
                AccountService accountService = new AccountService(_coreUnitOfWork, _bankService);
                var accountDTO = new AccountDTO() {
                    Id = "2705996888790",
                    FirstName = "Dejan",
                    LastName = "Boskovic",
                    Bank = 105,
                    Pin = "1447",
                    AccountNumber = "105147852369878985"
                };


                //Act
                string password = await accountService.CreateAccount(accountDTO);
                await adminService.BlockAccount("2705996888790");

                //Assert
                Account account = await _coreUnitOfWork.AccountRepository.GetById("2705996888790");
       

                Assert.AreEqual(true, account.Blocked, "Account must be blocked");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }

        [TestMethod]
        public async Task TestUnblockAccount()
        {
            try
            {
                //Arrange
                AdminService adminService = new AdminService(_coreUnitOfWork);
                AccountService accountService = new AccountService(_coreUnitOfWork, _bankService);
                var accountDTO = new AccountDTO()
                {
                    Id = "2706996888790",
                    FirstName = "Dejan",
                    LastName = "Boskovic",
                    Bank = 105,
                    Pin = "1447",
                    AccountNumber = "105147852369878985"
                };


                //Act
                string password = await accountService.CreateAccount(accountDTO);
                await adminService.BlockAccount("2706996888790");
                await adminService.UnblockAccount("2706996888790");

                //Assert
                Account account = await _coreUnitOfWork.AccountRepository.GetById("2706996888790");


                Assert.AreEqual(false, account.Blocked, "Account must be unblocked");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }
    }
}
