using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationServices.DTOs
{
    public class AccountOverviewRequestDTO
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public DateTime Date { get;  set; }
    }
}
