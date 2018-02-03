﻿using System;
using System.Collections.Generic;
using System.Text;
using wr.repository.context;
using wr.repository.unittest.contracts;
using Xunit;

namespace wr.repository.unittest.context
{
    public class EntryContext_Tests
    {   
        [Fact(DisplayName = "[EntryContext] without version check")]
        public static void CreateWithoutVersionCheck()
        {
            var entry = new ContractWithoutVersionCheck();

            Assert.Equal(BaseContract.VersionNotSet, entry.Version);
            Assert.Equal(IndicesList.Write, entry.Index);
        }

        [Fact(DisplayName = "[EntryContext] with version check")]
        public static void CreateWithVersionCheck()
        {
            var entry = new ContractWithVersionCheck();

            Assert.Equal(0l, entry.Version);
            Assert.Equal(IndicesList.Write, entry.Index);
        }

        [Fact(DisplayName = "[EntryContext] not registered in repo")]
        public static void CreateNotRegistered()
        {
            try
            {
                var entry = new ContractNotInRepository();
            }
            catch (ArgumentException ae)
            {
                Assert.NotNull(ae);
                Assert.Equal($"Type {nameof(ContractNotInRepository)} not signed as WR repository contract.", ae.Message);
            }
        }
    }
}
