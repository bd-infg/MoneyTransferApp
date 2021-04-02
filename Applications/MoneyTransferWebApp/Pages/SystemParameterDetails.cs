using ApplicationServices.DTOs;
using ApplicationServices.Interfaces;
using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using MoneyTransferWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyTransferWebApp.Pages
{
    public partial class SystemParameterDetails
    {
        [Inject]
        public NavigationManager NavigationManager { get; private set; }
        [Inject]
        public IToastService ToastService { get; private set; }
        [Inject]
        public ISystemParameterService SystemParameterService { get; private set; }
        public SystemParameterDTO SystemParameter { get; private set; }
        [CascadingParameter] 
        BlazoredModalInstance ModalInstance { get; set; }

        [Parameter]
        public int ParameterId { get; set; }

        protected async override void OnInitialized()
        {
            try {
                SystemParameter = await SystemParameterService.GetSystemParameterById(ParameterId);
            }
            catch(Exception ex)
            {
                ToastService.ShowError(ex.Message, "Error!");
            }
        }


        private async void SaveParameter()
        {
            try
            {
                await SystemParameterService.ChangeSystemParameter(SystemParameter.Id, SystemParameter.Value);
                ModalInstance.Close();
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message, "Error!");
            }
            
        }
    }
}
