using Domain.Services.External.BankService;
using System;
using System.Threading.Tasks;

namespace MockBankService
{
    public class BankService : IBankService
    {
        public BankService()
        {

        }
        static Random random = new Random();
        public async Task<string> CheckStatus(string personalId, string pin)
        {
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
            char[] chars = new char[6];

            for (int i = 0; i < 6; i++)
            {
                chars[i] = allowedChars[random.Next(0, allowedChars.Length)];
            }

            var result = new string(chars);
            return result;
        }

        public async Task<bool> Withdraw(string personalId, string pin)
        {
            return true;
        }

        public async Task<bool> Deposit(string personalId, string pin)
        {
            return true;
        }
    }
}
