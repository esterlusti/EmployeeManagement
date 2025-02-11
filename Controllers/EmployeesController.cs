using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Services;
using EmployeeManagement.Models;
using EmployeeManagement.Models.EmployeeManagement.Models;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeesController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("ActiveEmployees")]
        public async Task<ActionResult<IEnumerable<EmployeeResponseDto>>> GetActiveEmployees()
        {
            var employees = await _employeeService.GetActiveEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("Managers")]
        public async Task<ActionResult<IEnumerable<EmployeeResponseDto>>> GetActiveManagers()
        {
            var managers = await _employeeService.GetActiveManagersAsync();
            return Ok(managers);
        }

        [HttpGet("OSEmployees")]
        public async Task<ActionResult<IEnumerable<EmployeeResponseDto>>> GetActiveContractors()
        {
            var contractors = await _employeeService.GetActiveOSEmployeesAsync();
            return Ok(contractors);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(EmployeeRequestDto employeeDto)
        {
            var employee = await _employeeService.CreateEmployeeAsync(employeeDto);
            if (employee == null)
            {
                return BadRequest("Invalid manager or role ID.");
            }
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.IdNumber }, employeeDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(string id, EmployeeRequestDto employeeDto)
        {
            var result = await _employeeService.UpdateEmployeeAsync(id, employeeDto);
            if (!result) return BadRequest();
            return NoContent();
        }

        [HttpGet("Deleted")]
        public async Task<ActionResult<IEnumerable<EmployeeResponseDto>>> GetDeletedEmployees()
        {
            var employees = await _employeeService.GetDeletedEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeResponseDto>> GetEmployee(string id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();
            return Ok(employee);
        }
    }
}
