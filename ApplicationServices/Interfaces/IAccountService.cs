using ApplicationServices.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Interfaces
{
    public interface IAccountService
    {
        public Task<string> CreateAccount(AccountDTO accountDTO);
        public Task<bool> AccountPayIn(string accountId, string password, decimal value);
        public Task<bool> AccountPayOut(string accountId, string password, decimal value);
    }
}
