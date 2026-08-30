using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000274 RID: 628
[Preserve]
public class ConsoleCmdRemoveQuest : ConsoleCmdAbstract
{
	// Token: 0x0600129D RID: 4765 RVA: 0x000741E0 File Offset: 0x000723E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "removequest" };
	}

	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x0600129E RID: 4766 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x0600129F RID: 4767 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x060012A0 RID: 4768 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060012A1 RID: 4769 RVA: 0x000741F0 File Offset: 0x000723F0
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (GameManager.IsDedicatedServer)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("cannot execute removequest on dedicated server, please execute as a client");
		}
		if (_params.Count < 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("remove requires quest id");
			return;
		}
		string text = _params[0];
		foreach (KeyValuePair<string, QuestClass> keyValuePair in QuestClass.s_Quests)
		{
			if (keyValuePair.Key.EqualsCaseInsensitive(text))
			{
				text = keyValuePair.Key;
				break;
			}
		}
		if (QuestClass.GetQuest(text) == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Quest '{0}' does not exist!", text));
			return;
		}
		XUiM_Player.GetPlayer().QuestJournal.ForceRemoveQuest(text);
	}

	// Token: 0x060012A2 RID: 4770 RVA: 0x000742BC File Offset: 0x000724BC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "usage: removequest questname";
	}
}
