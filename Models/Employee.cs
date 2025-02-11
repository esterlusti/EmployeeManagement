using EmployeeManagement.Models;
using System.ComponentModel.DataAnnotations;

public class Employee
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string IdNumber { get; set; }

    public int RoleId { get; set; } // Foreign key property
    public EmployeeRole Role { get; set; } // Navigation property

    public string? ManagerId { get; set; }
    public Employee? Manager { get; set; } // Make Manager nullable

    public bool IsDeleted { get; set; } = false;
    public DateTime Created { get; set; } = DateTime.UtcNow;
}
