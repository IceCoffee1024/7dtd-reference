using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000286 RID: 646
[Preserve]
public class ConsoleCmdServerMessage : ConsoleCmdAbstract
{
	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x060012DD RID: 4829 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060012DE RID: 4830 RVA: 0x000753F4 File Offset: 0x000735F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "say" };
	}

	// Token: 0x060012DF RID: 4831 RVA: 0x00075404 File Offset: 0x00073604
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Sends a message to all connected clients";
	}

	// Token: 0x060012E0 RID: 4832 RVA: 0x0007540C File Offset: 0x0007360C
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
