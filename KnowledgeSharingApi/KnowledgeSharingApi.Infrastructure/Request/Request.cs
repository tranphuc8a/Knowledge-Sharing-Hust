using KnowledgeSharingApi.Infrastructures.Interfaces.Request;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Request
{
    public abstract class Request(string url) : IRequest
    {
        protected string _url = url;
        protected string _contentType = "application/json";
        protected string? _authenticationToken = null;
        protected object? _body = null;
        protected object? _param = null;

        public const string ApplicationJson = "application/json";
        public const string TextPlain = "text/plain";
        public const string FormData = "multipart/form-data";


        public virtual async Task<T?> Execute<T>()
        {
            try
            {
                using HttpClient httpClient = new();

                // Set Bearer token authentication header if token is provided
                if (!string.IsNullOrEmpty(_authenticationToken))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authenticationToken);
                }

                // Set body
                string requestBody = _body != null ? JsonConvert.SerializeObject(_body) : "";
                StringContent content = new(requestBody, System.Text.Encoding.UTF8, _contentType);

                // Set params
                string urlWithParams = _param != null ? AppendParamsToUrl(_url) : _url;

                // Send request and get response
                HttpResponseMessage response = await SendRequest(httpClient, urlWithParams, content);

                // Check if response is successful
                if (response.IsSuccessStatusCode)
                {
                    // Read response and deserialize JSON
                    string responseContent = await response.Content.ReadAsStringAsync();
                    T? responseObject = JsonConvert.DeserializeObject<T>(responseContent);
                    return responseObject;
                }
                else
                {
                    // Handle unsuccessful response here, return default (null)
                    return default;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return default;
            }
        }

        protected abstract Task<HttpResponseMessage> SendRequest(HttpClient httpClient, string urlWithParams, StringContent content);

        public virtual IRequest SetBearerAuthentication(string token)
        {
            _authenticationToken = token;
            return this;
        }

        public virtual IRequest SetContentType(string contentType)
        {
            if (!string.IsNullOrEmpty(contentType))
            {
                // Kiểm tra tính hợp lệ của contentType
                if (contentType.StartsWith(ApplicationJson) ||
                    contentType.StartsWith(TextPlain) ||
                    contentType.StartsWith(FormData))
                {
                    _contentType = contentType;
                }
                else
                {
                    throw new ArgumentException("Invalid content type.");
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(contentType), "Content type cannot be null or empty.");
            }
            return this;
        }

        public virtual IRequest SetUrl(string url)
        {
            _url = url;
            return this;
        }

        public virtual IRequest SetBody(object body)
        {
            _body = body;
            return this;
        }

        public virtual IRequest SetParams(object param)
        {
            _param = param;
            return this;
        }

        protected virtual string AppendParamsToUrl(string url)
        {
            string paramUrl = _param != null ? $"?{_param}" : "";
            return $"{url}{paramUrl}";
        }
    }
}
