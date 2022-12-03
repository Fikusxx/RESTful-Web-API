using Application.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class LeaveRequestController : ControllerBase
{
    private readonly IMediator mediator;

    public LeaveRequestController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<LeaveRequestDTO>>> GetAll()
    {
        var request = new GetLeaveAllocationListRequest();
        var leaveRequests = await mediator.Send(request);

        return Ok(leaveRequests);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<LeaveRequestDTO>> Get(int id)
    {
        var request = new GetLeaveAllocationDetailsRequest() { Id = id };
        var leaveRequest = await mediator.Send(request);

        return Ok(leaveRequest);
    }

    [HttpPost]
    public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateLeaveRequestDTO createLeaveRequestDTO)
    {
        var command = new CreateLeaveRequestCommand() { CreateLeaveRequestDTO = createLeaveRequestDTO };
        var response = await mediator.Send(command);

        return NoContent();
    }

    [HttpPut]
    public async Task<ActionResult<BaseCommandResponse>> Put([FromBody] UpdateLeaveRequestDTO updateLeaveRequestDTO)
    {
        var command = new UpdateLeaveRequestCommand()
        {
            Id = updateLeaveRequestDTO.Id,
            UpdateLeaveRequestDTO = updateLeaveRequestDTO
        };
        var response = await mediator.Send(command);

        return NoContent();
    }

    [HttpPut]
    [Route("ChangeApproval")]
    public async Task<ActionResult<BaseCommandResponse>> Put([FromBody] ChangeLeaveRequestApprovalDTO changeLeaveRequestApprovalDTO)
    {
        var command = new UpdateLeaveRequestCommand()
        { 
            Id = changeLeaveRequestApprovalDTO.Id, 
            ChangeLeaveRequestApprovalDTO = changeLeaveRequestApprovalDTO 
        };

        var response = await mediator.Send(command);

        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<BaseCommandResponse>> Delete(int id)
    {
        var command = new DeleteLeaveRequestCommand() { Id = id };
        var response = await mediator.Send(command);

        return NoContent();
    }
}
