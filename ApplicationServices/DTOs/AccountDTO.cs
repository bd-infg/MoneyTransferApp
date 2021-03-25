using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationServices.DTOs
{
    public class AccountDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Bank { get; set; }
        public string Pin { get; set; }
        public string AccountNumber { get;  set; }
    }
}
