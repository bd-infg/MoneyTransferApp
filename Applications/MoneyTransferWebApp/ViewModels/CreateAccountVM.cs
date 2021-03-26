using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ApplicationServices.DTOs;
using Enums;

namespace MoneyTransferWebApp.ViewModels
{
    public class CreateAccountVM
    { 
        [Required]
        [StringLength(13, ErrorMessage = "JMBG mora da ima 13 cifara")]
        public string Id { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Ime ima maksimalno 30 karaktera")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Prezime ima maksimalno 30 karaktera")]
        public string LastName { get; set; }
        [Required]
        public string Bank { get; set; }
        [Required]
        [StringLength(4, ErrorMessage = "Pin mora da ima 4 cifre")]
        public string Pin { get; set; }
        [Required]
        [StringLength(18, ErrorMessage = "Broj racuna mora imati 18 cifara")]
        public string AccountNumber { get; set; }

        public AccountDTO ToDTO()
        {
            var dto = new AccountDTO()
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Pin = this.Pin,
                AccountNumber = this.AccountNumber,
                Bank = (int)Enum.Parse(typeof(BankType), this.Bank)
            };

            return dto;
        }
    }
}
