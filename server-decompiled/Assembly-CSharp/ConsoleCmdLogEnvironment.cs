using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200023C RID: 572
[Preserve]
public class ConsoleCmdLogEnvironment : ConsoleCmdAbstract
{
	// Token: 0x06001117 RID: 4375 RVA: 0x0006CBE3 File Offset: 0x0006ADE3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "logenv" };
	}

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x06001118 RID: 4376 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001119 RID: 4377 RVA: 0x0006CBF3 File Offset: 0x0006ADF3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Log the process environment variables";
	}

	// Token: 0x0600111A RID: 4378 RVA: 0x0003223F File Offset: 0x0003043F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x0600111B RID: 4379 RVA: 0x0006CBFC File Offset: 0x0006ADFC
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
