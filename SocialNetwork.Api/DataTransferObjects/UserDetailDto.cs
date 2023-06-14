using System;
namespace SocialNetwork.Api.DataTransferObjects
{
    public class UserDetailDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }

        public ICollection<PostDetailDto> Posts { get; set; }
    }
}

