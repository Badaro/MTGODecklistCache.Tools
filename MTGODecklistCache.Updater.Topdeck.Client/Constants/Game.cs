using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Constants
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Game
    {
        [EnumMember(Value = "Magic: The Gathering")]
        MagicTheGathering
    }
}
