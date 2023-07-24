using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(BookingDbContext context) : base(context) { }

        public bool IsNotExist(string value)
        {
            return _context.Set<Employee>()
                .SingleOrDefault(employee => employee.Email.Contains(value) || employee.PhoneNumber.Contains(value))is null;
        }

    }
}
