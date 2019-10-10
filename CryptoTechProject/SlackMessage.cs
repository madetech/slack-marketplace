using Newtonsoft.Json;

namespace CryptoTechProject
{
    public class SlackMessage
    {
        abstract public class SlackMessageBlock
        {
        }

        public class DividerBlock : SlackMessageBlock
        {
            [JsonProperty("type")] public string Type = "divider";
        }

        public class SectionBlock : SlackMessageBlock
        {
            public class SectionBlockText
            {
                [JsonProperty("type")] public string Type;

                [JsonProperty("emoji")] public bool Emoji;

                [JsonProperty("text")] public string Text;
            }

            [JsonProperty("type")] public string Type = "section";

            [JsonProperty("text")] public SectionBlockText Text;
        }

        [JsonProperty("blocks")] public SlackMessageBlock[] Blocks;
    }
}