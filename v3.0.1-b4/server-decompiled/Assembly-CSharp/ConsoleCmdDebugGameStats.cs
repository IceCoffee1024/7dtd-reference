using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000205 RID: 517
[Preserve]
public class ConsoleCmdDebugGameStats : ConsoleCmdAbstract
{
	// Token: 0x17000160 RID: 352
	// (get) Token: 0x06000FCC RID: 4044 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000FCD RID: 4045 RVA: 0x00065DD1 File Offset: 0x00063FD1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debuggamestats" };
	}

	// Token: 0x06000FCE RID: 4046 RVA: 0x00065DE1 File Offset: 0x00063FE1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "GameStats commands";
	}

	// Token: 0x06000FCF RID: 4047 RVA: 0x00065DE8 File Offset: 0x00063FE8
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count >= 1)
		{
			if (_params[0].ToUpper() == "LOG")
			{
				bool flag;
				if (_params.Count >= 2 && bool.TryParse(_params[1], out flag))
				{
					if (flag)
					{
						this.logFile = Path.Join(PlatformApplicationManager.Application.temporaryCachePath, "GameStats.tsv");
						this.stringBuilder = new StringBuilder();
						this.stringBuilder.AppendLine(DebugGameStats.GetHeader('\t'));
						File.AppendAllText(this.logFile, this.stringBuilder.ToString());
						DebugGameStats.StartStatisticsUpdate(new DebugGameStats.StatisticsUpdatedCallback(this.logDebugGameStats));
						SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Started logging debug stats to " + this.logFile + ".");
						return;
					}
					DebugGameStats.StopStatisticsUpdate();
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Stopped logging debug stats to " + this.logFile + ".");
					return;
				}
			}
			else if (_params[0].ToUpper() == "PRINT")
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(DebugGameStats.GetHeader(','));
				stringBuilder.AppendLine(DebugGameStats.GetCurrentStatsString(','));
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(stringBuilder.ToString());
				return;
			}
		}
		Log.Out("Incorrect params, expected 'log [true|false]' or 'print'");
	}

	// Token: 0x06000FD0 RID: 4048 RVA: 0x00065F44 File Offset: 0x00064144
	[PublicizedFrom(EAccessModifier.Private)]
	public void logDebugGameStats(Dictionary<string, string> statisticsDictionary)
	{
		this.stringBuilder.Clear();
		foreach (KeyValuePair<string, string> keyValuePair in statisticsDictionary)
		{
			this.stringBuilder.Append(keyValuePair.Value);
			this.stringBuilder.Append('\t');
		}
		this.stringBuilder.AppendLine();
		File.AppendAllText(this.logFile, this.stringBuilder.ToString());
	}

	// Token: 0x04000C9D RID: 3229
	[PublicizedFrom(EAccessModifier.Private)]
	public string logFile;

	// Token: 0x04000C9E RID: 3230
	[PublicizedFrom(EAccessModifier.Private)]
	public StringBuilder stringBuilder;
}
