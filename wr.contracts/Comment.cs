using Nest;
using System;
using System.Collections.Generic;
using wr.repository;

namespace wr.contracts
{
    [RepositoryEntry(IdProperty = "Id", ReadAlias = WRIndexAliases.Read, WriteAlias = WRIndexAliases.Write, CheckVersion = true)]
    public class Comment
    {
        [Text]
        public string Id { get; set; }

        [Text]
        public string Content { get; set; }
        
        [Nested]
        public List<string> Tags { get; set; }
    }
}
