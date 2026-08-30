using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000214 RID: 532
[Preserve]
public class ConsoleCmdFallingBlocks : ConsoleCmdAbstract
{
	// Token: 0x0600102B RID: 4139 RVA: 0x00066CD3 File Offset: 0x00064ED3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "fallingblocks", "fb" };
	}

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x0600102C RID: 4140 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x0600102D RID: 4141 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x0600102E RID: 4142 RVA: 0x00066CEB File Offset: 0x00064EEB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "FallingBlocks WIP Settings";
	}

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x0600102F RID: 4143 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06001030 RID: 4144 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06001031 RID: 4145 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06001032 RID: 4146 RVA: 0x00066CF4 File Offset: 0x00064EF4
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params[0].ToLowerInvariant() == "enable")
		{
			EntityFallingBlocks.Enabled = !EntityFallingBlocks.Enabled;
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("EntityFallingBlocks Feature Enabled: {0}", EntityFallingBlocks.Enabled));
			return;
		}
	}

	// Token: 0x04000CA4 RID: 3236
	public static bool ClientVerification = true;

	// Token: 0x04000CA5 RID: 3237
	public static bool AllowSendToPeer = true;

	// Token: 0x04000CA6 RID: 3238
	public static bool AllowReceiveFromPeer = true;

	// Token: 0x04000CA7 RID: 3239
	public static bool DisablePeerAction = false;
}
