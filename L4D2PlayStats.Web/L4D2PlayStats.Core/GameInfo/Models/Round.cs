namespace L4D2PlayStats.Core.GameInfo.Models;

public class Round
{
    public bool IsInReady { get; set; }
    public bool IsInPause { get; set; }
    public bool InSecondHalfOfRound { get; set; }
    public int MaxChapterProgressPoints { get; set; }
    public decimal TankPercent { get; set; }
    public decimal WitchPercent { get; set; }
    public bool TankDisabled => TankPercent <= 0;
    public bool WitchDisabled => WitchPercent <= 0;
}