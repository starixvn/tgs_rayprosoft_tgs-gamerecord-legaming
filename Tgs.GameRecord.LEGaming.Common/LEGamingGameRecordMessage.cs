using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tgs.GameRecord.LEGaming.Common;

public record LEGamingGameRecordMessage
{
	[Required]
	[DefaultValue(typeof(Uri), "https://wc1-record.kewmn686.com/")]
	public Uri ApiUrl { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(string), "706EB65DD6E6DF4C")]
	public string Deskey { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(string), "3E00054B89B31B7A")]
	public string Md5key { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(string), "200995")]
	public string AgentId { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(DateTimeOffset), "2023-07-19T15:50:00+08:00")]
	public DateTimeOffset StartTime { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(DateTimeOffset), "2023-07-19T16:00:00+08:00")]
	public DateTimeOffset EndTime { get; init; } = default!;
}
