using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MTGODecklistCache.Updater.Topdeck.Client.Constants
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PlayerColumn
    {
        [EnumMember(Value = "name")]
        Name,
        [EnumMember(Value = "decklist")]
        Decklist,
        [EnumMember(Value = "deckSnapshot")]
        DeckSnapshot,
        [EnumMember(Value = "commanders")]
        Commanders,
        [EnumMember(Value = "wins")]
        Wins,
        [EnumMember(Value = "winsSwiss")]
        WinsSwiss,
        [EnumMember(Value = "winsBracket")]
        WinsBracket,
        [EnumMember(Value = "winRate")]
        WinRate,
        [EnumMember(Value = "winRateSwiss")]
        WinRateSwiss,
        [EnumMember(Value = "winRateBracket")]
        WinRateBracket,
        [EnumMember(Value = "draws")]
        Draws,
        [EnumMember(Value = "losses")]
        Losses,
        [EnumMember(Value = "lossesSwiss")]
        LossesSwiss,
        [EnumMember(Value = "lossesBracket")]
        LossesBracket,
        [EnumMember(Value = "id")]
        ID
    }
}
