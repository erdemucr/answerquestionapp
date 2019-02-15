using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AqApplication.Core.Redis
{
    /// <summary>
    ///   CacheManager<string> cm = new CacheManager<string>(); //keep this instance in a class
    //      cm.Add("key1", "data1", 5000);  //remove after 5 sc
    //      cm.Add("key2", "data2");        //don't remove from cache
    //      cm.Add("key3", "data3", 3000);  //remove after 3 sc
    //  cm.Add("key4", "data4");    
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CacheManager<T>
    {
        private Dictionary<string, T> _items = null;
        public CacheManager()
        {
            _items = new Dictionary<string, T>();
        }

        private void TimerProc(object state)
        {
            //Remove the cached object
            string key = state.ToString();
            this.Remove(key);
        }

        public void Add(string key, T obj, int cacheTime = System.Threading.Timeout.Infinite)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key is null.");

            if (_items.Keys.Contains(key))
                throw new ArgumentException("An element with the same key already exists.");

            //Set timer
            System.Threading.Timer t = new System.Threading.Timer(new TimerCallback(TimerProc),
                                                                 key,
                                                                 cacheTime,
                                                                 System.Threading.Timeout.Infinite);
            _items.Add(key, obj);
        }

        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key is null.");

            return _items.Remove(key);
        }

        public T Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key is null.");

            if (!_items.Keys.Contains(key))
                throw new KeyNotFoundException("key does not exist in the collection.");

            return _items[key];
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}
