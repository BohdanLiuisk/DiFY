using Dify.Entity.Abstract;
using Dify.Entity.SelectQuery.Models;
using Dify.Entity.Structure;
using Dify.Entity.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dify.Entity.WebApi.Controllers;

[ApiController]
[Route("api/entity")]
public class EntityController(EntityStructureManager entityStructureManager, ISelectQueryExecutor selectQueryExecutor) 
    : ControllerBase
{
    [HttpGet("create")]
    public async Task<ActionResult> CreateEntity()
    {
        var structures = await entityStructureManager.GetAllEntityStructures();
        Console.WriteLine(structures.Count);
        var contactStructure = await entityStructureManager.FindEntityStructureByName("contact");
        return await Task.FromResult<ActionResult>(Ok(null));
    }
    
    [HttpPost("select_query")]
    public async Task<ActionResult> SelectEntity(SelectQueryConfig selectConfig) {
        var selectResult = await selectQueryExecutor.ExecuteAsync(selectConfig);
        return new ContentResult {
            Content = selectResult,
            ContentType = "application/json",
            StatusCode = 200
        };
    }
    
    [HttpGet("select_query_test")]
    public async Task<ActionResult> SelectContact() {
        var selectRequest = new SelectQueryConfig {
            EntityName = "contact",
            Columns = new List<SelectColumnConfig> {
                new("id"),
                new("name"),
                new("created_by") {
                    Columns = new List<SelectColumnConfig> {
                        new("id"),
                        new("name")
                    }
                },
                new("parent_contact") {
                    Columns = new List<SelectColumnConfig> {
                        new("id"),
                        new("name"),
                    }
                }
            }
        };
        var selectResult = await selectQueryExecutor.ExecuteAsync<IEnumerable<ContactModelTest>>(
            selectRequest);
        return Ok(selectResult);
    }
}
