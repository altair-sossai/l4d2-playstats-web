using L4D2PlayStats.Core.Campaign.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/campaigns")]
[ApiController]
public class CampaignController(ICampaignName campaignName) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(campaignName.Campaigns);
    }
}