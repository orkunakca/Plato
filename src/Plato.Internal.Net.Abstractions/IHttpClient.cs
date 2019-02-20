﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plato.Internal.Net.Abstractions
{

    public interface IHttpClient
    {

        int Timeout { get; set; }

        Task<string> GetAsync(Uri url);

        Task<string> GetAsync(Uri url, Dictionary<string, string> parameters);

        Task<string> PostAsync(Uri url);

        Task<string> PostAsync(Uri url, Dictionary<string, string> parameters);

        Task<string> RequestAsync(HttpMethod method, Uri url, Dictionary<string, string> parameters);
    }
    
    public enum HttpMethod
    {
        Get,
        Post,
        Put,
        Delete
    }
    
}