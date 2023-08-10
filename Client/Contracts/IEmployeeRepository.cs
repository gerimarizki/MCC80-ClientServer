using API.Models;
using API.DTOs.Employees;

namespace Client.Contracts
{
    public interface IEmployeeRepository : IRepository<Employee, Guid>
    {
    }
}