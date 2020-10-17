using System.ComponentModel.DataAnnotations;

namespace ItemPriceCharts.Services.Models
{
    public abstract class EntityModel : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
    }
}
