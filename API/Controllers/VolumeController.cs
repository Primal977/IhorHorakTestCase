using API.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VolumeController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;

    public VolumeController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [HttpGet("find")]
    public async Task<IActionResult> GetVolumes([FromQuery] string q)
    {
        var googleApiClient = _clientFactory.CreateClient("google-api-client");
        var client = new VolumeAPIClient(googleApiClient);
        var result = await client.FindVolumesAsync(q);
        return Ok(result);
    }
}
