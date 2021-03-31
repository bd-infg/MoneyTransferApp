using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationServices.DTOs
{
    public class TransactionDTO
    {
        public decimal Amount { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
        public byte Type { get; set; }
        public byte Flow { get; set; }
        public DateTime DateTime { get; set; }
    }
}
