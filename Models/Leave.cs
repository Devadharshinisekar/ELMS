namespace ELMSApplication;
using System.ComponentModel.DataAnnotations;

public class Leave
{
    [Key]
    public int LeaveId { get; set; }
    public string? EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    [StringLength(75)]
    public string? LeaveType { get; set; }
    [StringLength(75)]
    public string? Status { get; set; }

}