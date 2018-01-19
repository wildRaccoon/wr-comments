using Nest;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace wr.repository
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RepositoryEntryAttribute : ElasticsearchTypeAttribute
    {
        public string ReadAlias { get; set; }

        public string WriteAlias { get; set; }

        public bool CheckVersion { get; set; }

        public bool UseSourceIndex { get; set; }

        #region resolved types
        private static readonly ConcurrentDictionary<Type, RepositoryEntryAttribute> CachedTypeLookups =
        new ConcurrentDictionary<Type, RepositoryEntryAttribute>();

        public static RepositoryEntryAttribute FromType(Type type)
        {
            if (CachedTypeLookups.TryGetValue(type, out var attr))
                return attr;

            var attributes = type.GetTypeInfo().GetCustomAttributes(typeof(ElasticsearchTypeAttribute), true);
            if (attributes.Any())
                attr = (RepositoryEntryAttribute)attributes.First();
            CachedTypeLookups.TryAdd(type, attr);
            return attr;
        } 
        #endregion
    }
}
