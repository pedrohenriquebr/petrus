using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using PetrusPackage.Interfaces.Models;
using PetrusPackage.Interfaces.Props;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Xml;
using PetrusPackage.Extensions;

namespace PetrusPackage
{

    public class Petrus
    {
        private Petrus()
        {

        }

        /// <summary>
        /// Parse a JSON into object or objects array
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static JContainer ParseJSONData(string data)
        {
            if(data.StartsWith("[") && data.EndsWith("]"))
            {
                return JArray.Parse(data);
            }

            return JObject.Parse(data);
        }
        public static PInstance Create(PInstanceOptions options) => new(options);
        public static PInstance Create() => new(new PInstanceOptions() { });
        public static async Task<PResult> Get(string url)
        {
            return await Get(url, new POptions
            {
                Params = null,
                Headers = null
            });
        }
        public static async Task<PResult> Post(string url)
        {
            return await Post(url, new POptions { });
        }
        public static async Task<PResult> Post(string url, object body)
        {

            var dict = new Dictionary<string, string>();

            foreach (var prop in body.GetType().GetProperties())
            {
                dict.Add(prop.Name, prop.GetValue(body, null).ToString());
            }

            return await Post(url, new POptions
            {
                Body = dict
            });
        }
        public static async Task<PResult> Post(string url, Dictionary<string, string> body)
        {
            return await Post(url, new POptions
            {
                Body = body
            });
        }
        public static async Task<PResult> Post(string url, POptions options)
        {
            using (var client = new HttpClient())
            {
                var fullUrl = url;
                HttpContent httpContent = null;

                if (options.Headers == null)
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                else
                {
                    if (!string.IsNullOrEmpty(options.Headers.Accept))
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(options.Headers.Accept));
                    }
                }

                if (options.Params != null)
                {
                    var dict = new Dictionary<string, string>();
                    foreach (var param in options.Params)
                    {
                        dict.Add(param.Item1, param.Item2);
                    }

                    fullUrl = QueryHelpers.AddQueryString(fullUrl, dict);
                }

                if (options.Body != null)
                {
                    httpContent = new FormUrlEncodedContent(options.Body);
                }

                using (var response = await client.PostAsync(fullUrl, httpContent))
                {
                    using (var content = response.Content)
                    {
                        var data = await content.ReadAsStringAsync();
                        var result = new PResult();

                        result.Response = response;
                        if (content != null)
                        {

                            switch (content.Headers.ContentType.MediaType)
                            {
                                case MediaTypeNames.Text.Html:
                                    result.Data = data;
                                    return result;
                                case MediaTypeNames.Application.Json:
                                    result.Data = JObject.Parse(data);
                                    return result;
                                case MediaTypeNames.Application.Xml:
                                    var doc = new XmlDocument();
                                    doc.LoadXml(data);
                                    result.Data = doc;
                                    return result;
                            }

                            throw new Exception(string.Format("Unknow MimeType {0}", content.Headers.ContentType.MediaType));
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public static async Task<PResult> Get(string url, POptions options)
        {
            using (var client = new HttpClient())
            {
                var fullUrl = url;

                if (options.Headers == null)
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                else
                {
                    if (!string.IsNullOrEmpty(options.Headers.Accept))
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(options.Headers.Accept));
                    }
                }

                if (options.Params != null)
                {
                    var dict = new Dictionary<string, string>();
                    foreach (var param in options.Params)
                    {
                        dict.Add(param.Item1, param.Item2);
                    }

                    fullUrl = QueryHelpers.AddQueryString(fullUrl, dict);
                }

                using (var response = await client.GetAsync(fullUrl))
                {
                    using (var content = response.Content)
                    {
                        var data = await content.ReadAsStringAsync();
                        var result = new PResult();

                        result.Response = response;
                        if (content != null)
                        {

                            if (options.ForceJson)
                            {
                                result.Data = ParseJSONData(data);
                                result.Response = response;
                            }

                            switch (content.Headers.ContentType.MediaType)
                            {
                                case MediaTypeNames.Text.Html:
                                    result.Data = data;
                                    return result;
                                case MediaTypeNames.Application.Json:
                                    result.Data = ParseJSONData(data);
                                    return result;
                                case MediaTypeNames.Application.Xml:
                                    var doc = new XmlDocument();
                                    doc.LoadXml(data);
                                    result.Data = doc;
                                    return result;
                            }

                            throw new Exception(string.Format("Unknow MimeType {0}", content.Headers.ContentType.MediaType));
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
    }

    public class PInstance
    {

        private PInstanceOptions _options;

        private PInstance()
        {

        }

        public PInstance(PInstanceOptions options)
        {
            _options = options;
        }

        public Task<PResult> Get(string url)
        {
            return Petrus.Get(_options.BaseURL.AppendToURL(url), new POptions
            {
                ForceJson = _options.ForceJson,
                Headers = _options.Headers,
            });
        }

        public Task<PResult> Post(string url)
        {
            return Petrus.Post(_options.BaseURL.AppendToURL(url), new POptions
            {
                ForceJson = _options.ForceJson,
                Headers = _options.Headers,
            });
        }

        public Task<PResult> Get()
        {
            return Get("");
        }

        public Task<PResult> Post()
        {
            return Post("");
        }
    }
}
