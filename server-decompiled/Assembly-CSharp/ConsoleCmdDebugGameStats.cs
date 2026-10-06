using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000208 RID: 520
[Preserve]
public class ConsoleCmdDebugGameStats : ConsoleCmdAbstract
{
	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000FCA RID: 4042 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000FCB RID: 4043 RVA: 0x00066309 File Offset: 0x00064509
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debuggamestats" };
	}

	// Token: 0x06000FCC RID: 4044 RVA: 0x00066319 File Offset: 0x00064519
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "GameStats commands";
	}

	// Token: 0x06000FCD RID: 4045 RVA: 0x00066320 File Offset: 0x00064520
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

	// Token: 0x06000FCE RID: 4046 RVA: 0x0006647C File Offset: 0x0006467C
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

	// Token: 0x04000CA3 RID: 3235
	[PublicizedFrom(EAccessModifier.Private)]
	public string logFile;

	// Token: 0x04000CA4 RID: 3236
	[PublicizedFrom(EAccessModifier.Private)]
	public StringBuilder stringBuilder;
}
