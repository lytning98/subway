using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace subway.JsonClass
{
    class SubwayStation
    {
        public int x { get; set; }
        public int y { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
