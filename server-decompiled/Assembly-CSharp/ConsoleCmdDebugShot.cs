using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200020C RID: 524
[Preserve]
public class ConsoleCmdDebugShot : ConsoleCmdAbstract
{
	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06000FE2 RID: 4066 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06000FE3 RID: 4067 RVA: 0x00061642 File Offset: 0x0005F842
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000FE5 RID: 4069 RVA: 0x000666E4 File Offset: 0x000648E4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debugshot", "dbs" };
	}

	// Token: 0x06000FE6 RID: 4070 RVA: 0x000666FC File Offset: 0x000648FC
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GUIWindowConsole.Close();
		bool flag = _params.Count > 0 && StringParsers.ParseBool(_params[0], 0, -1, true);
		ThreadManager.StartCoroutine(this.openWindowLater(flag));
	}

	// Token: 0x06000FE7 RID: 4071 RVA: 0x00066737 File Offset: 0x00064937
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator openWindowLater(bool _savePerks)
	{
		yield return null;
		GUIWindowScreenshotText.Open(LocalPlayerUI.primaryUI, _savePerks);
		yield break;
	}

	// Token: 0x06000FE8 RID: 4072 RVA: 0x00066746 File Offset: 0x00064946
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Creates a screenshot with some debug information";
	}

	// Token: 0x06000FE9 RID: 4073 RVA: 0x0006674D File Offset: 0x0006494D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n  debugshot [save perks]\nLets you make a screenshot that will have some generic info\non it and a custom text you can enter. Also stores a list\nof your current perk levels, buffs and cvars in a CSV file\nnext to it if the optional parameter 'save perks' is set to true";
	}
}
