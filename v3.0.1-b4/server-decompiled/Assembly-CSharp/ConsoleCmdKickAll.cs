using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200022D RID: 557
[Preserve]
public class ConsoleCmdKickAll : ConsoleCmdAbstract
{
	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x060010DD RID: 4317 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x0006B3B2 File Offset: 0x000695B2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "kickall" };
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x0006B3C2 File Offset: 0x000695C2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Kicks all users with optional reason. \"kickall reason\"";
	}

	// Token: 0x060010E0 RID: 4320 RVA: 0x0006B3CC File Offset: 0x000695CC
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
