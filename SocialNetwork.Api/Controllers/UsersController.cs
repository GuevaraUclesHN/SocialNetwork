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

        [HttpGet]
        public IActionResult Get()
        {
            var data = _userRepository.Find(FilterDefinition<User>.Empty).ToList();
            return Ok(data);
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
