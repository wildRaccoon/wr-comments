using System;
using System.Collections.Generic;
using System.Text;

namespace wr.repository.interfaces
{
    public interface IBaseEntry<T>
    {
        T Id { get; set; }
    }
}
