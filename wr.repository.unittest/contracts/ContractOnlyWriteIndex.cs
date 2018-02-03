using System;
using System.Collections.Generic;
using System.Text;
using wr.repository.context;

namespace wr.repository.unittest.contracts
{
    [RepositoryEntry(CheckVersion = true, WriteAlias = IndicesList.Write)]
    public class ContractOnlyWriteIndex : BaseContract
    {
    }
}
