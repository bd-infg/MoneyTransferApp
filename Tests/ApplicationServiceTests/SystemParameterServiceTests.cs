using ApplicationServices;
using ApplicationServices.DTOs;
using Domain.Entities;
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
    public class SystemParameterServiceTests
    {
        private ICoreUnitOfWork _coreUnitOfWork;
        private CoreEFCoreDbContext _dbContext;

        [TestInitialize]
        public void Setup()
        {
            var dbContextFactory = new SampleDbContextFactory();
            _dbContext = dbContextFactory.CreateDbContext(new string[] { });
            _coreUnitOfWork = new CoreEFCoreUnitOfWork(_dbContext);
        }

        [TestCleanup()]
        public async Task Cleanup()
        {
            await _dbContext.DisposeAsync();
            _coreUnitOfWork = null;
        }

        [TestMethod]
        public async Task TestAddNewSystemParameter()
        {
            try
            {
                //Arrange
                SystemParameterService systemParameterService = new SystemParameterService(_coreUnitOfWork);
                var systemParameterDTO = new SystemParameterDTO() {
                    Name = "Parameter1",
                    Value = 100.00m,
                };

                //Act
                int id  = await systemParameterService.AddNewSystemParameter(systemParameterDTO);

                //Assert
                SystemParameter systemParameter = await _coreUnitOfWork.SystemParameterRepository.GetById(id);
       
                Assert.AreNotEqual(null, systemParameter, "SystemParameter must not be null");
                Assert.AreEqual("Parameter1", systemParameter.Name, "Name must be Parameter1");
                Assert.AreEqual(100.00m, systemParameter.Value, "Value must be 100.00");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }

        [TestMethod]
        public async Task TestChangeSystemParameter()
        {
            try
            {
                //Arrange
                SystemParameterService systemParameterService = new SystemParameterService(_coreUnitOfWork);
                var systemParameterDTO = new SystemParameterDTO()
                {
                    Name = "Parameter2",
                    Value = 100.00m,
                };
                int id = await systemParameterService.AddNewSystemParameter(systemParameterDTO);



                //Act
                await systemParameterService.ChangeSystemParameter(id, 120.00m);

                //Assert
                SystemParameter systemParameter = await _coreUnitOfWork.SystemParameterRepository.GetById(id);

                Assert.AreNotEqual(null, systemParameter, "SystemParameter must not be null");
                Assert.AreEqual("Parameter2", systemParameter.Name, "Name must be Parameter2");
                Assert.AreEqual(120.00m, systemParameter.Value, "Value must be 120.00");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }
    }
}
