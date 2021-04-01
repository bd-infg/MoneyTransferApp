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
        public Task<bool> AccountPayIn(AccountRequestDTO accountBankTransferDTO);
        public Task<bool> AccountPayOut(AccountRequestDTO accountBankTransferDTO);
        public Task<bool> IntraWalletTransfer(IntraWalletTransferDTO intraWalletTransferDTO);
        public Task<AccountBalanceOverviewDTO> GetAccountBalance(AccountOverviewRequestDTO accountRequestDTO);
        public Task<ICollection<TransactionDTO>> GetAccountTransactions(AccountOverviewRequestDTO accountRequestDTO);
        public Task ChangePassword(AccountPasswordRequestDTO accountPasswordRequestDTO);
    }
}
