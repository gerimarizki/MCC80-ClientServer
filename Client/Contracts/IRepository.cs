using API.Utilities;

namespace Client.Contracts
{
    public interface IRepository<T, X>
         where T : class
    {
        Task<HandlerForResponseEntity<IEnumerable<T>>> Get();
        Task<HandlerForResponseEntity<T>> Get(X id);
        Task<HandlerForResponseEntity<T>> Post(T entity);
        Task<HandlerForResponseEntity<T>> Put(X id, T entity);
        Task<HandlerForResponseEntity<T>> Delete(X id);
    }
}