using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Devon4Net.Application.WebAPI.Implementation.Domain.Entities
{
    [Owned]
    public class Category
    {
        public Category()
        {
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
