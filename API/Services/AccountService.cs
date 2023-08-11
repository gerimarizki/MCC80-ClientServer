using API.Contracts;
using API.DTOs.AccountRoles;
using API.DTOs.Accounts;
using API.DTOs.Educations;
using API.DTOs.Employees;
using API.DTOs.Roles;
using API.Models;
using API.Repositories;
using API.Utilities;
using API.Utilities.Validations;
using System.Security.Claims;

namespace API.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUniversityRepository _universityRepository;
        private readonly IEducationRepository _educationRepository;
        private readonly IEmailHandler _emailHandler;
        private readonly ITokenHandler _tokenHandler;
        private readonly IAccountRoleRepository _accountRoleRepository;

        public AccountService(IAccountRepository accountRepository,
                         IEmployeeRepository employeeRepository,
                         IUniversityRepository universityRepository,
                         IEducationRepository educationRepository,
                         IEmailHandler emailHandler,
                         ITokenHandler tokenHandler,
                         IAccountRoleRepository accountRoleRepository)
        {
            _accountRepository = accountRepository;
            _employeeRepository = employeeRepository;
            _universityRepository = universityRepository;
            _educationRepository = educationRepository;
            _emailHandler = emailHandler;
            _tokenHandler = tokenHandler;
            _accountRoleRepository = accountRoleRepository;
        }

        public IEnumerable<GetAccountDto>? GetAccount()
        {
            var accounts = _accountRepository.GetAll();
            if (!accounts.Any())
            {
                return null; // No Account  found
            }
            var toDto = accounts.Select(account =>
                                                new GetAccountDto
                                                {
                                                    guid = account.Guid,
                                                    Password = account.Password,
                                                    IsDeleted = account.IsDeleted,
                                                    OTP = account.OTP,
                                                    IsUsed = account.IsUsed,
                                                    ExpiredDate = account.ExpiredDate,
                                                }).ToList();

            return toDto; // Account found
        }

        public GetAccountDto? GetAccount(Guid guid)
        {
            var account = _accountRepository.GetByGuid(guid);
            if (account is null)
            {
                return null; // account not found
            }

            var toDto = new GetAccountDto
            {
                guid = account.Guid,
                IsDeleted = account.IsDeleted,
                IsUsed = account.IsUsed,
            };
            return toDto; // accounts found
        }

        public GetAccountDto? CreateAccount(NewAccountDto newAccountDto)
        {
            var account = new Account
            {
                Guid = newAccountDto.Guid,
                Password = newAccountDto.Password,
                IsDeleted = newAccountDto.IsDeleted,
                OTP = newAccountDto.OTP,
                IsUsed = newAccountDto.IsUsed,
                ExpiredDate = newAccountDto.ExpiredDate,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var createdAccount = _accountRepository.Create(account);
            if (createdAccount is null)
            {
                return null; // Account not created
            }

            var toDto = new GetAccountDto
            {
                guid = createdAccount.Guid,
                Password = newAccountDto.Password,
                IsDeleted = newAccountDto.IsDeleted,
                OTP = newAccountDto.OTP,
                IsUsed = newAccountDto.IsUsed,
                ExpiredDate = newAccountDto.ExpiredDate,
            };
            return toDto; // Account created
        }

        public int UpdateAccount(UpdateAccountDto updateAccountDto)
        {
            var isExist = _accountRepository.IsExist(updateAccountDto.Guid);
            if (!isExist)
            {
                return -1; // Account not found
            }
            var getAccount = _accountRepository.GetByGuid(updateAccountDto.Guid);
            var account = new Account
            {
                Guid = updateAccountDto.Guid,
                IsUsed = updateAccountDto.IsUsed,
                Password = updateAccountDto.Password,
                IsDeleted = updateAccountDto.IsDeleted,
                ModifiedDate = DateTime.Now,
                CreatedDate = getAccount!.CreatedDate
            };
            var isUpdate = _accountRepository.Update(account);
            if (!isUpdate)
            {
                return 0; // Account not updated
            }
            return 1;
        }


        public int DeleteAccount(Guid guid)
        {
            var isExist = _accountRepository.IsExist(guid);
            if (!isExist)
            {
                return -1; // Account not found
            }

            var account = _accountRepository.GetByGuid(guid);
            var isDelete = _accountRepository.Delete(account!);
            if (!isDelete)
            {
                return 0; // Account not deleted
            }

            return 1;
        }

        public string Login(LoginDto loginDto)
        {
            var employeeAccount = from e in _employeeRepository.GetAll()
                                  join a in _accountRepository.GetAll() on e.Guid equals a.Guid
                                  where e.Email == loginDto.Email && HashingHandler.ValidateHash(loginDto.Password, a.Password)
                                  select new LoginDto()
                                  {
                                      Email = e.Email,
                                      Password = a.Password
                                  };

            if (!employeeAccount.Any())
            {
                return "0";
            }

            var employee = _employeeRepository.GetByEmail(loginDto.Email);

            var claims = new List<Claim>
            {
                new Claim("Guid", employee.Guid.ToString()),
                new Claim("FullName", $"{employee.FirstName} {employee.LastName}"),
                new Claim("Email", employee.Email)
            };

            var getRoles = _accountRoleRepository.GetRoleNamesByAccountGuid(employee.Guid);

            foreach (var getRole in getRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, getRole));
            }

            var generateToken = _tokenHandler.GenerateToken(claims);
            if (generateToken is null)
            {
                return "-1";
            }
            return generateToken;
        }

        public RegisterDto? Register(RegisterDto registerDto)
        {
            EmployeeService employeeService = new EmployeeService(_employeeRepository);

            Employee employee = new Employee

            {
                Guid = new Guid(),
                NIK = employeeService.GenerateNikByService(),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                BirthDate = registerDto.BirthDate,
                Gender = registerDto.Gender,
                HiringDate = registerDto.HiringDate,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var createdEmployee = _employeeRepository.Create(employee);
            if (createdEmployee is null)
            {
                return null;
            }

            University university = new University
            {
                Guid = new Guid(),
                Code = registerDto.UniversityCode,
                Name = registerDto.UniversityName
            };

            var createdUniversity = _universityRepository.Create(university);
            if (createdUniversity is null)
            {
                return null;
            }

            Education education = new Education
            {
                Guid = employee.Guid,
                Major = registerDto.Major,
                Degree = registerDto.Degree,
                GPA = registerDto.GPA,
                UniversityGuid = university.Guid
            };

            var createdEducation = _educationRepository.Create(education);
            if (createdEducation is null)
            {
                return null;
            }

            Account account = new Account
            {
                Guid = employee.Guid,
                Password = HashingHandler.GenerateHash(registerDto.Password),
                CreatedDate = employee.CreatedDate,
            };

            var accountRole = _accountRoleRepository.Create(new AccountRole
            {
                AccountGuid = account.Guid,
                RoleGuid = Guid.Parse("f2d7d3ea-a134-48b7-a21c-08db925d18be")
            });


            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return null;
            }

            var createdAccount = _accountRepository.Create(account);
            if (createdAccount is null)
            {
                return null;
            }


            var toDto = new RegisterDto
            {
                FirstName = createdEmployee.FirstName,
                LastName = createdEmployee.LastName,
                BirthDate = createdEmployee.BirthDate,
                Gender = createdEmployee.Gender,
                HiringDate = createdEmployee.HiringDate,
                Email = createdEmployee.Email,
                PhoneNumber = createdEmployee.PhoneNumber,
                Password = HashingHandler.GenerateHash(createdAccount.Password),
                Major = createdEducation.Major,
                Degree = createdEducation.Degree,
                GPA = createdEducation.GPA,
                UniversityCode = createdUniversity.Code,
                UniversityName = createdUniversity.Name
            };

            return toDto;
        }


        public int ForgotPassword(ForgotPasswordOTPDto forgotPassword)
        {
            var getAccountDetail = (from e in _employeeRepository.GetAll()
                                    join a in _accountRepository.GetAll() on e.Guid equals a.Guid
                                    where e.Email == forgotPassword.Email
                                    select a).FirstOrDefault();

            _accountRepository.Clear();

            if (getAccountDetail is null)
            {
                return 0;
            }

            var otp = new Random().Next(111111, 999999);
            var account = new Account
            {
                Guid = getAccountDetail.Guid,
                Password = getAccountDetail.Password,
                ExpiredDate = DateTime.Now.AddMinutes(5),
                OTP = otp,
                IsUsed = false,
                CreatedDate = getAccountDetail.CreatedDate,
                ModifiedDate = DateTime.Now
            };

            var isUpdated = _accountRepository.Update(account);

            if (!isUpdated)
                return -1;


            _emailHandler.SendEmail(forgotPassword.Email, 
                "Booking - Forgot Password", $"Your Otp is {otp}");
            return 1;
            }


        public int ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var getAccountDetail = (from e in _employeeRepository.GetAll()
                                    join a in _accountRepository.GetAll() on e.Guid equals a.Guid
                                    where e.Email == changePasswordDto.Email
                                    select a).FirstOrDefault();
            _accountRepository.Clear();
            if (getAccountDetail is null)
            {
                return -1; // Account not found
            }
            if (getAccountDetail.OTP != changePasswordDto.OTP)
            {
                return -2;
            }
            if (getAccountDetail.IsUsed)
            {
                return -3;
            }
            if (getAccountDetail.ExpiredDate < DateTime.Now)
            {
                return -4;
            }
            var account = new Account
            {
                Guid = getAccountDetail.Guid,
                IsUsed = true,
                ModifiedDate = DateTime.Now,
                CreatedDate = getAccountDetail.CreatedDate,
                OTP = getAccountDetail.OTP,
                ExpiredDate = getAccountDetail.ExpiredDate,
                Password = HashingHandler.GenerateHash(changePasswordDto.NewPassword)
            };

            var isUpdated = _accountRepository.Update(account);
            if (!isUpdated)
            {
                return -5; //Account Not Update
            }


            return 1;
        }

    } 

}

