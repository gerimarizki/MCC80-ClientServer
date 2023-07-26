using API.Models;

namespace API.Contracts
{
    public interface IEmployeeRepository : IGeneralRepository<Employee>
    { 
        bool IsNotExist(string value);
        string GetPastNik();
        Employee GetByEmail(string email);
        Employee? CheckEmail(string email);
    }
}
