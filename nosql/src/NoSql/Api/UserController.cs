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
        public async Task<IActionResult> Add(AddUserRequest request)
        {
            var address = new Address
            {
                Street = request.Street,
                City = request.City,
                PostCode = request.PostCode
            };
            var user = new User
            {
                Id = Guid.NewGuid(),
                Login = request.Login,
                Password = request.Password,
                CreationDate = DateTime.Now,
                IsActive = true,
                Address = address
            };

            var users = _database.GetCollection<User>("Users");
            await users.InsertOneAsync(user);

            var personalData = new PersonalData
            {
                UserId = user.Id,
                FirstName = request.FirstName,
                Surname = request.Surname
            };
            var personalDatas = _database.GetCollection<PersonalData>("PersonalDatas");
            await personalDatas.InsertOneAsync(personalData);

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
                Login = name,
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

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var userCollection = _database.GetCollection<User>("Users");
            var personalDataCollection = _database.GetCollection<PersonalData>("PersonalDatas");

            var users = await userCollection.AsQueryable().ToListAsync();
            // var presonalData = await personalDataCollection.AsQueryable().ToListAsync();
            // var response = new
            // {
            //     users,
            //     presonalData
            // };

            return new JsonResult(users);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUsers(Guid id)
        {
            var userCollection = _database.GetCollection<User>("Users");
            var personalDataCollection = _database.GetCollection<PersonalData>("PersonalDatas");

            var userCur = await userCollection.FindAsync(x => x.Id == id);
            var user = await userCur.SingleAsync();

            var pdCur = await personalDataCollection.FindAsync(x => x.UserId == id);
            var pd = await pdCur.SingleOrDefaultAsync();
            user.PersonalData = pd;

            return new JsonResult(user);
        }



        [HttpGet("users/personalData")]
        public async Task<IActionResult> GetUsersWithPersonalData()
        {
            var userCollection = _database.GetCollection<User>("Users");
            var personalDataCollection = _database.GetCollection<PersonalData>("PersonalDatas");

            var usersCur = await userCollection.FindAsync(FilterDefinition<User>.Empty);
            var users = await usersCur.ToListAsync();
            foreach (var item in users)
            {
                var pdCur = await personalDataCollection.FindAsync(x => x.UserId == item.Id);
                item.PersonalData = await pdCur.FirstOrDefaultAsync();
            }

            return new JsonResult(users);
        }

        [HttpGet("personalDatas")]
        public async Task<IActionResult> GetPersonalDatas()
        {
            var personalDataCollection = _database.GetCollection<PersonalData>("PersonalDatas");

            var users = await personalDataCollection.AsQueryable().ToListAsync();

            return new JsonResult(users);
        }

        [HttpGet("perosnalData/{userId}")]
        public async Task<IActionResult> GetPersonalData(Guid userId)
        {
            var personalDataCollection = _database.GetCollection<PersonalData>("PersonalDatas");

            var cur = await personalDataCollection.FindAsync(x => x.UserId == userId);
            var personalData = await cur.SingleAsync();

            return new JsonResult(personalData);
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