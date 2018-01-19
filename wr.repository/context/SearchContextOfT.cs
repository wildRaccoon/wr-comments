using System;
using System.Collections.Generic;
using System.Text;

namespace wr.repository.context
{
    public class SearchContext<T>
        where T: class
    {
        #region properties
        private string _read_index { get; set; }

        private string _write_index { get; set; }

        private bool _check_version { get; set; }

        private bool _use_source_index { get; set; }
        #endregion

        #region constructor
        public SearchContext(RepositoryEntryAttribute attr)
        {
            if (attr == null)
            {
                throw new ArgumentNullException(nameof(attr));
            }

            _read_index = attr.ReadAlias ?? string.Empty;
            _write_index = attr.WriteAlias ?? string.Empty;
            _check_version = attr.CheckVersion;
            _use_source_index = attr.UseSourceIndex;
        }
        #endregion
    }
}
