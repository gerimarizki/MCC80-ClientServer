using API.DTOs.Accounts;
using API.Utilities;
using API.DTOs.Employees;
using API.Models;

namespace Client.Contracts
{
    public interface IAccountRepository : IRepository<LoginDto, Guid>
    {
        public Task<HandlerForResponseEntity<TokenDTO>> Login(LoginDto entity);
    }
}
