using KnowledgeSharingApi.Infrastructures.Interfaces.Caches;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Caches
{
    /// <summary>
    /// Cache lưu trữ dữ liệu dạng bảng băm
    /// </summary>
    /// Created: PhucTV (30/1/24)
    /// Modified: None
    public class HashTableCache : ICache
    {
        /// <summary>
        /// Triển khai pattern giả Singleton cho Cache
        /// Mọi đối tượng hướng về đối tượng đại diện: instance
        /// </summary>
        /// Created: PhucTV (30/1/24)
        /// Modified: None
        private static HashTableCache? instance = null;
        public static HashTableCache GetSingleton()
        {
            return instance ??= new HashTableCache();
        }

        /// <summary>
        /// Đóng vai trò là Queue chứa dữ liệu
        /// </summary>
        /// Created: PhucTV (26/1/24)
        /// Modified: None
        private Hashtable Data { get; set; } = [];  // ban đầu rỗng
        private Queue<string> queueOfKeys = new();  // hàng đợi các key để kiểm soát xem xóa key nào trước
        private int MaxSize = 100;                  // Cache lưu tối đa 100 bản ghi

        public bool Contains(string key)
        {
            return Data.Contains(key);
        }

        public object? Get(string key)
        {
            if (Contains(key))
            {
                return Data[key];
            }
            return null;
        }

        public void Set(string key, object value)
        {
            if (queueOfKeys.Contains(key))
            {
                Data.Add(key, value);
                return;
            }
            if (queueOfKeys.Count >= MaxSize)
            {
                string peek = queueOfKeys.Peek();
                Data.Remove(peek);
                queueOfKeys.Dequeue();
            }
            queueOfKeys.Enqueue(key);
            Data.Add(key, value);
        }

        public ICache GetInstance()
        {
            return HashTableCache.GetSingleton();
        }

        public T? Get<T>(string key)
        {
            return (T?)Data[key];
        }

        public void Set<T>(string key, T value)
        {
            Data.Add(key, value);
        }

        public void Remove(string key)
        {
            Data.Remove(key);
        }
    }
}
