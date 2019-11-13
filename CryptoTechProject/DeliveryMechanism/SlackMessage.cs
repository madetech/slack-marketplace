using System.Collections.Generic;
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


        public class SectionBlockText
        {
            [JsonProperty("type")] public string Type;

            [JsonProperty("text")] public string Text;
        }

        public class TitleSectionBlock : SlackMessageBlock
        {
            [JsonProperty("type")] public string Type = "section";

            [JsonProperty("text")] public SectionBlockText Text;
        }

        public class SectionBlock : SlackMessageBlock
        {
            public class AccessoryBlock
            {
                [JsonProperty("type")] public string Type = "button";

                [JsonProperty("text")] public SectionBlockText Text;

                [JsonProperty("value")] public string Value;
            }

            [JsonProperty("type")] public string Type = "section";

            [JsonProperty("text")] public SectionBlockText Text;

            [JsonProperty("accessory")] public AccessoryBlock Accessory;
        }
        
        public class ShowcaseSectionBlock : SlackMessageBlock
        {
            [JsonProperty("type")] public string Type = "section";

            [JsonProperty("text")] public SectionBlockText Text;
        }

        [JsonProperty("blocks")] public List<SlackMessageBlock> Blocks;
    }
}