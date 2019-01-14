using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace subway.JsonClass
{
    class SubwayLine
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("path")]
        public List<string> Path { get; set; }
    }
}
