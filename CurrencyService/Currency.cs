using System;

namespace CurrencyConverter.Service
{
    public class Currency : IEquatable<Currency>
    {
        public Currency(string code,string name)
        {
            Code = code;
            Name = name;
        }

        public string Name { get; private set; }
        public string Code { get; private set; }

        public bool Equals(Currency other)
        {
            return this.Code == other.Code && this.Name == other.Name;
        }
    }
}
