using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace subway.JsonClass
{
    class Subway
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("stations")]
        public List<SubwayStation> Stations { get; set; }

        [JsonProperty("lines")]
        public List<SubwayLine> Lines { get; set; }
    }
}
