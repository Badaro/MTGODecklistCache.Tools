using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Constants
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Format
    {
        [EnumMember(Value = "EDH")]
        EDH,
        [EnumMember(Value = "Pauper EDH")]
        PauperEDH,
        [EnumMember(Value = "Standard")]
        Standard,
        [EnumMember(Value = "Pioneer")]
        Pioneer,
        [EnumMember(Value = "Modern")]
        Modern,
        [EnumMember(Value = "Legacy")]
        Legacy,
        [EnumMember(Value = "Pauper")]
        Pauper,
        [EnumMember(Value = "Vintage")]
        Vintage,
        [EnumMember(Value = "Premodern")]
        Premodern,
        [EnumMember(Value = "Limited")]
        Limited,
        [EnumMember(Value = "Timeless")]
        Timeless,
        [EnumMember(Value = "Historic")]
        Historic,
        [EnumMember(Value = "Explorer")]
        Explorer,
        [EnumMember(Value = "Oathbreaker")]
        Oathbreaker,
    }
}
