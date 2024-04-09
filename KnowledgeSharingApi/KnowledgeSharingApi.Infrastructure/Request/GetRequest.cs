using KnowledgeSharingApi.Infrastructures.Interfaces.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Request
{
    public class GetRequest(string url) : Request(url), IGetRequest
    {
        protected override async Task<HttpResponseMessage> SendRequest(HttpClient httpClient, string urlWithParams, StringContent content)
        {
            return await httpClient.GetAsync(urlWithParams);
        }
    }
}
