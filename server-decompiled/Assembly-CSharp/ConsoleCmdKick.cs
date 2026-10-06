using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000231 RID: 561
[Preserve]
public class ConsoleCmdKick : ConsoleCmdAbstract
{
	// Token: 0x1700019B RID: 411
	// (get) Token: 0x060010DC RID: 4316 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060010DD RID: 4317 RVA: 0x0006BA51 File Offset: 0x00069C51
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "kick" };
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x0006BA61 File Offset: 0x00069C61
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Kicks user with optional reason. \"kick playername reason\"";
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x0006BA68 File Offset: 0x00069C68
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count < 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Wrong number of arguments, expected at least 1, found " + _params.Count.ToString() + ".");
			return;
		}
		PlatformUserIdentifierAbs platformUserIdentifierAbs;
		ClientInfo clientInfo;
		if (ConsoleHelper.ParseParamPartialNameOrId(_params[0], out platformUserIdentifierAbs, out clientInfo, true) != 1 || clientInfo == null)
		{
			return;
		}
		string text = string.Empty;
		if (_params.Count > 1)
		{
			text = _params[1];
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Kicking Player " + clientInfo.playerName + ": " + text);
		ClientInfo clientInfo2 = clientInfo;
		GameUtils.EKickReason ekickReason = GameUtils.EKickReason.ManualKick;
		int num = 0;
		string text2 = text;
		GameUtils.KickPlayerForClientInfo(clientInfo2, new GameUtils.KickPlayerData(ekickReason, num, default(DateTime), text2));
	}
}
