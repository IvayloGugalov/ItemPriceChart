using System;
using System.Collections.Generic;

namespace ItemPriceCharts.Services.Models
{
    public class Email : ValueObject
    {
        public string Value { get; }

        public Email(string value)
        {
            _ = string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentNullException(nameof(value));

            this.Value = value.Length > 220 ? value : throw new ArgumentOutOfRangeException("Value is too long");
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }

        public override string ToString() => this.Value;
    }
}
