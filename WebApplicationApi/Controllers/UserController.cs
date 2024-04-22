using System.Net;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApplicationApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpClientFactory _httpClientFactory;

    public UserController(IUserRepository userRepository, IHttpClientFactory httpClientFactory)
    {
        _userRepository = userRepository;
        _httpClientFactory = httpClientFactory;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userRepository.GetUsersAsync();
        return Ok(users);
    }
    
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> CallExternalApi()
    {
        var username = "myuser";
        var password = "mypass";
        var queryString = new Dictionary<string, string>{{"environment", "test"}};
        var queryStringEnvironmentParameeter = QueryHelpers.AddQueryString("/rest-api/v1/system/apikey", queryString);
        using var request = new HttpRequestMessage(HttpMethod.Post, queryStringEnvironmentParameeter)
        {
            Content = JsonContent.Create(new { username, password })
        };
        
        var client = _httpClientFactory.CreateClient("myClient");
        var response = await client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.Created)
        {
            return Ok();
        }
        
        return BadRequest();
    }
}