using System;
using System.Threading.Tasks;
using DiFY.Modules.Social.Application.Calling.CreateCall;
using DiFY.Modules.Social.Application.Calling.EndCall;
using DiFY.Modules.Social.Application.Contracts;
using DiFY.WebAPI.Configuration.Authorization;
using DiFY.WebAPI.Modules.Social.Calling.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiFY.WebAPI.Modules.Social.Calling;

[ApiController]
[Route("api/social/calls")]
public class CallController : ControllerBase
{
    private readonly ISocialModule _socialModule;
    
    public CallController(ISocialModule socialModule)
    {
        _socialModule = socialModule;
    }
    
    [HttpPost("createCall")]
    [HasPermission(CallPermission.CanCreateCall)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<NewCallDto>> CreateCall(CreateCallDto request) 
    {
        var callId = await _socialModule.ExecuteCommandAsync(new CreateCallCommand(DateTime.UtcNow));
        return Ok(new NewCallDto() { CallId = callId });
    }
    
    [HttpPut("endCall/{callId:guid}")]
    [HasPermission(CallPermission.CanEndCall)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<CallSummary>> EndCall([FromQuery] Guid callId)
    {
        var summary = await _socialModule.ExecuteCommandAsync(new EndCallCommand(callId, DateTime.UtcNow));
        return Ok(summary);
    }
}