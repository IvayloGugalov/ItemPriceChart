using System;
using System.Collections.Generic;

namespace ItemPriceCharts.Services.Models
{
    public class Email : ValueObject
    {
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.Length > 220)
            {
                throw new ArgumentOutOfRangeException(nameof(value), message: "Email value is longer than 220 characters");
            }

            this.Value = value;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }

        public override string ToString() => this.Value;
    }
}
