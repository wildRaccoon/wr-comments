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
        protected T _entry { get; set; }

        protected string _source_index { get; set; } 

        protected long _version { get; set; }
        #endregion

        #region constructor
        public EntryContext(T entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            _entry = entry;
        }

        public EntryContext(IHit<T> hit) : this(hit?.Source)
        {
            _source_index = hit.Index;
            _version = hit.Version.GetValueOrDefault();
        }
        #endregion

        #region operators
        public static implicit operator EntryContext<T>(T entry)
        {
            return new EntryContext<T>(entry);
        }
        #endregion
    }
}
