using Microsoft.AspNetCore.Mvc;
using Tgs.GameRecord.LEGaming.Core.ApplicationServices;
using Tgs.GameRecord.LEGaming.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.LEGaming.Controllers;

[ApiController]
[Route("LY")]
public class LEGamingController : ControllerBase
{
    /// <summary>
    /// Games the record asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///         {
    ///             "apiRecordUrl": "https://wc1-record.kewmn686.com/",
    ///             "deskey": "706EB65DD6E6DF4C",
    ///             "md5key": "3E00054B89B31B7A",
    ///             "agentId": "200995",
    ///             "startTime": "2023-07-19T15:50:00+08:00",
    ///             "endTime": "2023-07-19T16:00:00+08:00"
    ///         }
    /// </remarks>
    [HttpPost]
    public ValueTask GameRecordAsync(
        [FromServices] ICommandService<PullRecordCommand> service,
        PullRecordViewModel source)
    {
        var command = new PullRecordCommand(
            ApiUrl: source.ApiRecordUrl,
            Deskey: source.Deskey,
            Md5key: source.Md5key,
            AgentId: source.AgentId,
            StartTime: source.StartTime,
            EndTime: source.EndTime);
        return service
            .ExecuteAsync(
            command,
            HttpContext.RequestAborted);
    }
}
