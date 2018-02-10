using System;
using System.Collections.Generic;
using Nest;
using wr.repository;
using wr.repository.context;

namespace service.comments.Contracts
{
    [RepositoryEntry(IdProperty = "Id", ReadAlias = IndexAliases.Read, WriteAlias = IndexAliases.Write, CheckVersion = true)]
    public class Comment : BaseContract
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

        [Text]
        public string ArtitleId { get; set; }

        /// <summary>
        /// /[YYYYMMDDHHmmSS0000]-[id1]/[YYYYMMDDHHmmSS0000]-[id2] ....
        /// </summary>
        [Text]
        public string Path { get; set; }

        [Number(NumberType.Integer)]
        public int Level { get; set; } = 1;
    }
}