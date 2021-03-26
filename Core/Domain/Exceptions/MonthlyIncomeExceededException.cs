using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class MonthlyIncomeExceededException : Exception
    {
        public MonthlyIncomeExceededException()
        {
        }

        public MonthlyIncomeExceededException(string message) : base(message)
        {
        }
    }
}
