using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using wr.repository.context;
using wr.repository.extension;
using wr.repository.interfaces;

namespace wr.repository.proxy
{
    public partial class SearchProxy : ISearchProxy
    {
        #region properties
        private IElasticClient _client { get; set; }
        private ILogger<SearchProxy> _logger { get; set; }
        #endregion

        #region constructor
        public SearchProxy(IElasticClient client, ILogger<SearchProxy> logger)
        {
            _client = client;
            _logger = logger;

            ApplyMaping();
        }
        #endregion

        #region Mapping
        public void AutoMap<T>(string index)
            where T: class
        {
            try
            {
                _logger.LogInformation($"Apply mapping for type {typeof(T).FullName} on alias {index}");

                var resp = _client.Map<T>(p => p.AutoMap().Index(index));

                if (!resp.IsValid)
                {
                    _logger.LogError($"Error when map type {typeof(T).FullName} - {resp.ServerError}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Error when map type {typeof(T).FullName}");
            }
        }

        private void ApplyMaping()
        {
            var types = (from a in AppDomain.CurrentDomain.GetAssemblies()
                         from t in a.GetTypes()
                         let attr = t.GetCustomAttribute<RepositoryEntryAttribute>(true)
                         where attr != null
                         select new Tuple<Type, RepositoryEntryAttribute>(t, attr))
                    .ToList();

            if (types?.Count > 0)
            {
                var automap = GetType().GetMethod(nameof(AutoMap));

                types.ForEach(i =>
                {
                    var mapTypeAuto = automap.MakeGenericMethod(i.Item1);
                    mapTypeAuto.Invoke(this, new object[] { i.Item2.WriteAlias });
                });
            }
            else
            {
                _logger.LogError($"Unable to find any contracts type for work!");
            }
        }
        #endregion
    }
}