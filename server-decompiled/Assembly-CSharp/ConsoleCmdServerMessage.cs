using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000288 RID: 648
[Preserve]
public class ConsoleCmdServerMessage : ConsoleCmdAbstract
{
	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x060012E1 RID: 4833 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060012E2 RID: 4834 RVA: 0x000755C4 File Offset: 0x000737C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "say" };
	}

	// Token: 0x060012E3 RID: 4835 RVA: 0x000755D4 File Offset: 0x000737D4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Sends a message to all connected clients";
	}

	// Token: 0x060012E4 RID: 4836 RVA: 0x000755DC File Offset: 0x000737DC
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count < 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Wrong number of arguments, expected 1, found " + _params.Count.ToString() + ".");
			return;
		}
		string text = _params[0];
		GameManager.Instance.ChatMessageServer(null, EChatType.Global, -1, text, null, EMessageSender.Server, GeneratedTextManager.BbCodeSupportMode.Supported);
	}
}
