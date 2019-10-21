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

                [JsonProperty("text")] public string Text;
            }

            [JsonProperty("type")] public string Type = "section";

            [JsonProperty("text")] public SectionBlockText Text;
            
            
            
            
        }

        public class ContextBlock : SlackMessageBlock
        {
            public class ElementsBlock
            {
                [JsonProperty("type")] public string Type;

                [JsonProperty("text")] public string Text;
            }
            
            [JsonProperty("type")] public string Type = "context";

            [JsonProperty("elements")] public ElementsBlock[] Elements;
        }

        [JsonProperty("blocks")] public SlackMessageBlock[] Blocks;
    }
}