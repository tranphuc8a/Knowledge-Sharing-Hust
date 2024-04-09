using KnowledgeSharingApi.Infrastructures.Interfaces.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Request
{
    public class PatchRequest(string url) : Request(url), IPatchRequest
    {
        protected override async Task<HttpResponseMessage> SendRequest(HttpClient httpClient, string urlWithParams, StringContent content)
        {
            return await httpClient.PatchAsync(urlWithParams, content);
        }
    }
}
