using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace wr.repository.context
{
    public abstract class BaseContract
    {
        #region properties

        private string _source_index { get; set; } = string.Empty;

        private long _version { get; set; } = VersionNotSet;

        private bool _checkVersion = false;

        #region public
        public const long VersionNotSet = -1;

        [Number(Ignore = true)]
        public long Version => _version;

        [Text(Ignore = true)]
        public string Index => _source_index;

        #endregion

        #endregion

        #region constructor

        protected BaseContract()
        {
            InitializeEntry();
        }

        internal void InitializeEntry()
        {
            var attr = RepositoryEntryAttribute.FromType(this.GetType());
            if (attr == null)
            {
                throw new ArgumentException($"Type {this.GetType().Name} not signed as WR repository contract.");
            }

            if (attr.CheckVersion)
            {
                _checkVersion = true;
                _version = 0;
            }

            //new items will be populated into write index
            _source_index = attr.WriteAlias;
        }
        #endregion

        #region public
        internal void UpdateContext(string index, long version = VersionNotSet)
        {
            _source_index = index;

            if (_checkVersion)
            {
                if (version < _version)
                {
                    throw new ArgumentOutOfRangeException($"Current context version {_version} is greater then {version}.");
                }

                _version = version;
            }
        }
        #endregion
    }
}
