using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.Cache
{
    public interface ICacheHelper
    {
        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetString(string key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="TValue">缓存值类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>缓存值</returns>
        TValue Get<TValue>(string key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="TValue">缓存值类型</typeparam>
        /// <param name="keys">键集合</param>
        /// <returns>缓存值集合</returns>
        List<TValue> Get<TValue>(List<string> keys);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="TValue">缓存值类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期时间</param>
        /// <param name="isSliding">是否滑动过期</param>
        /// <returns>是否成功</returns>
        bool Set<TValue>(string key, TValue value, double expires = 0, bool isSliding = false);

        /// <summary>
        /// 判断缓存是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否存在</returns>
        bool Exists(string key);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否成功</returns>
        bool Remove(string key);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="keys">键集合</param>
        /// <returns>是否成功</returns>
        bool Remove(List<string> keys);
        /// <summary>
        /// 加锁
        /// </summary>
        /// <param name="key">需要加锁的锁名</param>
        /// <param name="values">需要加锁的值（释放时需要用到）</param>
        /// <param name="expireTimeSeconds">该锁自动到期时间，单位秒默认1小时，  如果没其他要求可设置为最大时常TimeSpan.MaxValue   该方式一定要手动解锁</param>
        /// <returns></returns> 
        bool GetLock(string key, string values, long expireTimeSeconds = 60 * 60);
        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="key">需要解锁的锁名</param>
        /// <param name="values">需要解锁的值</param>
        /// <returns></returns> 
        bool UnLock(string key, string values);
    }
}
