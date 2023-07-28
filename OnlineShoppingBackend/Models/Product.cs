using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingBackend.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        public string ProductName { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; }
        [Required]
        public string Brand { get; set; } = string.Empty;
        [Required]
        public string Features { get; set; } = string.Empty;
        [Required]
        public string ProductDescription { get; set; } = string.Empty;
        [Required]
        public string ImageURL { get; set; } = string.Empty;
    }
    public class ProductDetailsDto
    {
        [Required]
        public string ProductName { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; }
        [Required]
        public string Brand { get; set; } = string.Empty;
        [Required]
        public string Features { get; set; } = string.Empty;
        [Required]
        public string ProductDescription { get; set; } = string.Empty;
        [Required]
        public string ImageURL { get; set; } = string.Empty;
    }
}
