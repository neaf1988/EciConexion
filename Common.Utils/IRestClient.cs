using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Common.Utils
{
    public interface IRestClient<T> : IDisposable
        where T : class
    {
        /// <summary>
        /// Create a list of parameters for a POST request
        /// </summary>
        /// <param name="parametersList">A collection key value par of parameters</param>
        void CreateParameterListRequest(IEnumerable<KeyValuePair<string, string>> parametersList);
        /// <summary>
        /// Add a header to the http content
        /// </summary>
        /// <param name="headerName">The header name</param>
        /// <param name="headerValue">The header value</param>
        void AddHeader(string headerName, string headerValue);

        /// <summary>
        /// Adds a hdefault request header to the client
        /// </summary>
        /// <param name="headerName">The header name</param>
        /// <param name="headerValue">The header value</param>
        void AddDefaultRequestHeader(string headerName, string headerValue);

        /// <summary>
        /// Perform a POST request
        /// </summary>
        /// <returns>Return a task of a customed T object</returns>
        Task<T> POSTRequestAsync();
        /// <summary>
        /// Perform a GET request
        /// </summary>
        /// <returns></returns>
        Task<T> GETRequestAsync();
    }
}