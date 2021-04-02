using ApplicationServices.DTOs;
using ApplicationServices.Interfaces;
using Blazored.Modal;
using Blazored.Modal.Services;
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
    public partial class AccountManagement
    {
        [Inject]
        public NavigationManager NavigationManager { get; private set; }
        [Inject]
        public IAdminService AdminService { get; private set; }
        [Inject]
        public IToastService ToastService { get; private set; }
        [Inject]
        public IModalService ModalService { get; private set; }
        [Inject]
        public ISystemParameterService SystemParameterService { get; private set; }

        public bool AdminLogged { get; private set; } = false;

        public class StringWrapper
        {
            public string Data { get; set; }
        }

        public StringWrapper InputPassword { get; private set; } = new StringWrapper();
        public StringWrapper InputIdForBlock { get; private set; } = new StringWrapper();
        public StringWrapper InputIdForUnblock { get; private set; } = new StringWrapper();

        public ICollection<SystemParameterDTO> SystemParameterDTOs { get; private set; } = new List<SystemParameterDTO>();

        protected async void ChooseParameter(int id)
        {
            var parameters = new ModalParameters();
            parameters.Add("ParameterId", id);

            var modal = ModalService.Show<SystemParameterDetails>("Detalji",parameters);
            var result = await modal.Result;
            if (result.Cancelled)
            {

            }
            else
            {
                SystemParameterDTOs = await SystemParameterService.GetSystemParameters();
                StateHasChanged();
            }
        }

        protected async Task CheckAdmin()
        {
            try
            {
                var response = await AdminService.AdminCheck(InputPassword.Data);
                if (response)
                {
                    SystemParameterDTOs = await SystemParameterService.GetSystemParameters();
                    ToastService.ShowSuccess("Dobrodošao admine", "Uspeh!");
                    AdminLogged = true;
                    StateHasChanged();
                }
                else
                {
                    ToastService.ShowError("Laž!", "Neuspeh!");
                    AdminLogged = false;
                }

            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message, "Error!");
                AdminLogged = false;
            }
            finally
            {
                InputPassword.Data = string.Empty;
            }
        }

        protected async Task BlockAccount()
        {
            try
            {
                await AdminService.BlockAccount(InputIdForBlock.Data);
                ToastService.ShowSuccess("I ako je bio blokiran i dalje je blokiran!", "Uspeh!");
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message, "Error!");
            }
            finally
            {
                InputIdForBlock.Data = string.Empty;
            }
        }

        protected async Task UnblockAccount()
        {
            try
            {
                await AdminService.UnblockAccount(InputIdForUnblock.Data);
                ToastService.ShowSuccess("I ako je bio odblokiran i dalje je odblokiran!", "Uspeh!");
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message, "Error!");
            }
            finally
            {
                InputIdForUnblock.Data = string.Empty;
            }
        }
    }
}
