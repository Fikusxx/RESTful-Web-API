using Application.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveAllocationController : ControllerBase
{
    private readonly IMediator mediator;

    public LeaveAllocationController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<LeaveAllocationDTO>>> GetAll()
    {
        var request = new GetLeaveAllocationListRequest();
        var leaveAllocations = await mediator.Send(request);

        return Ok(leaveAllocations);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<LeaveAllocationDTO>> Get(int id)
    {
        var request = new GetLeaveAllocationDetailsRequest() { Id = id };
        var leaveAllocation = await mediator.Send(request);

        return Ok(leaveAllocation);
    }

    [HttpPost]
    public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateLeaveAllocationDTO createLeaveAllocationDTO)
    {
        var command = new CreateLeaveAllocationCommand() { CreateLeaveAllocationDTO = createLeaveAllocationDTO };
        var response = await mediator.Send(command);    

        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<BaseCommandResponse>> Put([FromBody] UpdateLeaveAllocationDTO updateLeaveAllocationDTO)
    {
        var command = new UpdateLeaveAllocationCommand() { UpdateLeaveAllocationDTO = updateLeaveAllocationDTO };
        var response = await mediator.Send(command);

        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<BaseCommandResponse>> Delete(int id)
    {
        var command = new DeleteLeaveAllocationCommand() { Id = id };
        var response = await mediator.Send(command);

        return NoContent();
    }
}
