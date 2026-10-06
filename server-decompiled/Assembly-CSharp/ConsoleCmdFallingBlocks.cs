using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000217 RID: 535
[Preserve]
public class ConsoleCmdFallingBlocks : ConsoleCmdAbstract
{
	// Token: 0x06001029 RID: 4137 RVA: 0x0006720B File Offset: 0x0006540B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "fallingblocks", "fb" };
	}

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x0600102A RID: 4138 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x0600102B RID: 4139 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x0600102C RID: 4140 RVA: 0x00067223 File Offset: 0x00065423
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "FallingBlocks WIP Settings";
	}

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x0600102D RID: 4141 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x0600102E RID: 4142 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x0600102F RID: 4143 RVA: 0x00061642 File Offset: 0x0005F842
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06001030 RID: 4144 RVA: 0x0006722C File Offset: 0x0006542C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params[0].ToLowerInvariant() == "enable")
		{
			EntityFallingBlocks.Enabled = !EntityFallingBlocks.Enabled;
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("EntityFallingBlocks Feature Enabled: {0}", EntityFallingBlocks.Enabled));
			return;
		}
	}

	// Token: 0x04000CAA RID: 3242
	public static bool ClientVerification = true;

	// Token: 0x04000CAB RID: 3243
	public static bool AllowSendToPeer = true;

	// Token: 0x04000CAC RID: 3244
	public static bool AllowReceiveFromPeer = true;

	// Token: 0x04000CAD RID: 3245
	public static bool DisablePeerAction = false;
}
