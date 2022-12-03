using Microsoft.AspNetCore.Mvc;
using MVC.Contracts;
using MVC.Models;

namespace MVC.Controllers;


public class LeaveTypeController : Controller
{
    private readonly ILeaveTypeService leaveTypeService;
    private readonly ILeaveAllocationService leaveAllocationService;

    public LeaveTypeController(ILeaveTypeService leaveTypeService, ILeaveAllocationService leaveAllocationService)
    {
        this.leaveTypeService = leaveTypeService;
        this.leaveAllocationService = leaveAllocationService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var leaveTypes = await leaveTypeService.GetLeaveTypes();

        return View(leaveTypes);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateLeaveTypeVM createLeaveTypeVM)
    {
        try
        {
            var response = await leaveTypeService.CreateLeaveType(createLeaveTypeVM);

            if (response.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", response.ValidationErrors);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }

        return View(createLeaveTypeVM);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var leaveTypeVM = await leaveTypeService.GetLeaveTypeDetails(id);

        return View(leaveTypeVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(LeaveTypeVM leaveTypeVM)
    {
        try
        {
            var response = await leaveTypeService.UpdateLeaveType(leaveTypeVM);

            if (response.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", response.ValidationErrors);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }

        return View(leaveTypeVM);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var leaveType = await leaveTypeService.GetLeaveTypeDetails(id);

        return View(leaveType);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var leaveTypeVM = await leaveTypeService.GetLeaveTypeDetails(id);

        return View(leaveTypeVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePost(int id)
    {
        try
        {
            var response = await leaveTypeService.DeleteLeaveType(id);

            if (response.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", response.ValidationErrors);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }

        return BadRequest();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Allocate(int id)
    {
        try
        {
            var response = await leaveAllocationService.CreateLeaveAllocations(id);

            if (response.Success)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }

        return BadRequest();
    }
}
