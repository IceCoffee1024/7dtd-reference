using System;
using System.Collections.Generic;
using Platform;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020002BA RID: 698
[Preserve]
public class ConsoleCmdTwitchCommand : ConsoleCmdAbstract
{
	// Token: 0x06001405 RID: 5125 RVA: 0x0007A0EE File Offset: 0x000782EE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "twitch" };
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x06001406 RID: 5126 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x06001407 RID: 5127 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x06001408 RID: 5128 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001409 RID: 5129 RVA: 0x0007A100 File Offset: 0x00078300
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!TwitchManager.Current.IsReady)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Twitch must be active to use this command!");
		}
		if (_params.Count < 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("twitch commands available:");
			for (int i = 0; i < TwitchManager.Current.TwitchCommandList.Count; i++)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("   " + TwitchManager.Current.TwitchCommandList[i].CommandText[0]);
			}
			return;
		}
		if (_params[0] == "refresh")
		{
			GameManager.Instance.StartCoroutine(TwitchManager.Current.Authentication.RefreshToken(0f, true));
			return;
		}
		TwitchManager.Current.HandleConsoleAction(_params);
	}

	// Token: 0x0600140A RID: 5130 RVA: 0x0007A1C5 File Offset: 0x000783C5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "usage: twitch <command> <params>";
	}
}
