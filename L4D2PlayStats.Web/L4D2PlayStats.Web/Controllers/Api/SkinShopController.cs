using System.Globalization;
using L4D2PlayStats.Core.Auth;
using L4D2PlayStats.Core.Infrastructure.Structures;
using L4D2PlayStats.Core.Skins.Commands;
using L4D2PlayStats.Core.Skins.Results;
using L4D2PlayStats.Core.Skins.Services;
using L4D2PlayStats.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/shop")]
[Authorize]
[ApiController]
public class SkinShopController(ISkinShopService skinShopService, ISkinService skinService, ICurrentUser currentUser) : ControllerBase
{
    [HttpGet("wallet")]
    public async Task<IActionResult> WalletAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(currentUser.CommunityId))
            return Unauthorized();

        var wallet = await skinShopService.GetWalletAsync(currentUser.CommunityId, cancellationToken);
        if (wallet == null)
            return StatusCode(StatusCodes.Status503ServiceUnavailable);

        return Ok(new { wallet.Balance, wallet.Stars, wallet.Spent });
    }

    [HttpPost("skins/{skinId}/buy")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BuyAsync(string skinId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(currentUser.CommunityId))
            return Unauthorized();

        var result = await skinShopService.BuyAsync(currentUser.CommunityId, currentUser.Name, skinId, cancellationToken);

        return Ok(ToResponse(result));
    }

    [HttpPost("skins/{skinId}/equip")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EquipAsync(string skinId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(currentUser.CommunityId))
            return Unauthorized();

        var result = await skinShopService.EquipAsync(currentUser.CommunityId, skinId, "site", cancellationToken);

        return Ok(ToResponse(result));
    }

    [HttpPost("weapons/{weapon}/unequip")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UnequipAsync(string weapon, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(currentUser.CommunityId))
            return Unauthorized();

        var result = await skinShopService.UnequipAsync(currentUser.CommunityId, weapon, "site", cancellationToken);

        return Ok(ToResponse(result));
    }

    [HttpPost("skins/{skinId}/price")]
    [AdminAuthorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PriceAsync(string skinId, [FromBody] SkinPriceCommand command, CancellationToken cancellationToken)
    {
        if (command.Price is < SkinService.MinPrice or > SkinService.MaxPrice)
            return BadRequest();

        if (await skinService.FindAsync(skinId, cancellationToken) == null)
            return NotFound();

        await skinService.SetPriceAsync(skinId, command.Price, currentUser.CommunityId!, cancellationToken);

        var skin = await skinService.FindAsync(skinId, cancellationToken);

        return Ok(new { skin!.Price, skin.DefaultPrice, custom = skin.CustomPrice != null });
    }

    [HttpPost("gift")]
    [AdminAuthorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GiftAsync([FromBody] SkinOwnershipCommand command, CancellationToken cancellationToken)
    {
        if (!TryParseCommunityId(command.CommunityId, out var communityId) || string.IsNullOrEmpty(command.SkinId))
            return BadRequest();

        var result = await skinShopService.GiftAsync(communityId, command.SkinId, currentUser.CommunityId!, currentUser.Name, cancellationToken);

        return Ok(ToResponse(result));
    }

    [HttpPost("refund")]
    [AdminAuthorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RefundAsync([FromBody] SkinOwnershipCommand command, CancellationToken cancellationToken)
    {
        if (!TryParseCommunityId(command.CommunityId, out var communityId) || string.IsNullOrEmpty(command.SkinId))
            return BadRequest();

        var result = await skinShopService.RefundAsync(communityId, command.SkinId, currentUser.CommunityId!, cancellationToken);

        return Ok(ToResponse(result));
    }

    [HttpPost("refund-all")]
    [AdminAuthorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RefundAllAsync()
    {
        var result = await skinShopService.RefundAllAsync(currentUser.CommunityId!);

        return Ok(result);
    }

    private static object ToResponse(SkinShopResult result)
    {
        return new
        {
            result.Status,
            result.Succeeded,
            price = result.Skin?.Price,
            balance = result.Wallet?.Balance
        };
    }

    private static bool TryParseCommunityId(string? value, out string communityId)
    {
        communityId = string.Empty;

        if (!SteamIdentifiers.TryParse(value?.Trim(), out var steamIdentifiers) || steamIdentifiers.CommunityId is not > 0)
            return false;

        communityId = steamIdentifiers.CommunityId.Value.ToString(CultureInfo.InvariantCulture);

        return true;
    }
}