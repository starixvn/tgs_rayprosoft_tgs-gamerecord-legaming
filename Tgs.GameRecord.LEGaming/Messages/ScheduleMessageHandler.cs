using System.Text;
using System.Text.Json;
using Tgs.GameRecord.LEGaming.Common;
using Tgs.GameRecord.LEGaming.Core;
using Tgs.GameRecord.LEGaming.Core.ApplicationServices;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.LEGaming.Messages;

public class ScheduleMessageHandler(
	ILogger<ScheduleMessageHandler> logger,
	IPullRecordDispatcher dispatcher) : IMessageHandler<ReadOnlyMemory<byte>>
{
	public ValueTask HandleAsync(string subject, ReadOnlyMemory<byte> data, IEnumerable<MessageHeaderValue>? headerValues, CancellationToken cancellationToken = default)
	{
		var json = Encoding.UTF8.GetString(data.Span);
		var message = JsonSerializer.Deserialize<LEGamingGameRecordMessage>(json)
			?? throw new NullReferenceException("The request data resolves to a null value");

		logger.LogInformation("{logInformation}",
			JsonSerializer.Serialize(new
			{
				LogOn = DateTime.Now.ToString("u"),
				Subject = subject,
				Data = new
				{
					message.ApiUrl,
					message.Deskey,
					message.Md5key,
					message.AgentId,
					StartTime = message.StartTime.ToString("u"),
					EndTime = message.EndTime.ToString("u"),
				},
				Message = "This receive message handler."
			}));

		return dispatcher.PushCommandAsync(
			command: new PullRecordCommand(
				ApiUrl: message.ApiUrl,
				Deskey: message.Deskey,
				Md5key: message.Md5key,
				AgentId: message.AgentId,
				StartTime: message.StartTime,
				EndTime: message.EndTime),
			cancellationToken: cancellationToken);
	}
}
