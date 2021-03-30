using Domain.Exceptions;
using Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Account
    {
        public string Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public BankType Bank { get; private set; }
        public string Pin { get; private set; }
        public string Password { get; private set; }
        public string AccountNumber { get; private set; }
        public decimal Balance { get; private set; }
        public DateTime OpeningDate { get; private set; }
        public DateTime LastTransactionDate { get; private set; }
        public decimal MonthlyIncome { get; private set; }
        public decimal MonthlyOutcome { get; private set; }
        public ICollection<Transaction> Transactions { get; private set; }
        public byte[] RowVersion { get; private set; }
        public bool Blocked { get; private set; }
        public Account()
        {
            Transactions = new List<Transaction>();
        }

        public Account(string id, string firstName, string lastName, BankType bankType, string pin, string accountNumber, string password)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Bank = bankType;
            Pin = pin;
            Password = password;
            AccountNumber = accountNumber;
            OpeningDate = DateTime.Now;
            LastTransactionDate = DateTime.Now.AddMonths(-1);
            MonthlyIncome = 0.0m;
            MonthlyOutcome = 0.0m;
            Balance = 0.0m;
            Transactions = new List<Transaction>();
            Blocked = false;
        }

        public void PayIn(decimal amount, TransactionType type, string accountFrom, decimal monthlyIncomeLimit)
        {
            decimal newMonthlyIncome;
            if (LastTransactionDate < DateTime.Now && LastTransactionDate.Month < DateTime.Now.Month)
            {
                newMonthlyIncome = amount;
            }
            else
            {
                newMonthlyIncome = MonthlyIncome + amount;
            }
            if(newMonthlyIncome > monthlyIncomeLimit)
            {
                throw new MonthlyIncomeExceededException("This account would exceed the monthly income limit");
            }
            MonthlyIncome = newMonthlyIncome;
            Balance += amount;
            var transaction = new Transaction(amount, accountFrom, this.Id, type, TransactionFlowType.In);
            Transactions.Add(transaction);
            LastTransactionDate = DateTime.Now;
        }

        public int PayOut(decimal amount, TransactionType type, string accountTo, decimal monthlyOutcomeLimit, decimal provision = 0.0m)
        {
            decimal newMonthlyOutcome;
            if (LastTransactionDate < DateTime.Now && LastTransactionDate.Month < DateTime.Now.Month)
            {
                newMonthlyOutcome = amount + provision;
            }
            else
            {
                newMonthlyOutcome = MonthlyOutcome + amount + provision;
            }
            if (newMonthlyOutcome > monthlyOutcomeLimit)
            {
                throw new MonthlyOutcomeExceededException("This account would exceed the monthly outcome limit");
            }

            MonthlyOutcome = newMonthlyOutcome;
            int cnt = 0;
            Balance -= amount + provision;
            if (Balance < 0)
            {
                throw new AccountBalanceInsuficcientException($"Your account has insufficient funds, you need {amount - Balance} more");
            }

            var transaction = new Transaction(amount, this.Id, accountTo, type, TransactionFlowType.Out);
            Transactions.Add(transaction);
            cnt++;

            if (type == TransactionType.IntraWallet)
            {
                var provisionTransaction = new Transaction(provision, this.Id, "System", TransactionType.Compensation, TransactionFlowType.Out);
                Transactions.Add(provisionTransaction);
                cnt++;
            }

            LastTransactionDate = DateTime.Now;
            return cnt;
        }

        public void Block()
        {
            Blocked = true;
        }

        public void Unblock()
        {
            Blocked = false;
        }
    }
}
