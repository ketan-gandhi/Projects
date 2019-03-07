
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;

namespace CylanceGUID
{
    public class Caching : ICaching 
    {
        private readonly IDistributedCache _distributedCache;

        public Caching(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public void Add<GuidDataModel>(Guid key, GuidDataModel value)
        {
            var cachedItem = JsonConvert.SerializeObject(value);
            _distributedCache.SetString(key.ToString().ToUpper(), cachedItem);
        }

        public void Delete(Guid key)
        {
            _distributedCache.Remove(key.ToString());
        }

        public TItem Get<TItem>(Guid key) where TItem : class
        {
            var cachedItem = _distributedCache.GetString(key.ToString().ToUpper());
            if (!string.IsNullOrEmpty(cachedItem))
            {
                return JsonConvert.DeserializeObject<TItem>(cachedItem);
            }
            return null;
        }
    }
}

