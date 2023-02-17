using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EfCoreDdd.Controllers.Requests;
using EfCoreDdd.Infrastructure.DataAccess;
using EfCoreDdd.Model.Entities;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;
using Newtonsoft.Json;
using NUnit.Framework;

namespace EfCoreDdd.Performance.Tests;

[TestFixture]
public class AddCardTests
{
    private const string Host = "http://localhost:5000";
    private const string Url = $"{Host}/card";
    private int _counter;

    private Owner Owner { get; set; }
    private Group Group { get; set; }

    [OneTimeSetUp]
    public async Task Setup()
    {
        await using var dbContext = new LocalDbContext(false);
        // await dbContext.Database.EnsureDeletedAsync();
        // await dbContext.Database.EnsureCreatedAsync();

        Owner = new Owner();
        Group = Owner.CreateGroup("TestGroup");

        await dbContext.Owners.AddAsync(Owner);
        await dbContext.SaveChangesAsync();
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    public void AddCard_HappyPath(int iteration)
    {
        var addCardScenario = Step.Create($"Add Card {iteration}", HttpClientFactory.Create(), async context =>
        {
            try
            {
                Interlocked.Increment(ref _counter);
                var addCardRequest = new AddCard(
                    $"Front {_counter}",
                    $"Back {_counter}",
                    $"Front Example {_counter}",
                    $"Back Example {_counter}",
                    Group.Id);
                var request = Http.CreateRequest("POST", Url)
                    .WithBody(new StringContent(JsonConvert.SerializeObject(addCardRequest), Encoding.UTF8, "application/json"));

                return await Http.Send(request, context);
            }
            catch (Exception)
            {
                return Response.Fail();
            }
        });

        var scenario = ScenarioBuilder.CreateScenario($"card/add/{iteration}", addCardScenario)
            .WithWarmUpDuration(TimeSpan.FromSeconds(3))
            .WithLoadSimulations(LoadSimulation.NewKeepConstant(10, TimeSpan.FromSeconds(10)));

        NBomberRunner
            .RegisterScenarios(scenario)
            .Run();
    }
}