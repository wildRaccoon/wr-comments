using Nest;
using System;
using wr.repository;
using wr.repository.interfaces;

namespace wr.contracts
{
    [RepositoryEntry(IdProperty = "Id", ReadAlias = WRIndexAliases.Read, WriteAlias = WRIndexAliases.Write,UseSourceIndex = true, CheckVersion = true)]
    public class Comment : IBaseEntry<string>
    {
        [Text]
        public string Id { get; set; }

        [Text(Ignore = true)]
        public string SourceIndex { get; set; }

        [Text]
        public string Content { get; set; }
    }
}
