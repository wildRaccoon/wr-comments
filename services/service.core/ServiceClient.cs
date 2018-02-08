using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace service.core
{
    public class ServiceClient
    {
        private string _url { get; set; }

        public ServiceClient(string url)
        {
            _url = url;
        }

        public async Task<TRes> PostAsync<TReq, TRes>(string name,TReq req)
            where TRes: class
            where TReq: class
        {
            var client = new HttpClient();
            var content = JsonConvert.SerializeObject(req);
            var api = $"{_url}/{ name}";

            var data = new StringContent(content, Encoding.UTF8, "application/json");
            var resp = await client.PostAsync(api, data);

            if (resp.IsSuccessStatusCode)
            {
                var respCont = await resp.Content.ReadAsStringAsync();

                var res = JsonConvert.DeserializeObject<TRes>(respCont);

                return res;
            }
            else
            {
                throw new Exception($"{api}:{resp.StatusCode}");
            }
        }
    }
}
