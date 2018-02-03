using System;
using System.Collections.Generic;
using System.Text;
using Nest;
using wr.repository.interfaces;

namespace wr.repository.context
{
    public class SearchContext<T> : ISearchContext<T>
        where T : BaseContract
    {
        #region properties
        private string _read_index { get; set; }

        private string _write_index { get; set; }

        private bool _check_version { get; set; }

        protected string _search_working_index = string.Empty;
        #endregion

        #region constructor
        public SearchContext()
        {
            var attr = RepositoryEntryAttribute.FromType(typeof(T));
            if (attr == null)
            {
                throw new ArgumentException($"Type {typeof(T).Name} not signed as WR repository contract.");
            }

            _read_index = attr.ReadAlias ?? string.Empty;
            _write_index = attr.WriteAlias ?? string.Empty;

            if (string.IsNullOrEmpty(_write_index))
            {
                throw new Exception($"Write index on type {nameof(T)} not available.");
            }

            _search_working_index = !string.IsNullOrEmpty(_read_index) ? _read_index : _write_index;

            _check_version = attr.CheckVersion;
        }
        #endregion

        #region ISearchContext<T>
        //Operation: Search
        public SearchDescriptor<T> ApplyContext(SearchDescriptor<T> sd)
        {
            return sd.Index(_search_working_index).Version(_check_version);
        }

        //Operation: Add
        public IIndexRequest<T> ApplyContext(IndexDescriptor<T> sd)
        {
            return sd.Index(_write_index);
        }

        //Operation: Update
        public IIndexRequest<T> ApplyContext(IndexDescriptor<T> sd, T entry)
        {
            if (_check_version)
            {
                return sd.Index(entry.Index).Version(entry.Version);
            }
            else
            {
                return sd.Index(entry.Index);
            }
        }

        //Operation: Delete
        public IDeleteRequest ApplyContext(DeleteDescriptor<T> sd, T entry)
        {
            if (_check_version)
            {
                return sd.Index(entry.Index).Version(entry.Version);
            }
            else
            {
                return sd.Index(entry.Index);
            }
        }
        #endregion
    }
}
