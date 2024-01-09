namespace ELMSApplication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ELMSApplication.Properties.Attributes;
public class Employee
{
    [Key]
    public long Id { get; set; }
     [StringLength(75)]
    public string? EmployeeName { get; set; }
     [Required(ErrorMessage ="Please enter Employee ID")]
     [StringLength(75)]
    public string? EmployeeId { get; set; }
    [Required(ErrorMessage = "Password Required")]
    [PasswordValidation(ErrorMessage = "Your password must be at least 6 characters long, contain 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    public bool IsAdmin { get; set; }
    //[NotMapped]
     public bool KeepLoggedIn {get;internal set;}

}