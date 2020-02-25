namespace MDA.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Caching;

    public class CacheManager
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;

        /// <summary>
        /// Add Item to Cache (T)
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="objectToCache">Object To Cache</param>
        /// <param name="key">Cache Key</param>
        /// <param name="cacheItemPolicy">Cache Item Policy</param>
        public static void Add<T>(T objectToCache, string key, CacheItemPolicy cacheItemPolicy = null) where T : class
        {
            cacheItemPolicy = cacheItemPolicy ?? new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddDays(30)) };
            Cache.Add(key, objectToCache, cacheItemPolicy);
        }

        /// <summary>
        /// Add Item to Cache
        /// </summary>
        /// <param name="objectToCache">Object To Cache</param>
        /// <param name="key">Cache Key</param>
        /// <param name="cacheItemPolicy">Cache Item Policy</param>
        public static void Add(object objectToCache, string key, CacheItemPolicy cacheItemPolicy = null)
        {
            cacheItemPolicy = cacheItemPolicy ?? new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddDays(30)) };
            Cache.Add(key, objectToCache, cacheItemPolicy);
        }

        /// <summary>
        /// Remove Item from Cache
        /// </summary>
        /// <param name="key">Cache Key</param>
        public static void Clear(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// Check if Item Exists in Cache
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <returns>True if Item Exists, False if Item does not Exist</returns>
        public static bool Exists(string key)
        {
            return Cache.Get(key) != null;
        }

        /// <summary>
        /// Get Cache Item
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Cache Key</param>
        /// <returns>Cached Item</returns>
        public static T Get<T>(string key) where T : class
        {
            try
            {
                return (T)Cache[key];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get All Cached Items
        /// </summary>
        /// <returns>All Items in Cache</returns>
        public static List<string> GetAll()
        {
            return Cache.Select(keyValuePair => keyValuePair.Key).ToList();
        }
    }
}