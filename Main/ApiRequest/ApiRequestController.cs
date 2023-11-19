using Microsoft.AspNetCore.Mvc;

namespace Main.ApiRequest;

[ApiController]
[Route("[controller]")]
public class ApiRequestController(IHttpClientFactory httpClientFactory, ILogger<ApiRequestController> logger) : ControllerBase
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("weather");
    private readonly ILogger<ApiRequestController> _logger = logger;

    [HttpGet(Name = "api-request")]
    public async Task<IActionResult> Get()
    {
        var response = await _httpClient.GetAsync("weatherforecast");
        return Ok(response);
    }
}
