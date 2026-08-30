using System.Text.Json.Serialization;
using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Extensions;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "type",
    IgnoreUnrecognizedTypeDiscriminators = true,
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType)
]
[JsonDerivedType(typeof(SkeetEvent), "skeet")]
[JsonDerivedType(typeof(SkeetHurtEvent), "skeetHurt")]
[JsonDerivedType(typeof(HunterDeadstopEvent), "hunterDeadstop")]
[JsonDerivedType(typeof(BoomerPopEvent), "boomerPop")]
[JsonDerivedType(typeof(ChargerLevelEvent), "chargerLevel")]
[JsonDerivedType(typeof(ChargerLevelHurtEvent), "chargerLevelHurt")]
[JsonDerivedType(typeof(WitchCrownEvent), "witchCrown")]
[JsonDerivedType(typeof(WitchCrownHurtEvent), "witchCrownHurt")]
[JsonDerivedType(typeof(TongueCutEvent), "tongueCut")]
[JsonDerivedType(typeof(SmokerSelfClearEvent), "smokerSelfClear")]
[JsonDerivedType(typeof(TankRockSkeetedEvent), "tankRockSkeeted")]
[JsonDerivedType(typeof(TankRockEatenEvent), "tankRockEaten")]
[JsonDerivedType(typeof(HunterHighPounceEvent), "hunterHighPounce")]
[JsonDerivedType(typeof(JockeyHighPounceEvent), "jockeyHighPounce")]
[JsonDerivedType(typeof(DeathChargeEvent), "deathCharge")]
[JsonDerivedType(typeof(SpecialClearEvent), "specialClear")]
[JsonDerivedType(typeof(BoomerVomitLandedEvent), "boomerVomitLanded")]
[JsonDerivedType(typeof(SpecialShovedEvent), "specialShoved")]
[JsonDerivedType(typeof(BunnyHopStreakEvent), "bunnyHopStreak")]
[JsonDerivedType(typeof(CarAlarmTriggeredEvent), "carAlarmTriggered")]
[FeedPartial("GameEvents/_GameEvent")]
public class GameEvent : FeedItem
{
    [JsonIgnore]
    public virtual GameEventType EventType => GameEventType.Unknown;

    public virtual int Stars => EventType.Stars();

    [JsonIgnore]
    public string StarsText => new('★', Stars);

    public Player? Actor { get; set; }
}