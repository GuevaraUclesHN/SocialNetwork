using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetwork.Api.DataTransferObjects;
using SocialNetwork.Core.Entities;

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

        [HttpGet(Name = "GetUsers")]
        public ActionResult<IEnumerable<UserListDto>> GetUsers([FromQuery] string? username)
        {
            if (string.IsNullOrEmpty(username))
            {
                var users = _userRepository.Find(Builders<User>.Filter.Empty)
                    .ToList();

                var userList = users.Select(x => new UserListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Username = x.Username
                });

                return Ok(userList);
            }
            else
            {
                var filter = Builders<User>.Filter.Regex("Username", new BsonRegularExpression($"^{username}"));
                var users = _userRepository.Find(filter)
                    .ToList();

                var userList = users.Select(x => new UserListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Username = x.Username
                });

                return Ok(userList);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<UserDetailDto> GetUserById(string id)
        {
            var filter = Builders<User>.Filter.Eq("_id", new ObjectId(id));
            var user = _userRepository.Find(filter)
                .FirstOrDefault();

            if (user is null)
            {
                return BadRequest("No existe el usuario");
            }

            var userDetail = new UserDetailDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Posts = user.Posts.Select(x => new PostDetailDto
                {
                    Id = x.Id,
                    Content = x.Content,
                    UserId = x.UserId
                }).ToList()
            };

            return Ok(userDetail);
        }
    }
}
