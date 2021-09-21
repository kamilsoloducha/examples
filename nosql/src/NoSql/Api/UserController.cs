using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NoSql.Domain;

namespace NoSql.Api
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly MongoClient _clinet;
        private readonly IMongoDatabase _database;

        public UserController(MongoClient clinet)
        {
            _clinet = clinet;
            _database = _clinet.GetDatabase("test-db");
        }

        [HttpGet("get")]
        public async Task<IActionResult> Test()
        {
            var dbNames = await _clinet.ListDatabaseNamesAsync();
            return new JsonResult(await dbNames.ToListAsync());
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(string name, string password)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Password = password,
                CreationDate = DateTime.Now,
                IsActive = true
            };

            var users = _database.GetCollection<User>("Users");
            await users.InsertOneAsync(user);

            return new JsonResult(user.Id);
        }

        [HttpPost("add/transaction")]
        public async Task<IActionResult> AddTransaction(string name, string password)
        {

            using var session = await _clinet.StartSessionAsync();
            session.StartTransaction();
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Password = password,
                CreationDate = DateTime.Now,
                IsActive = true
            };

            var users = _database.GetCollection<User>("Users");
            await users.InsertOneAsync(session, user);
            if (string.IsNullOrEmpty(password))
            {
                await session.AbortTransactionAsync();
                return new BadRequestResult();
            }
            await session.CommitTransactionAsync();
            return new JsonResult(user.Id);
        }

        [HttpGet("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = _database.GetCollection<User>("Users");
            var result = await users.FindAsync(FilterDefinition<User>.Empty);
            return new JsonResult(await result.ToListAsync());
        }


        [HttpDelete("remove/all")]
        public async Task<IActionResult> RemoveAll()
        {
            var users = _database.GetCollection<User>("Users");
            await users.DeleteManyAsync(FilterDefinition<User>.Empty);
            return Ok();
        }
    }
}