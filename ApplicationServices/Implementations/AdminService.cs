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
    public class AdminService : IAdminService
    {
        private readonly ICoreUnitOfWork CoreUnitOfWork;

        public AdminService(
            ICoreUnitOfWork coreUnitOfWork
            )
        {
            CoreUnitOfWork = coreUnitOfWork;
        }

        public async Task<bool> AdminCheck(string password)
        {
            Account account = await CoreUnitOfWork.AccountRepository.GetById("0000000000000");
            if (account == null)
            {
                throw new ArgumentException("Admin is missing!");
            }
            if (account.Password != password)
            {
                return false;
            }
            return true;
        }

        public async Task BlockAccount(string accountId)
        {
            Account account = await CoreUnitOfWork.AccountRepository.GetById(accountId);
            if (account == null)
            {
                throw new ArgumentException("Account doesn't exist!");
            }

            account.Block();

            await CoreUnitOfWork.AccountRepository.Update(account);
            await CoreUnitOfWork.SaveChangesAsync();            
        }

        public async Task UnblockAccount(string accountId)
        {
            Account account = await CoreUnitOfWork.AccountRepository.GetById(accountId);
            if (account == null)
            {
                throw new ArgumentException("Account doesn't exist!");
            }

            account.Unblock();

            await CoreUnitOfWork.AccountRepository.Update(account);
            await CoreUnitOfWork.SaveChangesAsync();
        }
    }
}
