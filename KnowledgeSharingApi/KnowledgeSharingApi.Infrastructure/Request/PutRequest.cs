using KnowledgeSharingApi.Infrastructures.Interfaces.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Request
{
    public class PutRequest(string url) : Request(url), IPutRequest
    {
        protected override async Task<HttpResponseMessage> SendRequest(HttpClient httpClient, string urlWithParams, StringContent content)
        {
            return await httpClient.PutAsync(urlWithParams, content);
        }
    }
}
