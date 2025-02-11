using EmployeeManagement.Data;
using EmployeeManagement.Models;
using EmployeeManagement.Models.EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services
{
    public class EmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeResponseDto>> GetActiveEmployeesAsync()
        {
            return await _context.Employees
                .Where(e => !e.IsDeleted)
                .Select(e => new EmployeeResponseDto
                {
                    Name = e.Name,
                    IdNumber = e.IdNumber,
                    RoleName = e.Role.RoleName,
                    ManagerName = e.Manager != null ? e.Manager.Name : null
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeResponseDto>> GetActiveManagersAsync()
        {
            var managerRoleIds = new[] { 664, 322 };
            return await _context.Employees
                .Where(e => !e.IsDeleted && managerRoleIds.Contains(e.RoleId))
                .Select(e => new EmployeeResponseDto
                {
                    Name = e.Name,
                    IdNumber = e.IdNumber,
                    RoleName = e.Role.RoleName,
                    ManagerName = e.Manager != null ? e.Manager.Name : null
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeResponseDto>> GetActiveOSEmployeesAsync()
        {
            return await _context.Employees
                .Where(e => !e.IsDeleted && e.RoleId == 876)
                .Select(e => new EmployeeResponseDto
                {
                    Name = e.Name,
                    IdNumber = e.IdNumber,
                    RoleName = e.Role.RoleName,
                    ManagerName = e.Manager != null ? e.Manager.Name : null
                })
                .ToListAsync();
        }

        public async Task<EmployeeResponseDto?> GetEmployeeByIdAsync(string id)
        {
            return await _context.Employees
                .Where(e => e.IdNumber == id)
                .Select(e => new EmployeeResponseDto
                {
                    Name = e.Name,
                    IdNumber = e.IdNumber,
                    RoleName = e.Role.RoleName,
                    ManagerName = e.Manager != null ? e.Manager.Name : null
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Employee?> CreateEmployeeAsync(EmployeeRequestDto employeeDto)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == employeeDto.RoleId);
            if (role == null) return null;

            Employee? manager = null;
            if (employeeDto.ManagerId != null)
            {
                manager = await _context.Employees
                    .Where(e => e.IdNumber == employeeDto.ManagerId && !e.IsDeleted && (e.RoleId == 664 || e.RoleId == 322))
                    .FirstOrDefaultAsync();

                if (manager == null) return null;
            }

            var employee = new Employee
            {
                Name = employeeDto.Name,
                IdNumber = employeeDto.IdNumber,
                RoleId = role.RoleId,
                ManagerId = manager?.IdNumber,
                Created = DateTime.UtcNow,
                IsDeleted = false
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> UpdateEmployeeAsync(string id, EmployeeRequestDto employeeDto)
        {
            if (id != employeeDto.IdNumber) return false;

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == employeeDto.RoleId);
            if (role == null) return false;

            Employee? manager = null;
            if (employeeDto.ManagerId != null)
            {
                if (employeeDto.ManagerId == id) return false;

                manager = await _context.Employees
                    .Where(e => e.IdNumber == employeeDto.ManagerId && !e.IsDeleted && (e.RoleId == 664 || e.RoleId == 322))
                    .FirstOrDefaultAsync();

                if (manager == null) return false;
            }

            employee.Name = employeeDto.Name;
            employee.RoleId = role.RoleId;
            employee.ManagerId = manager?.IdNumber;

            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEmployeeAsync(string id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            employee.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EmployeeResponseDto>> GetDeletedEmployeesAsync()
        {
            return await _context.Employees
                .Where(e => e.IsDeleted)
                .Select(e => new EmployeeResponseDto
                {
                    Name = e.Name,
                    IdNumber = e.IdNumber,
                    RoleName = e.Role.RoleName,
                    ManagerName = e.Manager != null ? e.Manager.Name : null
                })
                .ToListAsync();
        }
    }
}
