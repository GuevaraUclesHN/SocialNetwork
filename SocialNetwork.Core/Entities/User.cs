using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
namespace SocialNetwork.Core.Entities
{
	public class User
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("Posts")]
        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
	}
}

