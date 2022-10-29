using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiFY.Modules.Social.Application.Calling.CreateCall;
using DiFY.Modules.Social.Application.Calling.EndCall;
using DiFY.Modules.Social.Application.Calling.GetAllCalls;
using DiFY.Modules.Social.Application.Calling.JoinCall;
using DiFY.Modules.Social.Application.Calling.LeftCall;
using DiFY.Modules.Social.Application.Contracts;
using DiFY.Modules.Social.Domain.Calling;
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
        var callId = await _socialModule.ExecuteCommandAsync(new CreateCallCommand(request.Name, DateTime.UtcNow));
        return Ok(new NewCallDto() { CallId = callId });
    }
    
    [HttpPut("joinCall/{callId:guid}")]
    [HasPermission(CallPermission.CanJoinOrLeftCall)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> JoinCall(Guid callId)
    {
        await _socialModule.ExecuteCommandAsync(new JoinCallCommand(callId));
        return Ok();
    }
    
    [HttpPut("leftCall/{callId:guid}")]
    [HasPermission(CallPermission.CanJoinOrLeftCall)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LeftCall(Guid callId)
    {
        await _socialModule.ExecuteCommandAsync(new LeftCallCommand(callId));
        return Ok();
    }

    [HttpPut("endCall/{callId:guid}")]
    [HasPermission(CallPermission.CanEndCall)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<CallSummary>> EndCall(Guid callId)
    {
        var summary = await _socialModule.ExecuteCommandAsync(new EndCallCommand(callId, DateTime.UtcNow));
        return Ok(summary);
    }
    
    [HttpGet("getAll")]
    [HasPermission(CallPermission.CanGetAllCalls)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CallDto>>> GetAllCalls(int? page, int? perPage)
    {
        var calls = await _socialModule.ExecuteQueryAsync(new GetAllCallsQuery(page, perPage));
        return Ok(calls);
    }
}