using System.Net;
using WireMock.Logging;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace IntegrationTests.MockApis;

public class MockChiliServer :IDisposable
{
    private WireMockServer _wireMockServer;
    public string Url => _wireMockServer.Url!;

    public void StartServer()
    {
        var logger = new WireMockConsoleLogger();
        _wireMockServer = WireMockServer.Start(new WireMockServerSettings
        {
            Port = 49291,
            Logger = logger,
            AllowPartialMapping = true
        });
    }

    public void SetupGetChiliApiKey()
    {
        var requestBuilder = Request.Create()
            .WithPath("/rest-api/vi/system/apikey")
            .WithParam("environment", "test")
            .UsingPost()
            .WithBody("{\r\n \"username\":\"myuser\",\r\n \"password\":\"mypass\"\r\n}")
            .WithHeader("Content-Type", "application/json");
        
        var responseBuilder = Response.Create()
            .WithHeader("Content-Type", "application/xml; charset=utf-8")
            .WithBody("<apikey succeeded=\"true\" key=\"testKey\"/>")
            .WithStatusCode(HttpStatusCode.Created);
        
        _wireMockServer.Given(requestBuilder).RespondWith(responseBuilder);
    }
    
    public void Dispose()
    {
        _wireMockServer?.Dispose();
        _wireMockServer?.Stop();
    }
}