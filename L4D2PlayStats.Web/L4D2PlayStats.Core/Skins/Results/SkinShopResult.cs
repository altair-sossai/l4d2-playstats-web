using L4D2PlayStats.Core.Skins.Enums;

namespace L4D2PlayStats.Core.Skins.Results;

public class SkinShopResult
{
    public SkinShopStatus Status { get; init; }
    public Skin? Skin { get; init; }
    public WalletResult? Wallet { get; init; }
    public bool Succeeded => Status == SkinShopStatus.Success;

    public static SkinShopResult Of(SkinShopStatus status, Skin? skin = null, WalletResult? wallet = null)
    {
        return new SkinShopResult { Status = status, Skin = skin, Wallet = wallet };
    }
}