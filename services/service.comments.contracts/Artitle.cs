using Nest;
using System;
using System.Collections.Generic;
using wr.repository;
using wr.repository.context;

namespace service.comments.contracts
{
    [RepositoryEntry(IdProperty = "Id", ReadAlias = IndexAliases.Read, WriteAlias = IndexAliases.Write, CheckVersion = true)]
    public class Artitle : BaseContract
    {
        [Text]
        public string Id { get; set; }

        [Text]
        public string Content { get; set; }

        [Nested]
        public List<string> Tags { get; set; }

        [Date]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Text]
        public string UserIdenty { get; set; }
    }
}
