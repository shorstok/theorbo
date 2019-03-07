using Newtonsoft.Json;

namespace theorbo.MusicTheory
{
    public class IanringScaleEntity
    {
        [JsonProperty("tones")] public int[] Tones { get; private set; }

        [JsonProperty("modes")] public int[] Modes { get; private set; }

        [JsonProperty("symmetries")] public int[] Symmetries { get; private set; }

        [JsonProperty("imperfections")] public int[] Imperfections { get; private set; }

        [JsonProperty("names")] public string[] Names { get; private set; }
    }
}