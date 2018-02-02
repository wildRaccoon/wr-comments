using System;
using System.Collections.Generic;
using System.Text;

namespace wr.repository.unittest.contracts
{
    [RepositoryEntry(CheckVersion = false, WriteAlias = IndicesList.Write)]
    public class ContractWithoutVersionOnlyWriteIndex
    {
    }
}
