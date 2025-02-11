using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class EmployeeRole
    {
        [Required]
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
