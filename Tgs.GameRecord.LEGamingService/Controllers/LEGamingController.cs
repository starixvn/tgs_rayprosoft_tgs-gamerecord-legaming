using Microsoft.AspNetCore.Mvc;
using Tgs.GameRecord.LEGamingService.Core.ApplicationServices;
using Tgs.GameRecord.LEGamingService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.LEGamingService.Controllers;

[ApiController]
[Route("LY")]
public class LEGamingController : ControllerBase
{
    [HttpPost]
    public async ValueTask<GameRecordResultViewModel> GameRecordAsync(
        [FromServices] IGetService<PullRecordQuery, PullRecordResult> service,
        GameRecordQueryViewModel model)
    {
        var result = await service.GetAsync(new PullRecordQuery()
        {
            BrandCode = model.BrandCode,
            CurrencyCode = model.CurrencyCode,
            StartTime = model.StartTime,
            EndTime = model.EndTime
        }).ConfigureAwait(false);

        return new GameRecordResultViewModel()
        {
            Success = result.Success,
            LastGameRecordTime = result.LastGameRecordTime
        };
    }
}
