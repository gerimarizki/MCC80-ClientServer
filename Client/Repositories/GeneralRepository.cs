using API.Utilities;
using Client.Contracts;
using Newtonsoft.Json;
using System.Text;

namespace Client.Repositories
{
    public class GeneralRepository<Entity, TId> : IRepository<Entity, TId>
        where Entity : class
    {
        private readonly string request;
        private readonly HttpClient httpClient;

        public GeneralRepository(string request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7124/api/")
            };
            this.request = request;
        }

        public async Task<HandlerForResponseEntity<Entity>> Delete(TId id)
        {
            HandlerForResponseEntity<Entity> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");

            using (var response = httpClient.DeleteAsync(request + "?guid=" + id).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<HandlerForResponseEntity<Entity>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<HandlerForResponseEntity<IEnumerable<Entity>>> Get()
        {
            HandlerForResponseEntity<IEnumerable<Entity>> entityVM = null;
            using (var response = await httpClient.GetAsync(request))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<HandlerForResponseEntity<IEnumerable<Entity>>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<HandlerForResponseEntity<Entity>> Get(TId id)
        {
            HandlerForResponseEntity<Entity> entity = null;
            using (var response = await httpClient.GetAsync(request + id))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<HandlerForResponseEntity<Entity>>(apiResponse);
            }
            return entity;
        }


        public async Task<HandlerForResponseEntity<Entity>> Post(Entity entity)
        {
            HandlerForResponseEntity<Entity> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(request, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<HandlerForResponseEntity<Entity>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<HandlerForResponseEntity<Entity>> Put(TId guid, Entity entity)
        {
            HandlerForResponseEntity<Entity> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PutAsync(request + "?guid=" + guid, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<HandlerForResponseEntity<Entity>>(apiResponse);
            }
            return entityVM;
        }
    }
}