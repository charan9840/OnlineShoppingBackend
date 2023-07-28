using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using MongoDB.Driver;
using OnlineShoppingBackend.Data;
using OnlineShoppingBackend.Models;
using System.Runtime;

namespace OnlineShoppingBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _User;
        private readonly IDatabaseSettings _settings;

        public UserController(IConfiguration configuration, IDatabaseSettings settings)
        {

            _settings = settings;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _User = database.GetCollection<User>("RegisteredUser");
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="newuser"></param>

        [HttpPost("Register"), AllowAnonymous]
        public async Task<PrepareResponse> CreateUser(UserDto data)
        {
            var response = new PrepareResponse();
            if (await _User.Find(f => f.LoginId == data.LoginId).AnyAsync())
            {
                response.IsSuccess = false;
                response.Message = "username already exists";
                return response;
            }
            try
            {
                var user = new User
                {
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Email = data.Email,
                    LoginId = data.LoginId,
                    Password = data.Password,
                    ConformPassword = data.ConformPassword,
                    Contactnumber = data.Contactnumber,
                    isAdmin = false,
                };
                await _User.InsertOneAsync(user);
                response.IsSuccess = true;
                response.Message = "Data inserted";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
