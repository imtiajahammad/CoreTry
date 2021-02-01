using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTry.Models
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int id);

        IQueryable<Employee> GetAllEmployees();

        Employee Add(Employee employee);

        Employee Update(Employee updatedEmployee);

        Employee Delete(int id);
    }
}
