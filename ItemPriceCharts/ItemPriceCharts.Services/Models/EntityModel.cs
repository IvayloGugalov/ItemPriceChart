using Microsoft.EntityFrameworkCore;

namespace ItemPriceCharts.Services.Models
{
    [Index(nameof(Id), IsUnique = true)]
    public abstract class EntityModel
    {
        public int Id { get; protected set; }
    }
}
