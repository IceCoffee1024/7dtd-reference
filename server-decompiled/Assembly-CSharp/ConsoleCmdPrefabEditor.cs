using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000269 RID: 617
[Preserve]
public class ConsoleCmdPrefabEditor : ConsoleCmdAbstract
{
	// Token: 0x0600124F RID: 4687 RVA: 0x00072BAA File Offset: 0x00070DAA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "prefabeditor", "prefabedit", "predit" };
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x06001250 RID: 4688 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001251 RID: 4689 RVA: 0x00072BCA File Offset: 0x00070DCA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Open the Prefab Editor";
	}

	// Token: 0x06001252 RID: 4690 RVA: 0x00072BD1 File Offset: 0x00070DD1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "prefabeditor";
	}

	// Token: 0x06001253 RID: 4691 RVA: 0x00072BD8 File Offset: 0x00070DD8
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (PrefabEditModeManager.Instance.IsActive())
		{
			Log.Out("You are already in the prefab editor.");
			return;
		}
		if (!GameManager.Instance.IsSafeToConnect())
		{
			Log.Warning("Please return to the main menu before using this command.");
			return;
		}
		GameUtils.StartSinglePrefabEditingWithMessage(null);
	}

	// Token: 0x04000D35 RID: 3381
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_connectingToPrefabEditor;
}
