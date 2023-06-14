using System;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SocialNetwork.Api.DataTransferObjects;
using SocialNetwork.Core.Entities;

namespace SocialNetwork.Api.Controllers
{
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IMongoCollection<Post> _postCollection;
        private readonly IMongoCollection<User> _userCollection;

        public PostsController(IMongoDatabase database)
        {
            _postCollection = database.GetCollection<Post>("Posts");
            _userCollection = database.GetCollection<User>("Users");
        }

        [HttpGet("users/{userId}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<PostCreateDto>> GetPosts([FromRoute] string userId)
        {
            var user = _userCollection.Find(x => x.Id == userId).FirstOrDefault();
            if (user is null)
            {
                return BadRequest($"No se encontró un usuario con id {userId}");
            }

            var posts = _postCollection.Find(x => x.UserId == userId)
                .ToList();

            var postList = posts.Select(x => new PostDetailDto
            {
                Id = x.Id,
                Content = x.Content,
                UserId = x.UserId
            });

            return Ok(postList);
        }

    }
}
