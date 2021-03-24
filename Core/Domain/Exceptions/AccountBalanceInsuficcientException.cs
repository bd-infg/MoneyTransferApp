using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class AccountBalanceInsuficcientException : Exception
    {
        public AccountBalanceInsuficcientException()
        {
        }

        public AccountBalanceInsuficcientException(string message) : base(message)
        {
        }
    }
}
