﻿using System;
using System.Threading.Tasks;

namespace PantryTracker.Model
{
    public interface ICacheManager
    {
        Task<T> GetAsync<inT, T>(string key, Func<inT, Task<T>> refreshDelegate, inT delegateParameter, TimeSpan validity);

        Task<T> GetAsync<T>(string key, Func<Task<T>> refreshDelegate, TimeSpan validity);

        T Get<inT, T>(string key, Func<inT, T> refreshDelegate, inT delegateParameter, TimeSpan validity);

        T Get<T>(string key, Func<T> refreshDelegate, TimeSpan validity);

        T Get<T>(string key);

        void Add<T>(string key, T toCache, TimeSpan validity);
    }
}
