using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.Cache
{
    public class RedisHelper: ICacheHelper
    {
        private readonly IDatabase database;
        private readonly ConnectionMultiplexer connection;
        private readonly string instanceName; 
        private readonly string lockPrefix = "Lock_";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        /// <param name="database"></param>
        //public RedisHelper(RedisCacheOptions options, int database = 0)
        public RedisHelper(int database = 0)
        {
            this.connection = ConnectionMultiplexer.Connect("");
            this.database = connection.GetDatabase(database);
            this.instanceName = "InstanceName";
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key)
        {
            string json = database.StringGet(instanceName + key);
            return json;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="TValue">缓存值类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>缓存值</returns>
        public TValue Get<TValue>(string key)
        {
            string json = database.StringGet(instanceName + key);
            try
            {
                if (!string.IsNullOrEmpty(json))
                {
                    return JsonConvert.DeserializeObject<TValue>(json);
                }
            }
            catch (Exception ex) { }
            return default(TValue);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="TValue">缓存值类型</typeparam>
        /// <param name="keys">键集合</param>
        /// <returns>缓存值集合</returns>
        public List<TValue> Get<TValue>(List<string> keys)
        {
            List<TValue> list = new List<TValue>();
            foreach (string key in keys)
            {
                TValue value = JsonConvert.DeserializeObject<TValue>(database.StringGet(instanceName + key));
                list.Add(value);
            }
            return list;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="TValue">缓存值类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期时间,默认一天</param>
        /// <param name="isSliding">是否滑动过期</param>
        /// <returns>是否成功</returns>
        public bool Set<TValue>(string key, TValue value, double expires = 1, bool isSliding = false)
        {
            return database.StringSet(instanceName + key, JsonConvert.SerializeObject(value), TimeSpan.FromDays(expires));
        }

        /// <summary>
        /// 判断缓存是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否存在</returns>
        public bool Exists(string key)
        {
            return database.KeyExists(instanceName + key);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否成功</returns>
        public bool Remove(string key)
        {
            try
            {
                database.KeyDelete(instanceName + key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="keys">键集合</param>
        /// <returns>是否成功</returns>
        public bool Remove(List<string> keys)
        {
            try
            {
                foreach (string key in keys)
                {
                    database.KeyDelete(instanceName + key);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #region 分布式锁
        /// <summary>
        /// 加锁
        /// </summary>
        /// <param name="key">需要加锁的锁名</param>
        /// <param name="value">需要加锁的值（释放时需要用到）</param>
        /// <param name="expireTimeSeconds">该锁自动到期时间，单位秒默认1小时，  如果没其他要求可设置为最大时常TimeSpan.MaxValue   该方式一定要手动解锁</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool GetLock(string key, string value, long expireTimeSeconds = 60 * 60)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            try
            {
                key = lockPrefix + key;
                bool lockflag = database.LockTake(key, value, TimeSpan.FromSeconds(expireTimeSeconds));
                if (!lockflag)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Redis加锁异常:原因{ex.Message}");
            }
        }

        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="key">需要解锁的锁名</param>
        /// <param name="values">需要解锁的值</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool UnLock(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            try
            {
                key = lockPrefix + key;
                return database.LockRelease(key, value);
            }
            catch (Exception ex)
            {
                throw new Exception($"Redis加锁异常:原因{ex.Message}");
            }
        }
        #endregion
    }
}
