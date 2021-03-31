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
    public partial class AccountPassword
    {
        [Inject]
        public NavigationManager NavigationManager { get; private set; }
        [Inject]
        public IAccountService AccountService { get; private set; }
        [Inject]
        public IToastService ToastService { get; private set; }
        
        public AccountPasswordRequestVM Input { get; private set; } = new AccountPasswordRequestVM();

        protected async Task HandleValidSubmit()
        {
            try
            {
                await AccountService.ChangePassword(Input.ToDTO());
                ToastService.ShowSuccess("Šifra je promenjena", "Uspeh!");
                Input = new AccountPasswordRequestVM();

            }
            catch(Exception ex)
            {
                ToastService.ShowError(ex.Message, "Error!");
            }
            finally
            {
                Input.OldPassword = string.Empty;
                Input.NewPassword = string.Empty;
                //Input = new CreateAccountVM();
            }
        }
    }
}
