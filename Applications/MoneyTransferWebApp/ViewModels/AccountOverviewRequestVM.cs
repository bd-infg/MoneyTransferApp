using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ApplicationServices.DTOs;
using Enums;

namespace MoneyTransferWebApp.ViewModels
{
    public class AccountOverviewRequestVM
    { 
        [Required]
        [StringLength(13, ErrorMessage = "JMBG mora da ima 13 cifara")]
        public string Id { get; set; }
        [Required]
        [StringLength(6, ErrorMessage = "Password mora da ima 6 cifre")]
        public string Password { get; set; }
        [Required]
        public DateTime Date { get; set; }


        public AccountOverviewRequestDTO ToDTO()
        {
            var dto = new AccountOverviewRequestDTO()
            {
                Id = this.Id,
                Password = this.Password,
                Date = this.Date
            };

            return dto;
        }
    }
}
