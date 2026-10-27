using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000219 RID: 537
[Preserve]
public class ConsoleCmdFallingBlocks : ConsoleCmdAbstract
{
	// Token: 0x0600102D RID: 4141 RVA: 0x000673DB File Offset: 0x000655DB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "fallingblocks", "fb" };
	}

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x0600102E RID: 4142 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x0600102F RID: 4143 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001030 RID: 4144 RVA: 0x000673F3 File Offset: 0x000655F3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "FallingBlocks WIP Settings";
	}

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x06001031 RID: 4145 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x06001032 RID: 4146 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x06001033 RID: 4147 RVA: 0x00061906 File Offset: 0x0005FB06
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06001034 RID: 4148 RVA: 0x000673FC File Offset: 0x000655FC
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params[0].ToLowerInvariant() == "enable")
		{
			EntityFallingBlocks.Enabled = !EntityFallingBlocks.Enabled;
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("EntityFallingBlocks Feature Enabled: {0}", EntityFallingBlocks.Enabled));
			return;
		}
	}

	// Token: 0x04000CAB RID: 3243
	public static bool ClientVerification = true;

	// Token: 0x04000CAC RID: 3244
	public static bool AllowSendToPeer = true;

	// Token: 0x04000CAD RID: 3245
	public static bool AllowReceiveFromPeer = true;

	// Token: 0x04000CAE RID: 3246
	public static bool DisablePeerAction = false;
}
