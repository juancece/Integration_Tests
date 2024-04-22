using System.Net;
using System.Net.Http.Json;
using Core.Entities;
using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Tests;

[CollectionDefinition("MyTestCollection")]
[TestCaseOrderer("IntegrationTests.PriorityOrderer", "IntegrationTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
public class UserControllerTest : IClassFixture<TestApiFactory>
{
    private readonly TestApiFactory _testApiFactory;
    private readonly ITestOutputHelper _testOutputHelper;
    private static string _test = "";

    public UserControllerTest(TestApiFactory testApiFactory, ITestOutputHelper testOutputHelper)
    {
        _testApiFactory = testApiFactory;
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact, TestPriority(1)]
        public async Task AWhenGetUser_ShouldReturnListOfSeedUsers()
        {
            _testOutputHelper.WriteLine(typeof(PriorityOrderer).FullName);
            _testOutputHelper.WriteLine(typeof(PriorityOrderer).Assembly.GetName().Name);
            _test += "1";
            _testOutputHelper.WriteLine(_test);
            var expectedUser = new UserEntity
            {
                Id = 1,
                Name = "Lela",
                LastName = "Estes"
            };
            var client = _testApiFactory.CreateClient();
            var result = await client.GetAsync("User");
            var users = await result.Content.ReadFromJsonAsync<List<UserEntity>>();

            //Asserts
            users.Should().ContainEquivalentOf(expectedUser);
            _test += "2";
            _testOutputHelper.WriteLine(_test);
        }

        [Fact, TestPriority(2)]
        public async Task BWhenGetUser_ShouldReturnListOfSeedUsers()
        {
            _test += "3";
            _testOutputHelper.WriteLine(_test);
            var expectedUser = new UserEntity
            {
                Id = 1,
                Name = "Lela",
                LastName = "Estes"
            };
            var client = _testApiFactory.CreateClient();
            var result = await client.GetAsync("User");
            var users = await result.Content.ReadFromJsonAsync<List<UserEntity>>();

            //Asserts
            users.Should().ContainEquivalentOf(expectedUser);
            _test += "4";
            _testOutputHelper.WriteLine(_test);
        }

        [Fact, TestPriority(3)]
        public async Task CWhenGetUser_ShouldReturnListOfSeedUsers()
        {
            _test += "5";
            _testOutputHelper.WriteLine(_test);
            var expectedUser = new UserEntity
            {
                Id = 1,
                Name = "Lela",
                LastName = "Estes"
            };
            var client = _testApiFactory.CreateClient();
            var result = await client.GetAsync("User");
            var users = await result.Content.ReadFromJsonAsync<List<UserEntity>>();

            //Asserts
            users.Should().ContainEquivalentOf(expectedUser);
            _test += "6";
            _testOutputHelper.WriteLine(_test);
        }
        
        [Fact, TestPriority(4)]
        public async Task WhenCallingExternalAPI_ShouldReturnStatusCodeCreated()
        {
            _test += "7";
            _testOutputHelper.WriteLine(_test);

            var client = _testApiFactory.CreateClient();
            var result = await client.GetAsync("User/CallExternalApi");

            //Asserts
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            _test += "8";
            _testOutputHelper.WriteLine(_test);
        }
}