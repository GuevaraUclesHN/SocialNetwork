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


        [HttpGet("posts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<PostDetailDto>> GetAllPosts()
        {
            var posts = _postCollection.Find(FilterDefinition<Post>.Empty).ToList();

            var postList = posts.Select(x => new PostDetailDto
            {
                Id = x.Id,
                Content = x.Content,
                UserId = x.UserId
            });

            return Ok(postList);
        }



        [HttpGet("posts/{postId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PostDetailDto> GetPostById([FromRoute] string postId)
        {

            var post = _postCollection.Find(p => p.Id == postId).FirstOrDefault();
            if (post is null)
            {
                return BadRequest($"No se encontró una publicación con id {postId}");
            }

            var postDetailDto = new PostDetailDto
            {
                Id = post.Id,
                Content = post.Content,
                UserId = post.UserId
            };

            return Ok(postDetailDto);
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

        [HttpPost("users/{userId}/posts")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PostDetailDto> CreatePost([FromRoute] string userId, [FromBody] PostCreateDto postDto)
        {
            var user = _userCollection.Find(x => x.Id == userId).FirstOrDefault();
            if (user is null)
            {
                return BadRequest($"No se encontró un usuario con id {userId}");
            }

            var post = new Post
            {
                Content = postDto.Content,
                UserId = userId
            };

            _postCollection.InsertOne(post);

            var postDetailDto = new PostDetailDto
            {
                Id = post.Id,
                Content = post.Content,
                UserId = post.UserId
            };

            return CreatedAtAction(nameof(GetPostById), new { postId = post.Id }, postDetailDto);
        }


        [HttpPut("users/{userId}/posts/{postId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PostDetailDto> UpdatePost([FromRoute] string userId, [FromRoute] string postId, [FromBody] PostUpdateDto postDto)
            {
                var postContent = postDto.Content;

                var post = _postCollection.FindOneAndUpdate(
                    Builders<Post>.Filter.Eq(p => p.Id, postId),
                    Builders<Post>.Update.Set(p => p.Content, postContent),
                    new FindOneAndUpdateOptions<Post>
                    {
                        ReturnDocument = ReturnDocument.After // Devuelve el documento actualizado
                    });

                if (post is null)
                {
                    return BadRequest($"No se encontró una publicación con id {postId}");
                }

                var postDetailDto = new PostDetailDto
                {
                    Id = post.Id,
                    Content = post.Content,
                    UserId = post.UserId
                };

                return Ok(postDetailDto);
            }

    }
}
