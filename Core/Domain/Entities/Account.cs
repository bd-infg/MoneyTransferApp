using Domain.Exceptions;
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

        public Transaction PayIn(decimal amount, TransactionType type, string accountFrom)
        {
            Balance += amount;
            if (LastTransactionDate.Month < DateTime.Now.Month)
            {
                MonthlyIncome = amount;
            }
            else
            {
                MonthlyIncome += amount;
            }
            var transaction = new Transaction(amount, accountFrom, this.Id, type, TransactionFlowType.In);
            Transactions.Add(transaction);
            LastTransactionDate = DateTime.Now;
            return transaction;
        }

        public Transaction PayOut(decimal amount, TransactionType type, string accountTo)
        {
            Balance -= amount;
            if(Balance < 0)
            {
                throw new AccountBalanceInsuficcientException($"Your account has insufficient funds, you need {amount - Balance} more");
            }
            if (LastTransactionDate.Month < DateTime.Now.Month)
            {
                MonthlyOutcome = amount;
            }
            else
            {
                MonthlyOutcome += amount;
            }
            var transaction = new Transaction(amount, this.Id, accountTo, type, TransactionFlowType.Out);
            Transactions.Add(transaction);
            LastTransactionDate = DateTime.Now;
            return transaction;
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
