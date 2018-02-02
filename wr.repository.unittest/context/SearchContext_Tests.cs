using Elasticsearch.Net;
using Moq;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using wr.repository.context;
using wr.repository.unittest.contracts;
using Xunit;

namespace wr.repository.unittest.context
{
    public class SearchContext_Tests
    {
        [Fact(DisplayName = "[EntryContext] not registered in repo")]
        public static void CreateNotRegistered()
        {
            try
            {
                var sc = new SearchContext<ContractNotInRepository>();
            }
            catch (ArgumentException ae)
            {
                Assert.NotNull(ae);
                Assert.Equal($"Type {nameof(ContractNotInRepository)} not signed as WR repository contract.", ae.Message);
            }
        }

        #region search tests
        [Fact(DisplayName = "[EntryContext] search - without version check")]
        public static void SearchWithoutVersionCheck()
        {
            var sd = new SearchDescriptor<ContractWithoutVersionCheck>();

            var sc = new SearchContext<ContractWithoutVersionCheck>();
            ISearchRequest req = sc.ApplyContext(sd);

            Assert.False(req.Version);

            var data = ((IUrlParameter)req.Index).GetString(new ConnectionSettings());
            Assert.Equal(IndicesList.Read, data);
        }

        [Fact(DisplayName = "[EntryContext] with version check")]
        public static void CreateWithVersionCheck()
        {
            var sd = new SearchDescriptor<ContractWithVersionCheck>();

            var sc = new SearchContext<ContractWithVersionCheck>();
            ISearchRequest req = sc.ApplyContext(sd);

            Assert.True(req.Version);

            var data = ((IUrlParameter)req.Index).GetString(new ConnectionSettings());
            Assert.Equal(IndicesList.Read, data);
        }

        [Fact(DisplayName = "[EntryContext] search - without version check & only write index")]
        public static void SearchWithoutVersionCheckOnlyWriteIndex()
        {
            var sd = new SearchDescriptor<ContractWithoutVersionOnlyWriteIndex>();

            var sc = new SearchContext<ContractWithoutVersionOnlyWriteIndex>();
            ISearchRequest req = sc.ApplyContext(sd);

            Assert.False(req.Version);

            var data = ((IUrlParameter)req.Index).GetString(new ConnectionSettings());
            Assert.Equal(IndicesList.Write, data);
        }

        [Fact(DisplayName = "[EntryContext] search - only write index")]
        public static void SearchOnlyWriteIndex()
        {
            var sd = new SearchDescriptor<ContractOnlyWriteIndex>();

            var sc = new SearchContext<ContractOnlyWriteIndex>();
            ISearchRequest req = sc.ApplyContext(sd);

            Assert.True(req.Version);

            var data = ((IUrlParameter)req.Index).GetString(new ConnectionSettings());
            Assert.Equal(IndicesList.Write, data);
        } 
        #endregion
    }
}
