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

        [Date]
        public DateTime CreateDate { get; set; }

        [Text]
        public string UserIdenty { get; set; }

        [Text]
        public string ArtitleId { get; set; }

        /// <summary>
        /// /id{1}[YYYY-MM-DD HH-mm-SS]/id{2}[YYYY-MM-DD HH-mm-SS] ....
        /// </summary>
        [Text]
        public string Path { get; set; }

        [Number(NumberType.Integer)]
        public int Level { get; set; } = 1;
    }
}
