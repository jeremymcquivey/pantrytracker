using System;
using System.Threading.Tasks;

namespace PantryTracker.Model
{
    public interface ICacheManager
    {
        Task<T> GetAsync<T>(string key, Func<Task<T>> refreshDelegate, TimeSpan validity);

        T Get<T>(string key, Func<T> refreshDelegate, TimeSpan validity);

        void Add<T>(string key, T toCache, TimeSpan validity);
    }
}
