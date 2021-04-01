using ApplicationServices.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Interfaces
{
    public interface ISystemParameterService
    {
        public Task<int> AddNewSystemParameter(SystemParameterDTO systemParameterDTO);
        public Task ChangeSystemParameter(int id, decimal newValue);
        public Task<ICollection<SystemParameterDTO>> GetSystemParameters();

    }
}
