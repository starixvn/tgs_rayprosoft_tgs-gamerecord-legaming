using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Tgs.GameRecord.LEGaming.Common;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.LEGaming.Controllers;

[ApiController]
[Route("LY")]
public class LEGamingController : ControllerBase
{
	/// <summary>
	/// Games the record manual asynchronous.
	/// </summary>
	/// <param name="messageSender">The message sender.</param>
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
	[HttpPost("manual")]
	public ValueTask GameRecordManualAsync(
		[FromServices] IMessageSender messageSender,
		LEGamingGameRecordMessage source)
		=> messageSender.PublishAsync(
			subject: LEGamingNatsConst.ManualPublish,
			data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source)),
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the record schedule asynchronous.
	/// </summary>
	/// <param name="messageSender">The message sender.</param>
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
	[HttpPost("schedule")]
	public ValueTask GameRecordScheduleAsync(
		[FromServices] IMessageSender messageSender,
		LEGamingGameRecordMessage source)
		=> messageSender.PublishAsync(
			subject: LEGamingNatsConst.SchedulePublish,
			data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source)),
			cancellationToken: HttpContext.RequestAborted);
}
