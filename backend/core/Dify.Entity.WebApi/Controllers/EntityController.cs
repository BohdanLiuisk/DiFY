using Dify.Entity.Structure;
using Microsoft.AspNetCore.Mvc;

namespace Dify.Entity.WebApi.Controllers;

[ApiController]
[Route("api/entity")]
public class EntityController(EntityStructureManager entityStructureManager) : ControllerBase
{
    [HttpGet("create")]
    public async Task<ActionResult> CreateEntity()
    {
        var structures = await entityStructureManager.GetAllEntityStructures();
        Console.WriteLine(structures.Count);
        var contactStructure = await entityStructureManager.FindEntityStructureByName("contact");
        return await Task.FromResult<ActionResult>(Ok(null));
    }
}
