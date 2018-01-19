using System;
using System.Collections.Generic;
using System.Text;

namespace wr.contracts.common
{
    public interface IBaseEntry<T>
    {
        T Id { get; set; }
    }
}
