using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class SystemParameter
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public decimal Value { get; set; }

        public SystemParameter()
        {

        }

        public SystemParameter(string name, decimal value)
        {
            Name = name;
            Value = value;
        }
    }
}
