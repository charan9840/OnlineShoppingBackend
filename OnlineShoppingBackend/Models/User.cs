using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OnlineShoppingBackend.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("firstname")]
        public string FirstName { get; set; }
        [BsonElement("lastname")]
        public string LastName { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("loginid")]
        public string LoginId { get; set; } = string.Empty;
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("conformpassword")]
        public string ConformPassword { get; set; }
        [BsonElement("contactnumber")]
        public long Contactnumber { get; set; }
        [BsonElement("isAdmin")]
        public bool isAdmin { get; set; }
    }
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string ConformPassword { get; set; }
        public long Contactnumber { get; set; }
    }
    public class PrepareResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
