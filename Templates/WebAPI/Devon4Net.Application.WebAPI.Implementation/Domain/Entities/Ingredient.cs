using Microsoft.EntityFrameworkCore;

namespace Devon4Net.Application.WebAPI.Implementation.Domain.Entities
{
    [Owned]
    public partial class Ingredient
    {
        public Ingredient()
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
    }
}
