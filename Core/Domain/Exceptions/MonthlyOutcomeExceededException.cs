using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class MonthlyOutcomeExceededException : Exception
    {
        public MonthlyOutcomeExceededException()
        {
        }

        public MonthlyOutcomeExceededException(string message) : base(message)
        {
        }
    }
}
