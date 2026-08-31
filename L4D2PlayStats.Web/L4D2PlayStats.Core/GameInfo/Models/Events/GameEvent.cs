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
[JsonDerivedType(typeof(BoomerVomitLandedEvent), "boomerVomitLanded")]
[JsonDerivedType(typeof(CarAlarmTriggeredEvent), "carAlarmTriggered")]
[JsonDerivedType(typeof(ChargerLevelEvent), "chargerLevel")]
[JsonDerivedType(typeof(ChargerLevelHurtEvent), "chargerLevelHurt")]
[JsonDerivedType(typeof(DeathChargeEvent), "deathCharge")]
[JsonDerivedType(typeof(HunterHighPounceEvent), "hunterHighPounce")]
[JsonDerivedType(typeof(PauseEvent), "pause")]
[JsonDerivedType(typeof(PlayerDeathEvent), "playerDeath")]
[JsonDerivedType(typeof(RoundEndedEvent), "roundEnded")]
[JsonDerivedType(typeof(RoundLiveEvent), "roundLive")]
[JsonDerivedType(typeof(SkeetEvent), "skeet")]
[JsonDerivedType(typeof(SkeetHurtEvent), "skeetHurt")]
[JsonDerivedType(typeof(SmokerSelfClearEvent), "smokerSelfClear")]
[JsonDerivedType(typeof(SpecialClearEvent), "specialClear")]
[JsonDerivedType(typeof(TankBecameBotEvent), "tankBecameBot")]
[JsonDerivedType(typeof(TankDiedEvent), "tankDied")]
[JsonDerivedType(typeof(TankRockEatenEvent), "tankRockEaten")]
[JsonDerivedType(typeof(TankRockSkeetedEvent), "tankRockSkeeted")]
[JsonDerivedType(typeof(TankSpawnedEvent), "tankSpawned")]
[JsonDerivedType(typeof(TongueCutEvent), "tongueCut")]
[JsonDerivedType(typeof(WitchCrownEvent), "witchCrown")]
[JsonDerivedType(typeof(WitchCrownHurtEvent), "witchCrownHurt")]
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