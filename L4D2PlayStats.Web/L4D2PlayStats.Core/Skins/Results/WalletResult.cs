namespace L4D2PlayStats.Core.Skins.Results;

public class WalletResult
{
    public int MvpSiDamage { get; init; }
    public int MvpCommon { get; init; }
    public int Stars { get; init; }
    public int Spent { get; init; }
    public int Balance => Math.Max(0, Stars - Spent);
}