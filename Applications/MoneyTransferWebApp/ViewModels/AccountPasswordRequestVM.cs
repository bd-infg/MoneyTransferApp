using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ApplicationServices.DTOs;
using Enums;

namespace MoneyTransferWebApp.ViewModels
{
    public class AccountPasswordRequestVM
    { 
        [Required]
        [StringLength(13, ErrorMessage = "JMBG mora da ima 13 cifara")]
        public string Id { get; set; }
        [Required]
        [StringLength(6, ErrorMessage = "Password mora da ima 6 cifre")]
        public string OldPassword { get; set; }
        [Required]
        [StringLength(6, ErrorMessage = "Password mora da ima 6 cifre")]
        public string NewPassword { get; set; }

        public AccountPasswordRequestDTO ToDTO()
        {
            var dto = new AccountPasswordRequestDTO()
            {
                Id = this.Id,
                OldPassword = this.OldPassword,
                NewPassword = this.NewPassword
            };

            return dto;
        }
    }
}
