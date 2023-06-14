using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetwork.Api.DataTransferObjects;
using SocialNetwork.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SocialNetwork.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMongoCollection<User> _userRepository;

        public UsersController(IMongoDatabase database)
        {
            _userRepository = database.GetCollection<User>("Users");
        }

        [HttpGet]
        public IActionResult Get()
        {
            var data = _userRepository.Find(FilterDefinition<User>.Empty).ToList();
            var userListDto = data.Select(user => new UserListDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email
            }).ToList();

            return Ok(userListDto);
        }

        [HttpGet("{id}")]
        public ActionResult<UserDetailDto> GetUserById(string id)
        {
            var filter = Builders<User>.Filter.Eq("_id", new ObjectId(id));
            var user = _userRepository.Find(filter).FirstOrDefault();

            if (user is null)
            {
                return BadRequest("No existe el usuario");
            }

            var userDetail = new UserDetailDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email,
                Posts = user.Posts.Select(x => new PostDetailDto
                {
                    Id = x.Id,
                    Content = x.Content,
                    UserId = x.UserId
                }).ToList()
            };

            return Ok(userDetail);
        }

        [HttpPost]
        public ActionResult<UserDetailDto> CreateUser([FromBody] UserCreateDto userDto)
        {
            var user = new User
            {
                Name = userDto.Name,
                Username = userDto.Username,
                Email = userDto.Email,
                Posts = new List<Post>()
            };

            _userRepository.InsertOne(user);

            var userDetailDto = new UserDetailDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email,
                Posts = new List<PostDetailDto>()
            };

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, userDetailDto);
        }

        [HttpPut("{id}")]
        public ActionResult<UserDetailDto> UpdateUser(string id, [FromBody] UserCreateDto userDto)
        {
            var filter = Builders<User>.Filter.Eq("_id", new ObjectId(id));
            var user = _userRepository.Find(filter).FirstOrDefault();

            if (user is null)
            {
                return BadRequest("No existe el usuario");
            }

            if (!string.IsNullOrEmpty(userDto.Name))
            {
                user.Name = userDto.Name;
            }

            if (!string.IsNullOrEmpty(userDto.Username))
            {
                user.Username = userDto.Username;
            }

            if (!string.IsNullOrEmpty(userDto.Email))
            {
                user.Email = userDto.Email;
            }

            _userRepository.ReplaceOne(filter, user);

            var userDetailDto = new UserDetailDto
            {
                Id = user.Id,
                Name = string.IsNullOrEmpty(userDto.Name) ? user.Name : userDto.Name,
                Username = string.IsNullOrEmpty(userDto.Username) ? user.Username : userDto.Username,
                Email = string.IsNullOrEmpty(userDto.Email) ? user.Email : userDto.Email,
                Posts = user.Posts.Select(x => new PostDetailDto
                {
                    Id = x.Id,
                    Content = x.Content,
                    UserId = x.UserId
                }).ToList()
            };

            return Ok(userDetailDto);
        }

    }
}
