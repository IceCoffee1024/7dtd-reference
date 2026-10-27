using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200027D RID: 637
[Preserve]
public class ConsoleCmdRemoveQuest : ConsoleCmdAbstract
{
	// Token: 0x060012B0 RID: 4784 RVA: 0x00074CC0 File Offset: 0x00072EC0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "removequest" };
	}

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x060012B1 RID: 4785 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x060012B2 RID: 4786 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x060012B3 RID: 4787 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060012B4 RID: 4788 RVA: 0x00074CD0 File Offset: 0x00072ED0
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

	// Token: 0x060012B5 RID: 4789 RVA: 0x00074D9C File Offset: 0x00072F9C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "usage: removequest questname";
	}
}
