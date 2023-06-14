using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace SocialNetwork.Core.Entities
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }

        [BsonElement("Content")]
        public string Content { get; set; }

        [BsonElement("UserId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public int UserId { get; set; }

	}
}

