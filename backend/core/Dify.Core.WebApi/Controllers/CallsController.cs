using Dify.Common.Dto;
using Dify.Common.Models;
using Dify.Core.Application.Calls.Commands;
using Dify.Core.Application.Calls.Queries;
using Dify.Core.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dify.Core.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/calls")]
public class CallsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public CallsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("getCalls")]
    public async Task<ActionResult<QueryResponse<PaginatedList<CallDto>>>> GetCalls(int pageNumber, int pageSize)
    {
        var users = await _mediator.Send(
            new GetPaginatedCallsQuery(pageNumber, pageSize));
        return Ok(users);
    }
    
    [HttpGet("getById/{callId:guid}")]
    public async Task<ActionResult<QueryResponse<CurrentCallDto>>> GetById(Guid callId)
    {
        var users = await _mediator.Send(new GetCurrentCallByIdQuery(callId));
        return Ok(users);
    }
    
    [HttpPost("createNew")]
    public async Task<ActionResult<CommandResponse<NewCallResponse>>> CreateNewCall(CreateNewCallCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
    
    [HttpPost("joinCall")]
    public async Task<ActionResult<CommandResponse<CommandResponse<bool>>>> JoinCall(JoinCallCommand joinCallCommand)
    {
        var response = await _mediator.Send(joinCallCommand);
        return Ok(response);
    }
    
    [HttpPost("leftCall")]
    public async Task<ActionResult<CommandResponse<CommandResponse<bool>>>> LeftCall(LeftCallCommand leftCallCommand)
    {
        var response = await _mediator.Send(leftCallCommand);
        return Ok(response);
    }
    
    [HttpGet("searchParticipants")]
    public async Task<ActionResult<QueryResponse<ICollection<ParticipantForCallDto>>>> SearchParticipants(string searchValue)
    {
        var response = await _mediator.Send(new SearchParticipantsForCallQuery(searchValue));
        return Ok(response);
    }
}
