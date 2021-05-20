using System;

namespace ItemPriceCharts.Domain.Entities
{
    public record Email
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

        public override string ToString() => this.Value;
    }
}
