using API.Contracts;
using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class UniversityRepository : GeneralRepository<University>, IUniversityRepository 
    {
        public UniversityRepository(BookingDbContext context) : base(context) { }

        public IEnumerable<University> GetByName(string name)
        {
            return _context.Set<University>().Where(u => u.Name.Contains(name));
        }

        public Guid GetLastUniversityGuid()
        {
            return _context.Set<University>().ToList().LastOrDefault().Guid;
        }

        public University? IsExist(string code)
        {
            return _context.Set<University>().SingleOrDefault(u => u.Code.Contains(code));
        }

        public University? GetByCode(string code)
        {
            return _context.Set<University>().SingleOrDefault(u => u.Code == code);
        }
    }
}
