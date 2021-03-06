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
    public partial class AccountPayIn
    {
        [Inject]
        public NavigationManager NavigationManager { get; private set; }
        [Inject]
        public IAccountService AccountService { get; private set; }
        [Inject]
        public IToastService ToastService { get; private set; }
        
        public AccountRequestVM Input { get; private set; } = new AccountRequestVM();

        protected async Task HandleValidSubmit()
        {
            try
            {
                var response = await AccountService.AccountPayIn(Input.ToDTO());
                if (response)
                {
                    ToastService.ShowSuccess("Novac je uplaćen na račun", "Uspeh!");
                    Input = new AccountRequestVM();
                }
                else
                {
                    ToastService.ShowError("Banka je odbila Vaš zahtev", "Neuspeh!");
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
