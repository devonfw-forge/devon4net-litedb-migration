using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Devon4Net.Application.WebAPI.Implementation.Domain.Entities
{
    [Owned]
    public class Image
    {
        public Image()
        {
        }

        public string Content { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public string Extension { get; set; }
        public int? ContentType { get; set; }
    }
}
