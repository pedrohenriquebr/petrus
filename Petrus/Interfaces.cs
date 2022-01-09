using PetrusPackage.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace PetrusPackage
{

    namespace Interfaces
    {


        namespace Props
        {
            public class POptions
            {
                public IEnumerable<ValueTuple<string?, string?>>? Params { get; set; }

                public Dictionary<string, string> Body { get; set; }

                public Headers Headers { get; set; }

                public bool ForceJson { get; set; } = false;
            }

            public class PostOptions
            {

            }


            public class PInstanceOptions
            {
                public Headers Headers { get; set; }
                public bool ForceJson { get; set; } = false;
                public string BaseURL { get; set; }
            }
        }


        namespace Models
        {
            public class Headers
            {
                public string? ContentType { get; set; }
                public string? Accept { get; set; }
            }

            public class PResult
            {
                public dynamic Data { get; set; }
                public HttpResponseMessage Response { get; set; }
            }

        }

    }
}
