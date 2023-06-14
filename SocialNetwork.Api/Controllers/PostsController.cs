using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Api.DataTransferObjects;
using SocialNetwork.Core.Entities;
using SocialNetwork.Core.Interfaces;

namespace SocialNetwork.Api.Controllers
{
	[ApiController]
	public class PostsController : ControllerBase
	{
        private readonly IRepository<Post> postRepository;
        private readonly IRepository<User> userRepository;

        public PostsController(IRepository<Post> postRepository, IRepository<User> userRepository)
        {
            this.postRepository = postRepository;
            this.userRepository = userRepository;
        }

        //http://localhost:4000/posts/43jkgdsggsdj
        /// <summary>
        /// Agrega una publicación para el usuario.
        /// </summary>
        /// <param name="userId">El id del usuario que agregará la publicación.</param>
        /// <param name="post">La publicación a agregar.</param>
        /// <returns>La publicación agregada.</returns>
        [HttpPost("users/{userId}/[controller]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PostDetailDto> AddPost([FromRoute]int userId, [FromBody]PostCreateDto post)
        {
            var user = this.userRepository.GetById(userId);
            if (user is null)
            {
                return BadRequest($"No se encontró un usuario con id {userId} para crear el post");
            }

            if (string.IsNullOrEmpty(post.Content))
            {
                return BadRequest("No se puede crear un post sin contenido.");
            }

            var createdPost = this.postRepository.Add(new Post
            {
                Content = post.Content,
                UserId = user.Id
            });
            return new CreatedAtActionResult("GetPostById", "Posts", new { userId = userId, postId = createdPost.Id }, new PostDetailDto
            {
                Content = createdPost.Content,
                UserId = createdPost.UserId,
                Id = createdPost.Id
            });
        }

        [HttpGet("users/{userId}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<PostCreateDto>> GetPosts([FromRoute] int userId)
        {
            var user = this.userRepository.GetById(userId);
            if (user is null)
            {
                return BadRequest($"No se encontró un usuario con id {userId}");
            }

            return Ok(this.postRepository.Filter(x => x.UserId == userId)
                .Select(x => new PostDetailDto
                {
                    Id = x.Id,
                    Content = x.Content,
                    UserId = x.UserId
                }));
        }

        [HttpGet("users/{userId}/[controller]/{postId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PostDetailDto> GetPostById([FromRoute] int userId, int postId)
        {
            var user = this.userRepository.GetById(userId);
            if (user is null)
            {
                return BadRequest($"No se encontró un usuario con id {userId}");
            }

            var post = this.postRepository.GetById(postId);
            if (post is null)
            {
                return NotFound($"No se encontró un post con el id {postId}");
            }
            return Ok(new PostDetailDto
            {
                Id = post.Id,
                Content = post.Content,
                UserId = post.UserId
            });
        }
    }
}

