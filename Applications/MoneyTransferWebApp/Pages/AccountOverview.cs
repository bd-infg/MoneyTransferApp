using ApplicationServices.DTOs;
using ApplicationServices.Interfaces;
using Blazored.Toast.Services;
using Enums;
using Microsoft.AspNetCore.Components;
using MoneyTransferWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyTransferWebApp.Pages
{
    public partial class AccountOverview
    {
        [Inject]
        public NavigationManager NavigationManager { get; private set; }
        [Inject]
        public IAccountService AccountService { get; private set; }
        [Inject]
        public IToastService ToastService { get; private set; }
        
        public AccountOverviewRequestVM Input { get; private set; } = new AccountOverviewRequestVM();
        public ICollection<TransactionDTO> Transactions { get; private set; }

        public AccountBalanceOverviewDTO AccountBalance { get; private set; }
        private string FilterBy { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Transactions = null;
            AccountBalance = null;
            FilterBy = "default";
        }
        protected async Task HandleValidSubmit()
        {
            try
            {
                AccountBalance = await AccountService.GetAccountBalance(Input.ToDTO());
                Transactions = await AccountService.GetAccountTransactions(Input.ToDTO());
            }
            catch(Exception ex)
            {
                ToastService.ShowError(ex.Message, "Error!");
            }
            finally
            {
                Input.Password = string.Empty;
                //Input = new CreateAccountVM();
            }
        }

        private bool IsVisible(TransactionDTO transactionDto)
        {
            if (transactionDto != null)
            {
                if(FilterBy == "default")
                {
                    return true;
                }
                else if(FilterBy == "income" && transactionDto.Type == 1)
                {
                    return true;
                }
                else if (FilterBy == "outcome" && transactionDto.Type == 2)
                {
                    return true;
                }
                else if (FilterBy == "transfers" && (transactionDto.Type == 3 || transactionDto.Type == 4))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
