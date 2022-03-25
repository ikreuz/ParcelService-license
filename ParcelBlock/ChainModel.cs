using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelBlock
{
    public class ChainModel
    {
        [JsonProperty("chain")]
        public ChainModel chain { get; set; }
    }

    public class Chain
    {
        [JsonProperty("index")]
        public string Index { get; set; }
        [JsonProperty("timestamp")]
        public string TimeStamp { get; set; }
        [JsonProperty("previoushash")]
        public string PreviousHash { get; set; }
        [JsonProperty("hash")]
        public string Hash { get; set; }
        [JsonProperty("data")]
        public string Data { get; set; }
        [JsonProperty("nonce")]
        public string Nonce { get; set; }
    }
}
