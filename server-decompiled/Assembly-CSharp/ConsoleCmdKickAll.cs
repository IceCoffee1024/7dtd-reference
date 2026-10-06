using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000232 RID: 562
[Preserve]
public class ConsoleCmdKickAll : ConsoleCmdAbstract
{
	// Token: 0x1700019C RID: 412
	// (get) Token: 0x060010E1 RID: 4321 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060010E2 RID: 4322 RVA: 0x0006BB12 File Offset: 0x00069D12
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "kickall" };
	}

	// Token: 0x060010E3 RID: 4323 RVA: 0x0006BB22 File Offset: 0x00069D22
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Kicks all users with optional reason. \"kickall reason\"";
	}

	// Token: 0x060010E4 RID: 4324 RVA: 0x0006BB2C File Offset: 0x00069D2C
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
