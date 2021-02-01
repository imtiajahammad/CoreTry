using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTry.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SQLEmployeeRepository> _logger;

        public SQLEmployeeRepository(AppDbContext context, ILogger<SQLEmployeeRepository> logger)
        {
            this._context = context;
            this._logger = logger;
        }
        public Employee Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _context.Employees.Find(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
            return employee;
        }

        public IQueryable<Employee> GetAllEmployees()
        {
            var employees = _context.Employees;//.ToList().AsQueryable();
            return employees;
        }

        public IEnumerable<Employee> GetAllEmployees2()
        {
            throw new NotImplementedException();
        }

        public Employee GetEmployee(int id)
        {
            _logger.LogTrace("LogTrace log");
            _logger.LogDebug("LogDebug log");
            _logger.LogInformation("LogInformation log");
            _logger.LogWarning("LogWarning log");
            _logger.LogError("LogError log");
            _logger.LogCritical("LogCritical log");

            return _context.Employees.Find(id);
        }

        public Employee Update(Employee updatedEmployee)
        {
            var employee = _context.Employees.Attach(updatedEmployee);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return updatedEmployee;
        }
    }
}
