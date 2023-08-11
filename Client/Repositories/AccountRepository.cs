using API.DTOs.Accounts;
using Newtonsoft.Json;
using System.Text;
using API.Utilities;
using Client.Contracts;

namespace Client.Repositories
{
    public class AccountRepository : GeneralRepository<LoginDto, Guid>, IAccountRepository
    {
        private readonly string request;
        private readonly HttpClient httpClient;
        public AccountRepository(string request = "Accounts/") : base(request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7124/api/")
            };
        }

        public async Task<HandlerForResponseEntity<TokenDTO>> Login(LoginDto entity)
        {
            HandlerForResponseEntity<TokenDTO> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(request + "Login", content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<HandlerForResponseEntity<TokenDTO>>(apiResponse);
            }
            return entityVM;
        }
    }
}
