using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.External.BankService
{
    public interface IBankService
    {
        Task<string> AuthenticateUser(string accountNumber, string pin);
    }
}
