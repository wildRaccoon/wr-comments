using Nest;
using System;
using wr.contracts.common;

namespace wr.contracts
{
    [MyTypeAttribute(IdProperty = "Id", ReadAlias = WRIndexAliases.Read, WriteAlias = WRIndexAliases.Write)]
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
