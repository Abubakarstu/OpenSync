using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenSync.Infrastructure.Realtime.Connection;

namespace OpenSync.Api.Controllers.V1;

public class HealthController : BaseController
{
    private readonly IConnectionManager _connectionManager;

    public HealthController(IConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    [AllowAnonymous]
    [HttpGet("health")]
    public IActionResult Get()
    {
        return Ok(new
        {
            Status = "healthy",
            Timestamp = DateTime.UtcNow,
            ActiveConnections = _connectionManager.ConnectionCount
        });
    }
}
