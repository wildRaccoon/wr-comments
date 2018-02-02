using System;
using System.Collections.Generic;
using System.Text;

namespace wr.repository.unittest.contracts
{
    [RepositoryEntry(CheckVersion = true, WriteAlias = IndicesList.Write)]
    public class ContractOnlyWriteIndex
    {
    }
}
