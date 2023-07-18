namespace Tgs.GameRecord.LEGaming.Core.JobApplicationServices;

public record JobExecuteCommand(
    Uri ApiUrl,
    string Deskey,
    string Md5key,
    string AgentId);
