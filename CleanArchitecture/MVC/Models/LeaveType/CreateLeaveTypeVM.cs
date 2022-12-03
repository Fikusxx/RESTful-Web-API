using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class CreateLeaveTypeVM
{
    [Required]
    public string Name { get; set; }

    [Required]
    [Display(Name = "Number of days")]
    public int DefaultDays { get; set; }
}
