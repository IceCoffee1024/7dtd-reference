using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200027F RID: 639
[Preserve]
public class ConsoleCmdServerMessage : ConsoleCmdAbstract
{
	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x060012CE RID: 4814 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060012CF RID: 4815 RVA: 0x00074AE4 File Offset: 0x00072CE4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "say" };
	}

	// Token: 0x060012D0 RID: 4816 RVA: 0x00074AF4 File Offset: 0x00072CF4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Sends a message to all connected clients";
	}

	// Token: 0x060012D1 RID: 4817 RVA: 0x00074AFC File Offset: 0x00072CFC
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
