﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationServices.DTOs
{
    public class AccountBankTransferDTO
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public decimal Amount { get;  set; }
    }
}
