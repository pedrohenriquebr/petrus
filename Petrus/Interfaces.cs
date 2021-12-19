using Petrus.Interfaces.Models;
using System;
using System.Collections.Generic;

namespace Petrus
{

    namespace Interfaces
    {


        namespace Props
        {
            public class PetrusOptions
            {
                public IEnumerable<ValueTuple<string?, string?>>? Params { get; set; }

                public Dictionary<string, string> Body { get; set; }

                public Headers Headers { get; set; }

                public bool ForceJson { get; set; } = false;
            }

            public class PostOptions
            {

            }
        }


        namespace Models
        {
            public class Headers
            {
                public string? ContentType { get; set; }
                public string? Accept { get; set; }
            }

            public class PetrusResult
            {
                public dynamic Data { get; set; }
            }

        }



    }
}
