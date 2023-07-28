using API.Contracts;
using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Models;
using API.Utilities;

namespace API.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEducationRepository _educationRepository;
        private readonly IUniversityRepository _universityRepository;
        public EmployeeService(IEmployeeRepository employeeRepository, IEducationRepository educationRepository, IUniversityRepository universityRepository)
        {
            _employeeRepository = employeeRepository;
            _educationRepository = educationRepository;
            _universityRepository = universityRepository;
        }

        public EmployeeService(IEmployeeRepository employeeRepository) 
        {
            _employeeRepository = employeeRepository;
        }

        public IEnumerable<GetEmployeeDto>? GetEmployee()
        {
            var employees = _employeeRepository.GetAll();
            if (!employees.Any())
            {
                return null; // No employee  found
            }

            var toDto = employees.Select(employee =>
                                                new GetEmployeeDto
                                                {
                                                    Guid = employee.Guid,
                                                    NIK = employee.NIK,
                                                    BirthDate = employee.BirthDate,
                                                    Email = employee.Email,
                                                    FirstName = employee.FirstName,
                                                    LastName = employee.LastName,
                                                    Gender = employee.Gender,
                                                    HiringDate = employee.HiringDate,
                                                    PhoneNumber = employee.PhoneNumber
                                                }).ToList();

            return toDto; // employee found
        }

        public GetEmployeeDto? GetEmployee(Guid guid)
        {
            var employee = _employeeRepository.GetByGuid(guid);
            if (employee is null)
            {
                return null; // employee not found
            }

            var toDto = new GetEmployeeDto
            {
                Guid = employee.Guid,
                NIK = employee.NIK,
                BirthDate = employee.BirthDate,
                Email = employee.Email,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                HiringDate = employee.HiringDate,
                PhoneNumber = employee.PhoneNumber
            };
            return toDto; // employees found
        }

        public GetEmployeeDto? CreateEmployee(NewEmployeeDto newEmployeeDto)
        {

            var employee = new Employee
            {
                Guid = new Guid(),
                NIK = GenerateNikByService(),
                PhoneNumber = newEmployeeDto.PhoneNumber,
                FirstName = newEmployeeDto.FirstName,
                LastName = newEmployeeDto.LastName,
                Gender = newEmployeeDto.Gender,
                HiringDate = newEmployeeDto.HiringDate,
                Email = newEmployeeDto.Email,
                BirthDate = newEmployeeDto.BirthDate,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };


            var createdEmployee = _employeeRepository.Create(employee);
            if (createdEmployee is null)
            {
                return null; // employee not created
            }

            var toDto = new GetEmployeeDto
            {
                Guid = employee.Guid,
                NIK = employee.NIK,
                BirthDate = employee.BirthDate,
                Email = employee.Email,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                HiringDate = employee.HiringDate,
                PhoneNumber = employee.PhoneNumber
            };

            return toDto; // employee created
        }

        public int UpdateEmployee(UpdateEmployeeDto updateEmployeeDto)
        {
            var isExist = _employeeRepository.IsExist(updateEmployeeDto.Guid);
            if (!isExist)
            {
                return -1; // employee not found
            }

            var getEmployee = _employeeRepository.GetByGuid(updateEmployeeDto.Guid);

            var employee = new Employee
            {
                Guid = updateEmployeeDto.Guid,
                PhoneNumber = updateEmployeeDto.PhoneNumber,
                FirstName = updateEmployeeDto.FirstName,
                LastName = updateEmployeeDto.LastName,
                Gender = updateEmployeeDto.Gender,
                HiringDate = updateEmployeeDto.HiringDate,
                Email = updateEmployeeDto.Email,
                BirthDate = updateEmployeeDto.BirthDate,
                NIK = updateEmployeeDto.NIK,
                ModifiedDate = DateTime.Now,
                CreatedDate = getEmployee!.CreatedDate
            };

            var isUpdate = _employeeRepository.Update(employee);
            if (!isUpdate)
            {
                return 0; // employee not updated
            }

            return 1;
        }

        public int DeleteEmployee(Guid guid)
        {
            var isExist = _employeeRepository.IsExist(guid);
            if (!isExist)
            {
                return -1; // employee not found
            }

            var employee = _employeeRepository.GetByGuid(guid);
            var isDelete = _employeeRepository.Delete(employee!);
            if (!isDelete)
            {
                return 0; // employee not deleted
            }

            return 1;
        }

        //generate nik by employee service
        public string GenerateNikByService()
        {
            int Nik = 111111;
            var employee = GetEmployee();
            if (employee is null)
            {
                Convert.ToString(Nik);
                return Convert.ToString(Nik);
            }
            var lastEmployee = employee.OrderByDescending(e => e.NIK).FirstOrDefault();
            int newNik = Int32.Parse(lastEmployee.NIK) + 1;
            string lastNik = Convert.ToString(newNik);
            return lastNik;
        }

        public OtpResponseDto? GetByEmail(string email)
        {
            var account = _employeeRepository.GetAll()
                .FirstOrDefault(e => e.Email.Contains(email));

            if (account != null)
            {
                return new OtpResponseDto
                {
                    Email = account.Email,
                    Guid = account.Guid
                };
            }

            return null;
        }

        public IEnumerable<EmployeeDetailDto> GetAllEmployeeDetail()
        {
            var result = from employee in _employeeRepository.GetAll()
                         join education in _educationRepository.GetAll() on employee.Guid equals education.Guid
                         join university in _universityRepository.GetAll() on education.UniversityGuid equals
                             university.Guid
                         select new EmployeeDetailDto
                         {
                             EmployeeGuid = employee.Guid,
                             NIK = employee.NIK,
                             FullName = employee.FirstName + " " + employee.LastName,
                             BirthDate = employee.BirthDate,
                             Gender = employee.Gender,
                             HiringDate = employee.HiringDate,
                             Email = employee.Email,
                             PhoneNumber = employee.PhoneNumber,
                             Major = education.Major,
                             Degree = education.Degree,
                             GPA = education.GPA,
                             UniversityName = university.Name
                         };

            return result;
        }

        public EmployeeDetailDto? GetAllEmployeeDetailByGuid(Guid guid)
        {
            return GetAllEmployeeDetail().SingleOrDefault(e => e.EmployeeGuid == guid);
        }
    }
}
