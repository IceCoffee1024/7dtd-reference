using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000278 RID: 632
[Preserve]
public class ConsoleCmdRemoveQuest : ConsoleCmdAbstract
{
	// Token: 0x060012AF RID: 4783 RVA: 0x0007483C File Offset: 0x00072A3C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "removequest" };
	}

	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x060012B0 RID: 4784 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x060012B1 RID: 4785 RVA: 0x00060397 File Offset: 0x0005E597
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x060012B2 RID: 4786 RVA: 0x00060397 File Offset: 0x0005E597
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060012B3 RID: 4787 RVA: 0x0007484C File Offset: 0x00072A4C
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

	// Token: 0x060012B4 RID: 4788 RVA: 0x00074918 File Offset: 0x00072B18
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "usage: removequest questname";
	}
}
