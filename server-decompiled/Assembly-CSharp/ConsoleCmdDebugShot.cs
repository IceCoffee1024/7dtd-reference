using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000209 RID: 521
[Preserve]
public class ConsoleCmdDebugShot : ConsoleCmdAbstract
{
	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000FE5 RID: 4069 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000FE7 RID: 4071 RVA: 0x000661AC File Offset: 0x000643AC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debugshot", "dbs" };
	}

	// Token: 0x06000FE8 RID: 4072 RVA: 0x000661C4 File Offset: 0x000643C4
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GUIWindowConsole.Close();
		bool flag = _params.Count > 0 && StringParsers.ParseBool(_params[0], 0, -1, true);
		ThreadManager.StartCoroutine(this.openWindowLater(flag));
	}

	// Token: 0x06000FE9 RID: 4073 RVA: 0x000661FF File Offset: 0x000643FF
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator openWindowLater(bool _savePerks)
	{
		yield return null;
		GUIWindowScreenshotText.Open(LocalPlayerUI.primaryUI, _savePerks);
		yield break;
	}

	// Token: 0x06000FEA RID: 4074 RVA: 0x0006620E File Offset: 0x0006440E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Creates a screenshot with some debug information";
	}

	// Token: 0x06000FEB RID: 4075 RVA: 0x00066215 File Offset: 0x00064415
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n  debugshot [save perks]\nLets you make a screenshot that will have some generic info\non it and a custom text you can enter. Also stores a list\nof your current perk levels, buffs and cvars in a CSV file\nnext to it if the optional parameter 'save perks' is set to true";
	}
}
