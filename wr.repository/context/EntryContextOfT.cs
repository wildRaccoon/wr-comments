using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace wr.repository.context
{
    public class EntryContext<T>
        where T : class
    {
        #region properties
        protected T _entry { get; set; } = null;

        protected string _source_index { get; set; } = string.Empty;

        protected long _version { get; set; } = VERSION_NOT_SET;

        #region public
        public const long VERSION_NOT_SET = -1;

        public T Item { get { return _entry; } }

        public long Version { get { return _version; } }

        public string Index { get { return _source_index; } }
        #endregion
        #endregion

        #region constructor
        public EntryContext(T entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            var attr = RepositoryEntryAttribute.FromType(typeof(T));
            if (attr == null)
            {
                throw new ArgumentNullException(nameof(attr));
            }

            if (attr.CheckVersion)
            {
                _version = 0;
            }

            //new items will be populated into write index
            _source_index = attr.WriteAlias;

            _entry = entry;
        }

        public EntryContext(IHit<T> hit) : this(hit?.Source)
        {
            _source_index = hit.Index;
            _version = hit.Version.GetValueOrDefault(VERSION_NOT_SET);
        }
        #endregion

        #region operators
        public static implicit operator EntryContext<T>(T entry)
        {
            return new EntryContext<T>(entry);
        }

        public static implicit operator T (EntryContext<T>  entry)
        {
            return entry.Item;
        }
        #endregion
    }
}
