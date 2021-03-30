using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationServices.DTOs
{
    public class IntraWalletTransferDTO
    {
        public string IdFrom { get; set; }
        public string IdTo { get; set; }
        public string Password { get; set; }
        public decimal Amount { get;  set; }
    }
}
