using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200027B RID: 635
[Preserve]
public class ConsoleCmdRemoveQuest : ConsoleCmdAbstract
{
	// Token: 0x060012AC RID: 4780 RVA: 0x00074AF0 File Offset: 0x00072CF0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "removequest" };
	}

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x060012AD RID: 4781 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x060012AE RID: 4782 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x060012AF RID: 4783 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060012B0 RID: 4784 RVA: 0x00074B00 File Offset: 0x00072D00
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

	// Token: 0x060012B1 RID: 4785 RVA: 0x00074BCC File Offset: 0x00072DCC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "usage: removequest questname";
	}
}
