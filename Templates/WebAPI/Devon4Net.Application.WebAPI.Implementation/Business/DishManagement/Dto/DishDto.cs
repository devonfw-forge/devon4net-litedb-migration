using Newtonsoft.Json;

namespace Devon4Net.Application.WebAPI.Implementation.Business.DishManagement.Dto
{
    public class DishDto
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }
        [JsonProperty(PropertyName = "modificationCounter")]
        public int ModificationCounter { get; set; }
        [JsonProperty(PropertyName = "revision")]
        public object Revision { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "price")]
        public decimal? Price { get; set; }
        [JsonProperty(PropertyName = "imageId")]
        public long? ImageId { get; set; }
    }
}
