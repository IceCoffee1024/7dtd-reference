using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200022C RID: 556
[Preserve]
public class ConsoleCmdKick : ConsoleCmdAbstract
{
	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x060010D8 RID: 4312 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060010D9 RID: 4313 RVA: 0x0006B2F1 File Offset: 0x000694F1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "kick" };
	}

	// Token: 0x060010DA RID: 4314 RVA: 0x0006B301 File Offset: 0x00069501
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Kicks user with optional reason. \"kick playername reason\"";
	}

	// Token: 0x060010DB RID: 4315 RVA: 0x0006B308 File Offset: 0x00069508
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
