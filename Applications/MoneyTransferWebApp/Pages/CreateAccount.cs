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
    public partial class CreateAccount
    {
        [Inject]
        public NavigationManager NavigationManager { get; private set; }
        [Inject]
        public IAccountService AccountService { get; private set; }
        [Inject]
        public IToastService ToastService { get; private set; }
        
        public CreateAccountVM Input { get; private set; } = new CreateAccountVM();

        protected async Task HandleValidSubmit()
        {
            try
            {
                var response = await AccountService.CreateAccount(Input.ToDTO());
                if (response != "ERROR!")
                {
                    ToastService.ShowSuccess(response, "Vaš password");
                    Input = new CreateAccountVM();
                }
                else
                {
                    ToastService.ShowError(response, "Banka je odbila Vaš zahtev");
                }

            }
            catch(Exception ex)
            {
                ToastService.ShowError(ex.Message, "Error!");
            }
            finally
            {
                Input.Pin = string.Empty;
                //Input = new CreateAccountVM();
            }
        }
    }
}
