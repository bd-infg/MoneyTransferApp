using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ApplicationServices.DTOs;
using Enums;

namespace MoneyTransferWebApp.ViewModels
{
    public class AccountRequestVM
    { 
        [Required]
        [StringLength(13, ErrorMessage = "JMBG mora da ima 13 cifara")]
        public string Id { get; set; }
        [Required]
        [StringLength(6, ErrorMessage = "Password mora da ima 6 cifre")]
        public string Password { get; set; }
        [Required]
        public decimal Amount { get; set; }

        public AccountRequestDTO ToDTO()
        {
            var dto = new AccountRequestDTO()
            {
                Id = this.Id,
                Password = this.Password,
                Amount = this.Amount
            };

            return dto;
        }
    }
}
