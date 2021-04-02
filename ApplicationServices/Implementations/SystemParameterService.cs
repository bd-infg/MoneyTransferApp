using ApplicationServices.DTOs;
using ApplicationServices.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationServices
{
    public class SystemParameterService : ISystemParameterService
    {
        private readonly ICoreUnitOfWork CoreUnitOfWork;

        public SystemParameterService(
            ICoreUnitOfWork coreUnitOfWork
            )
        {
            CoreUnitOfWork = coreUnitOfWork;
        }

        public async Task<int> AddNewSystemParameter(SystemParameterDTO systemParameterDTO)
        {
            SystemParameter systemParameter = await CoreUnitOfWork.SystemParameterRepository.GetFirstOrDefaultWithIncludes(sp => sp.Name == systemParameterDTO.Name);
            if (systemParameter != null)
            {
                throw new ArgumentException("System parameter with this name already exists");
            }

            systemParameter = new SystemParameter(
                    systemParameterDTO.Name,
                    systemParameterDTO.Value
                );
            await CoreUnitOfWork.SystemParameterRepository.Insert(systemParameter);
            await CoreUnitOfWork.SaveChangesAsync();
            return systemParameter.Id;
        }

        public async Task ChangeSystemParameter(int id, decimal newValue)
        {
            SystemParameter systemParameter = await CoreUnitOfWork.SystemParameterRepository.GetById(id);
            if (systemParameter == null)
            {
                throw new ArgumentException("System parameter with this id doesn't exist");
            }
            if(systemParameter.Name == "ProvisionOverLimitCostPercent" || systemParameter.Name == "BonusDaysOnCreate" || systemParameter.Name == "BonusTransfersPerMonth")
            {
                newValue = Math.Round(newValue);
            }
            systemParameter.SetValue(newValue);

            await CoreUnitOfWork.SystemParameterRepository.Update(systemParameter);
            await CoreUnitOfWork.SaveChangesAsync();
        }

        public async Task<SystemParameterDTO> GetSystemParameterById(int id)
        {
            var systemParam = await CoreUnitOfWork.SystemParameterRepository.GetById(id);
            if (systemParam == null)
            {
                throw new ArgumentException("System parameter with this id doesn't exist");
            }

            return new SystemParameterDTO() { Id = systemParam.Id, Name = systemParam.Name, Value = systemParam.Value };
        }

        public async Task<ICollection<SystemParameterDTO>> GetSystemParameters()
        {
            var result = new List<SystemParameterDTO>();

            var systemParameters = await CoreUnitOfWork.SystemParameterRepository.GetAllList();


            foreach (var sysParam in systemParameters)
            {
                result.Add(new SystemParameterDTO() { Id = sysParam.Id, Name = sysParam.Name, Value = sysParam.Value });
            }
            return result;
        }

    }
}
