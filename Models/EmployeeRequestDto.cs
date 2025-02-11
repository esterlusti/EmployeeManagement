namespace EmployeeManagement.Models
{
    public class EmployeeRequestDto
    {
        public string Name { get; set; }
        public string IdNumber { get; set; }
        public int RoleId { get; set; }
        public string? ManagerId { get; set; }
    }

}
