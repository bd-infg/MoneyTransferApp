using ApplicationServices.DTOs;
using ApplicationServices.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Enums;
using System;
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

            systemParameter.SetValue(newValue);

            await CoreUnitOfWork.SystemParameterRepository.Update(systemParameter);
            await CoreUnitOfWork.SaveChangesAsync();
        }


    }
}
