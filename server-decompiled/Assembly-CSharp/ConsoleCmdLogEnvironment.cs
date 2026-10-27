using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200023E RID: 574
[Preserve]
public class ConsoleCmdLogEnvironment : ConsoleCmdAbstract
{
	// Token: 0x0600111B RID: 4379 RVA: 0x0006CDB3 File Offset: 0x0006AFB3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "logenv" };
	}

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x0600111C RID: 4380 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600111D RID: 4381 RVA: 0x0006CDC3 File Offset: 0x0006AFC3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Log the process environment variables";
	}

	// Token: 0x0600111E RID: 4382 RVA: 0x000321FF File Offset: 0x000303FF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x0600111F RID: 4383 RVA: 0x0006CDCC File Offset: 0x0006AFCC
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		foreach (object obj in Environment.GetEnvironmentVariables())
		{
			DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
			string text = dictionaryEntry.Key as string;
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = dictionaryEntry.Value as string;
				Log.Out(string.Concat(new string[] { "Environment variable '", text, "' = '", text2, "'" }));
			}
		}
	}
}
