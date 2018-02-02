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
        #region General
        [Fact(DisplayName = "[SearchContext] not registered in repo")]
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
        #endregion

        #region search tests
        [Fact(DisplayName = "[SearchContext] search - without version check")]
        public static void SearchWithoutVersionCheck()
        {
            var sd = new SearchDescriptor<ContractWithoutVersionCheck>();

            var sc = new SearchContext<ContractWithoutVersionCheck>();
            ISearchRequest req = sc.ApplyContext(sd);

            Assert.False(req.Version);

            var data = ((IUrlParameter)req.Index).GetString(new ConnectionSettings());
            Assert.Equal(IndicesList.Read, data);
        }

        [Fact(DisplayName = "[SearchContext] search - with version check")]
        public static void CreateWithVersionCheck()
        {
            var sd = new SearchDescriptor<ContractWithVersionCheck>();

            var sc = new SearchContext<ContractWithVersionCheck>();
            ISearchRequest req = sc.ApplyContext(sd);

            Assert.True(req.Version);

            var data = ((IUrlParameter)req.Index).GetString(new ConnectionSettings());
            Assert.Equal(IndicesList.Read, data);
        }

        [Fact(DisplayName = "[SearchContext] search - without version check & only write index")]
        public static void SearchWithoutVersionCheckOnlyWriteIndex()
        {
            var sd = new SearchDescriptor<ContractWithoutVersionOnlyWriteIndex>();

            var sc = new SearchContext<ContractWithoutVersionOnlyWriteIndex>();
            ISearchRequest req = sc.ApplyContext(sd);

            Assert.False(req.Version);

            var data = ((IUrlParameter)req.Index).GetString(new ConnectionSettings());
            Assert.Equal(IndicesList.Write, data);
        }

        [Fact(DisplayName = "[SearchContext] search - only write index")]
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

        #region add tests
        [Fact(DisplayName = "[SearchContext] add - without version check")]
        public static void AddWithoutVersionCheck()
        {
            var entry = new EntryContext<ContractWithoutVersionCheck>(new ContractWithoutVersionCheck());

            var sd = new IndexDescriptor<ContractWithoutVersionCheck>(entry.Item);

            var sc = new SearchContext<ContractWithoutVersionCheck>();
            IIndexRequest<ContractWithoutVersionCheck> req = sc.ApplyContext(sd);

            Assert.Equal(IndicesList.Write, req.Index);
            Assert.False(req.RequestParameters.ContainsKey("version"));
        }

        [Fact(DisplayName = "[SearchContext] add - with version check")]
        public static void AddWithVersionCheck()
        {
            var entry = new EntryContext<ContractWithVersionCheck>(new ContractWithVersionCheck());

            var sd = new IndexDescriptor<ContractWithVersionCheck>(entry.Item);

            var sc = new SearchContext<ContractWithVersionCheck>();
            IIndexRequest<ContractWithVersionCheck> req = sc.ApplyContext(sd);

            Assert.Equal(IndicesList.Write, req.Index);
            Assert.False(req.RequestParameters.ContainsKey("version"));
        }

        [Fact(DisplayName = "[SearchContext] add - without version check & only write index")]
        public static void AddWithoutVersionCheckOnlyWriteIndex()
        {
            var entry = new EntryContext<ContractWithoutVersionOnlyWriteIndex>(new ContractWithoutVersionOnlyWriteIndex());

            var sd = new IndexDescriptor<ContractWithoutVersionOnlyWriteIndex>(entry.Item);

            var sc = new SearchContext<ContractWithoutVersionOnlyWriteIndex>();
            IIndexRequest<ContractWithoutVersionOnlyWriteIndex> req = sc.ApplyContext(sd);

            Assert.Equal(IndicesList.Write, req.Index);
            Assert.False(req.RequestParameters.ContainsKey("version"));
        }

        [Fact(DisplayName = "[SearchContext] add - only write index")]
        public static void AddOnlyWriteIndex()
        {
            var entry = new EntryContext<ContractOnlyWriteIndex>(new ContractOnlyWriteIndex());

            var sd = new IndexDescriptor<ContractOnlyWriteIndex>(entry.Item);

            var sc = new SearchContext<ContractOnlyWriteIndex>();
            IIndexRequest<ContractOnlyWriteIndex> req = sc.ApplyContext(sd);

            Assert.Equal(IndicesList.Write, req.Index);
            Assert.False(req.RequestParameters.ContainsKey("version"));
        }
        #endregion

        #region upgrade test
        [Fact(DisplayName = "[SearchContext] update - without version check")]
        public static void UpgradeWithoutVersionCheck()
        {
            var entry = new EntryContext<ContractWithoutVersionCheck>(new ContractWithoutVersionCheck());

            var sd = new IndexDescriptor<ContractWithoutVersionCheck>(entry.Item);

            var sc = new SearchContext<ContractWithoutVersionCheck>();
            IIndexRequest<ContractWithoutVersionCheck> req = sc.ApplyContext(sd,entry);

            Assert.Equal(IndicesList.Write, req.Index);
            Assert.False(req.RequestParameters.ContainsKey("version"));
        }

        [Fact(DisplayName = "[SearchContext] update - with version check")]
        public static void UpgradeWithVersionCheck()
        {
            var entry = new EntryContext<ContractWithVersionCheck>(new ContractWithVersionCheck());

            var sd = new IndexDescriptor<ContractWithVersionCheck>(entry.Item);

            var sc = new SearchContext<ContractWithVersionCheck>();

            var sdr = sc.ApplyContext(sd, entry);

            IIndexRequest<ContractWithVersionCheck> req = sdr;
            Assert.Equal(IndicesList.Write, req.Index);

            Assert.True(req.RequestParameters.ContainsKey("version"));

            var version = req.RequestParameters.GetQueryStringValue<long>("version");
            Assert.Equal(entry.Version, version);
        }

        [Fact(DisplayName = "[SearchContext] update - without version check & only write index")]
        public static void UpdateWithoutVersionCheckOnlyWriteIndex()
        {
            var entry = new EntryContext<ContractWithoutVersionOnlyWriteIndex>(new ContractWithoutVersionOnlyWriteIndex());

            var sd = new IndexDescriptor<ContractWithoutVersionOnlyWriteIndex>(entry.Item);

            var sc = new SearchContext<ContractWithoutVersionOnlyWriteIndex>();
            IIndexRequest<ContractWithoutVersionOnlyWriteIndex> req = sc.ApplyContext(sd,entry);

            Assert.Equal(IndicesList.Write, req.Index);
            Assert.False(req.RequestParameters.ContainsKey("version"));
        }

        [Fact(DisplayName = "[SearchContext] update - only write index")]
        public static void UpgradeOnlyWriteIndex()
        {
            var entry = new EntryContext<ContractOnlyWriteIndex>(new ContractOnlyWriteIndex());

            var sd = new IndexDescriptor<ContractOnlyWriteIndex>(entry.Item);

            var sc = new SearchContext<ContractOnlyWriteIndex>();
            IIndexRequest<ContractOnlyWriteIndex> req = sc.ApplyContext(sd,entry);

            Assert.Equal(IndicesList.Write, req.Index);
            Assert.True(req.RequestParameters.ContainsKey("version"));

            var version = req.RequestParameters.GetQueryStringValue<long>("version");
            Assert.Equal(entry.Version, version);
        }
        #endregion

        #region delete
        [Fact(DisplayName = "[SearchContext] delete - without version check")]
        public static void DeleteWithoutVersionCheck()
        {
            var entry = new EntryContext<ContractWithoutVersionCheck>(new ContractWithoutVersionCheck());

            var sd = new DeleteDescriptor<ContractWithoutVersionCheck>(entry.Item);

            var sc = new SearchContext<ContractWithoutVersionCheck>();
            IDeleteRequest req = sc.ApplyContext(sd, entry);

            Assert.Equal(IndicesList.Write, req.Index);
            Assert.False(req.RequestParameters.ContainsKey("version"));
        }

        [Fact(DisplayName = "[SearchContext] delete - with version check")]
        public static void DeleteWithVersionCheck()
        {
            var entry = new EntryContext<ContractWithVersionCheck>(new ContractWithVersionCheck());

            var sd = new DeleteDescriptor<ContractWithVersionCheck>(entry.Item);

            var sc = new SearchContext<ContractWithVersionCheck>();

            var sdr = sc.ApplyContext(sd, entry);

            IDeleteRequest req = sdr;
            Assert.Equal(IndicesList.Write, req.Index);

            Assert.True(req.RequestParameters.ContainsKey("version"));

            var version = req.RequestParameters.GetQueryStringValue<long>("version");
            Assert.Equal(entry.Version, version);
        }

        [Fact(DisplayName = "[SearchContext] delete - without version check & only write index")]
        public static void DeleteWithoutVersionCheckOnlyWriteIndex()
        {
            var entry = new EntryContext<ContractWithoutVersionOnlyWriteIndex>(new ContractWithoutVersionOnlyWriteIndex());

            var sd = new DeleteDescriptor<ContractWithoutVersionOnlyWriteIndex>(entry.Item);

            var sc = new SearchContext<ContractWithoutVersionOnlyWriteIndex>();
            IDeleteRequest req = sc.ApplyContext(sd, entry);

            Assert.Equal(IndicesList.Write, req.Index);
            Assert.False(req.RequestParameters.ContainsKey("version"));
        }

        [Fact(DisplayName = "[SearchContext] delete - only write index")]
        public static void DeleteOnlyWriteIndex()
        {
            var entry = new EntryContext<ContractOnlyWriteIndex>(new ContractOnlyWriteIndex());

            var sd = new DeleteDescriptor<ContractOnlyWriteIndex>(entry.Item);

            var sc = new SearchContext<ContractOnlyWriteIndex>();
            IDeleteRequest req = sc.ApplyContext(sd, entry);

            Assert.Equal(IndicesList.Write, req.Index);
            Assert.True(req.RequestParameters.ContainsKey("version"));

            var version = req.RequestParameters.GetQueryStringValue<long>("version");
            Assert.Equal(entry.Version, version);
        }
        #endregion
    }
}
