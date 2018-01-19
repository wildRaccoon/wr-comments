using System;
using System.Collections.Generic;
using System.Text;
using Nest;
using wr.repository.interfaces;

namespace wr.repository.context
{
    public class SearchContext<T> : ISearchContext<T>
        where T : class
    {
        #region properties
        private string _read_index { get; set; }

        private string _write_index { get; set; }

        private bool _check_version { get; set; }

        private bool _use_source_index { get; set; }

        protected string _search_working_index = string.Empty;
        #endregion

        #region constructor
        public SearchContext()
        {
            var attr = RepositoryEntryAttribute.FromType(typeof(T));
            if (attr == null)
            {
                throw new ArgumentNullException(nameof(attr));
            }

            _read_index = attr.ReadAlias ?? string.Empty;
            _write_index = attr.WriteAlias ?? string.Empty;

            if (string.IsNullOrEmpty(_write_index))
            {
                throw new Exception($"Write index on type {nameof(T)} not available.");
            }

            _search_working_index = string.IsNullOrEmpty(_read_index) ? _read_index : _write_index;

            _check_version = attr.CheckVersion;
            _use_source_index = attr.UseSourceIndex;
        }
        #endregion

        #region ISearchContext<T>
        public SearchDescriptor<T> ApplyContext(SearchDescriptor<T> sd)
        {
            return sd.Index(_search_working_index).Version(_use_source_index);
        }
        #endregion
    }
}
