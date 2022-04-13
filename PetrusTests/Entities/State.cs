using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetrusTests.Entities
{

    public class State
    {
        [JsonProperty("nome")]
        public string Name { get; set; }
    }

}
