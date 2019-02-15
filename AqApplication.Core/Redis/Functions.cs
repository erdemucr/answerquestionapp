using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AqApplication.Core.Redis
{
    public class Functions
    {
        //public async Task<List<object>> AddRedisCache(List<object> allData, int cacheTime, string cacheKey)
        //{
        //    var dataNews = await _distributedCache.GetAsync(cacheKey);
        //    if (dataNews == null)
        //    {
        //        var data = JsonConvert.SerializeObject(allData);
        //        var dataByte = Encoding.UTF8.GetBytes(data);

        //        var option = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(cacheTime));
        //        option.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheTime);
        //        await _distributedCache.SetAsync(cacheKey, dataByte, option);
        //    }
        //    var newsString = await _distributedCache.GetStringAsync(cacheKey);
        //    return JsonConvert.DeserializeObject<List<object>>(newsString);
        //}

    }
}
