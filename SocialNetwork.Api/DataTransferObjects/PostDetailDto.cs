using System;

namespace SocialNetwork.Api.DataTransferObjects
{
	public class PostDetailDto
	{
        public int Id { get; set; }

        public string Content { get; set; }

        public int UserId { get; set; }
    }
}

