using KnowledgeSharingApi.Infrastructures.Interfaces.Request;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KnowledgeSharingApi.Infrastructures.Request
{
    public class PostRequest(string url) : Request(url), IPostRequest
    {
        protected Dictionary<string, string> _formData = [];

        public IPostRequest PrepareFormData()
        {
            _contentType = FormData;
            return this;
        }

        public IPostRequest AddFormData<T>(string key, T value)
        {
            _formData.Add(key, value?.ToString() ?? string.Empty);
            return this;
        }

        protected override async Task<HttpResponseMessage> SendRequest(HttpClient httpClient, string urlWithParams, StringContent content)
        {
            if (_formData.Count > 0)
            {
                // Chuyển đổi FormData thành List<KeyValuePair<string, string>>
                List<KeyValuePair<string, string>> formDataList = [];
                foreach (var key in _formData.Keys)
                {
                    formDataList.Add(new KeyValuePair<string, string>(key, _formData[key]));
                }

                // Tạo HttpRequestMessage với FormData
                HttpRequestMessage request = new(HttpMethod.Post, urlWithParams)
                {
                    Content = new FormUrlEncodedContent(formDataList)
                };

                // Gửi request và nhận response
                HttpResponseMessage response = await httpClient.SendAsync(request);

                return response;
            }
            else
            {
                return await httpClient.PostAsync(urlWithParams, content);
            }
        }
    }
}
