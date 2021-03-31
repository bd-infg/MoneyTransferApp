using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationServices.DTOs
{
    public class AccountBalanceOverviewDTO
    {
        public decimal Balance { get; set; }
        public decimal MonthlyIncome { get; set; }
        public decimal MonthlyOutcome { get; set; }
        public bool Blocked { get; set; }
    }
}
