using Application.Features;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LeaveTypeController : ControllerBase
{
    private readonly IMediator mediator;

    public LeaveTypeController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<LeaveTypeDTO>>> GetAll()
    {
        var leaveTypes = await mediator.Send(new GetLeaveTypeListRequest());

        return Ok(leaveTypes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LeaveTypeDTO>> Get(int id)
    {
        var leaveType = await mediator.Send(new GetLeaveTypeDetailsRequest() { Id = id });

        return Ok(leaveType);
    }

    [HttpPost]
    public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateLeaveTypeDTO createLeaveTypeDTO)
    {
        var command = new CreateLeaveTypeCommand() { CreateLeaveTypeDTO = createLeaveTypeDTO };
        var response = await mediator.Send(command);

        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<BaseCommandResponse>> Put([FromBody] LeaveTypeDTO leaveTypeDTO)
    {
        var command = new UpdateLeaveTypeCommand() { LeaveTypeDTO = leaveTypeDTO };
        var response = await mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseCommandResponse>> Delete(int id)
    {
        var command = new DeleteLeaveTypeCommand() { Id = id };
        var response = await mediator.Send(command);

        return NoContent();
    }
}
