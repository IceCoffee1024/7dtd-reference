using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200022F RID: 559
[Preserve]
public class ConsoleCmdKick : ConsoleCmdAbstract
{
	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x060010E4 RID: 4324 RVA: 0x00060397 File Offset: 0x0005E597
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060010E5 RID: 4325 RVA: 0x0006B88D File Offset: 0x00069A8D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "kick" };
	}

	// Token: 0x060010E6 RID: 4326 RVA: 0x0006B89D File Offset: 0x00069A9D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Kicks user with optional reason. \"kick playername reason\"";
	}

	// Token: 0x060010E7 RID: 4327 RVA: 0x0006B8A4 File Offset: 0x00069AA4
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
