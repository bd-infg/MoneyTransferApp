using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ApplicationServices.DTOs;
using Enums;

namespace MoneyTransferWebApp.ViewModels
{
    public class AccountAccountTransferVM
    { 
        [Required]
        [StringLength(13, ErrorMessage = "JMBG mora da ima 13 cifara")]
        public string IdFrom { get; set; }
        [Required]
        [StringLength(13, ErrorMessage = "JMBG mora da ima 13 cifara")]
        public string IdTo { get; set; }
        [Required]
        [StringLength(6, ErrorMessage = "Password mora da ima 6 cifre")]
        public string Password { get; set; }
        [Required]
        public decimal Amount { get; set; }

        public IntraWalletTransferDTO ToDTO()
        {
            var dto = new IntraWalletTransferDTO()
            {
                IdFrom = this.IdFrom,
                IdTo = this.IdTo,
                Password = this.Password,
                Amount = this.Amount
            };

            return dto;
        }
    }
}
