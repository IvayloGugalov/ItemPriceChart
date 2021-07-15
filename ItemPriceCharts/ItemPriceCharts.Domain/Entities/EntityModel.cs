using System;

namespace ItemPriceCharts.Domain.Entities
{
    public abstract class EntityModel
    {
        public Guid Id { get; protected set; }
    }
}
