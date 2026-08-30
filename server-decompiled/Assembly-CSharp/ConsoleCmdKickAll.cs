using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000230 RID: 560
[Preserve]
public class ConsoleCmdKickAll : ConsoleCmdAbstract
{
	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x060010E9 RID: 4329 RVA: 0x00060397 File Offset: 0x0005E597
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060010EA RID: 4330 RVA: 0x0006B94E File Offset: 0x00069B4E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "kickall" };
	}

	// Token: 0x060010EB RID: 4331 RVA: 0x0006B95E File Offset: 0x00069B5E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Kicks all users with optional reason. \"kickall reason\"";
	}

	// Token: 0x060010EC RID: 4332 RVA: 0x0006B968 File Offset: 0x00069B68
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		string text = string.Empty;
		if (_params.Count > 0)
		{
			text = _params[0];
		}
		ReadOnlyCollection<ClientInfo> list = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.List;
		for (int i = 0; i < list.Count; i++)
		{
			ClientInfo clientInfo = list[i];
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Kicking Player {0}: {1}", clientInfo.playerName, text));
			ClientInfo clientInfo2 = clientInfo;
			GameUtils.EKickReason ekickReason = GameUtils.EKickReason.ManualKick;
			int num = 0;
			string text2 = text;
			GameUtils.KickPlayerForClientInfo(clientInfo2, new GameUtils.KickPlayerData(ekickReason, num, default(DateTime), text2));
		}
	}
}
