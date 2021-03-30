using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class SystemParameter
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public decimal Value { get; private set; }

        public SystemParameter()
        {

        }

        public SystemParameter(string name, decimal value)
        {
            Name = name;
            Value = value;
        }

        public void SetValue(decimal newValue)
        {
            Value = newValue;
        }
    }
}
