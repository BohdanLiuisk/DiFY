using Microsoft.AspNetCore.Mvc;

namespace Dify.Entity.WebApi.Controllers;

[ApiController]
[Route("api/entity")]
public class EntityController : ControllerBase
{
    [HttpGet("create")]
    public Task<ActionResult> CreateEntity()
    {
        return Task.FromResult<ActionResult>(Ok(null));
    }
}
