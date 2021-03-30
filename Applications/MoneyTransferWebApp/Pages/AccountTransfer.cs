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
    public partial class AccountTransfer
    {
        [Inject]
        public NavigationManager NavigationManager { get; private set; }
        [Inject]
        public IAccountService AccountService { get; private set; }
        [Inject]
        public IToastService ToastService { get; private set; }
        
        public AccountAccountTransferVM Input { get; private set; } = new AccountAccountTransferVM();

        protected async Task HandleValidSubmit()
        {
            try
            {
                var response = await AccountService.IntraWalletTransfer(Input.ToDTO());
                if (response)
                {
                    ToastService.ShowSuccess("Novac je prosleđen", "Uspeh!");
                    Input = new AccountAccountTransferVM();
                }
                else
                {
                    ToastService.ShowError("", "Neuspeh!");
                }

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
    }
}
