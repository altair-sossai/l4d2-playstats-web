using L4D2PlayStats.Matches;
using L4D2PlayStats.OpenAi;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/open-ai")]
[ApiController]
public class OpenAiController(IOpenAiService openAiService, IMatchesServiceCached matchesServiceCached) : ControllerBase
{
    [HttpGet("last-match")]
    public async Task<IActionResult> LastMatch()
    {
        var matches = await matchesServiceCached.GetAsync();
        var lastMatch = matches.FirstOrDefault();
        if (lastMatch == null)
            return NotFound();

        var result = await openAiService.LastMatchSummaryAsync(lastMatch);

        return Ok(result);
    }
}