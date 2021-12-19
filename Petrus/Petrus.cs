using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using Petrus.Interfaces.Models;
using Petrus.Interfaces.Props;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Petrus
{
    public class Petrus
    {

        public static async Task<PetrusResult> Get(string url)
        {
            return await Get(url, new PetrusOptions
            {
                Params = null,
                Headers = null
            });
        }
        public static async Task<PetrusResult> Post(string url)
        {
            return await Post(url, new PetrusOptions { });
        }
        public static async Task<PetrusResult> Post(string url, object body)
        {

            var dict = new Dictionary<string, string>();

            foreach(var prop in body.GetType().GetProperties())
            {
                dict.Add(prop.Name, prop.GetValue(body, null).ToString());
            }

            return await Post(url, new PetrusOptions
            {
                Body = dict
            });
        }
        public static async Task<PetrusResult> Post(string url, Dictionary<string, string> body)
        {
            return await Post(url, new PetrusOptions
            {
                Body = body
            });
        }
        public static async Task<PetrusResult> Post(string url, PetrusOptions options)
        {
            using (HttpClient client = new HttpClient())
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

                using (HttpResponseMessage response = await client.PostAsync(fullUrl, httpContent))
                {
                    using (HttpContent content = response.Content)
                    {
                        var data = await content.ReadAsStringAsync();

                        if (content != null)
                        {

                            switch (content.Headers.ContentType.MediaType)
                            {
                                case MediaTypeNames.Text.Html:
                                    return new PetrusResult
                                    {
                                        Data = data
                                    };

                                case MediaTypeNames.Application.Json:
                                    return new PetrusResult
                                    {
                                        Data = JObject.Parse(data)
                                    };
                                case MediaTypeNames.Application.Xml:
                                    var doc = new XmlDocument();
                                    doc.LoadXml(data);
                                    return new PetrusResult
                                    {
                                        Data = doc
                                    };

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
        public static async Task<PetrusResult> Get(string url, PetrusOptions options)
        {
            using (HttpClient client = new HttpClient())
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

                using (HttpResponseMessage response = await client.GetAsync(fullUrl))
                {
                    using (HttpContent content = response.Content)
                    {
                        var data = await content.ReadAsStringAsync();

                        if (content != null)
                        {

                            switch (content.Headers.ContentType.MediaType)
                            {
                                case MediaTypeNames.Text.Html:
                                    return new PetrusResult
                                    {
                                        Data = data
                                    };

                                case MediaTypeNames.Application.Json:
                                    return new PetrusResult
                                    {
                                        Data = JObject.Parse(data)
                                    };
                                case MediaTypeNames.Application.Xml:
                                    var doc = new XmlDocument();
                                    doc.LoadXml(data);
                                    return new PetrusResult
                                    {
                                        Data = doc
                                    };

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
}
