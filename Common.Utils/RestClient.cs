using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Utils
{
    public class RestClient<T> : IRestClient<T> where T : class
    {
        #region Attributes
        private HttpClient _httpClient;
        private HttpContent _httpContent;
        private IEnumerable<KeyValuePair<string, string>> _parameters;
        private string _uriRequest;

        public String UriRequest
        {
            get { return _uriRequest; }
            set { _uriRequest = value; }
        }
        #endregion
        #region Constructors
        /// <summary>
        /// Creates an instance of the RESTClient
        /// </summary>
        /// <param name="timeout"></param>
        public RestClient(string uriRequest, TimeSpan? timeout = null)
        {
            _httpClient = new HttpClient();
            _uriRequest = uriRequest;
            if (!timeout.HasValue)
            {
                _httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            }
            else
            {
                _httpClient.Timeout = timeout.Value;
            }
        }
        #endregion
        #region Publics
        public void CreateParameterListRequest(IEnumerable<KeyValuePair<string, string>> parametersList)
        {
            if (parametersList.Count() > 0)
            {
                _parameters = parametersList;
                _httpContent = new FormUrlEncodedContent(_parameters);
            }
            else
            {
                throw new Exception("Parameters list can not be empty");
            }
            

        }
        /// <summary>
        /// Add a header to the http content
        /// </summary>
        /// <param name="headerName">The header name</param>
        /// <param name="headerValue">The header value</param>
        public void AddHeader(string headerName, string headerValue)
        {
            if (_httpContent != null)
            {
                _httpContent.Headers.Add(headerName, headerValue);
            }
            else
            {
                throw new Exception("The HttpContent is null, can not add headers");
            }

        }
        /// <summary>
        /// Adds a hdefault request header to the client
        /// </summary>
        /// <param name="headerName">The header name</param>
        /// <param name="headerValue">The header value</param>
        public void AddDefaultRequestHeader(string headerName, string headerValue)
        {
            if (_httpClient != null)
            {
                _httpClient.DefaultRequestHeaders.Add(headerName, headerValue);
            }
        }
        /// <summary>
        /// Perform a GET request
        /// </summary>
        /// <returns></returns>
        public async Task<T> GETRequestAsync()
        {
            T output = (T)Activator.CreateInstance(typeof(T));
            HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead;
            if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                httpCompletionOption = HttpCompletionOption.ResponseHeadersRead;
            }
            var request = new HttpRequestMessage(HttpMethod.Get, _uriRequest);
            using (var response = await _httpClient.SendAsync(request, httpCompletionOption))
            {
                if (response.IsSuccessStatusCode)
                {
                    string contentResult = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(contentResult))
                    {
                        throw new Exception("The response content is empty");
                    }
                    
                    output = JsonConvert.DeserializeObject<T>(contentResult);
                }
            }
            return output;
        }
        /// <summary>
        /// Perform a POST request
        /// </summary>
        /// <returns>Return a task of a customed T object</returns>
        public Task<T> POSTRequestAsync()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Dispose the resources of the class
        /// </summary>
        public void Dispose()
        {
            _httpContent.Dispose();
            _httpClient.Dispose();
        }

        
        #endregion
    }
}
