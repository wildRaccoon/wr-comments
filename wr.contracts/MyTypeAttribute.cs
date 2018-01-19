using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace wr.contracts
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MyTypeAttribute : ElasticsearchTypeAttribute
    {
        public string ReadAlias { get; set; }

        public string WriteAlias { get; set; }

        public bool CheckVersion { get; set; }

        public bool UseSourceIndex { get; set; }
    }
}
