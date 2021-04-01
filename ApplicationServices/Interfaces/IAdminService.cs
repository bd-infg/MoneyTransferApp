using ApplicationServices.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Interfaces
{
    public interface IAdminService
    {
        public Task<bool> AdminCheck(string password);
        public Task BlockAccount(string accountId);
        public Task UnblockAccount(string accountId);
    }
}
