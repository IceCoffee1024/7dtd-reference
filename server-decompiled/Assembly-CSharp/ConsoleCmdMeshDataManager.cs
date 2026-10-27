using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200024D RID: 589
[Preserve]
public class ConsoleCmdMeshDataManager : ConsoleCmdAbstract
{
	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x0600117B RID: 4475 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x0600117C RID: 4476 RVA: 0x00061906 File Offset: 0x0005FB06
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x0600117D RID: 4477 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x0600117E RID: 4478 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x0600117F RID: 4479 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001180 RID: 4480 RVA: 0x0006F0DC File Offset: 0x0006D2DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "meshdatamanager", "mdm" };
	}

	// Token: 0x06001181 RID: 4481 RVA: 0x0006F0F4 File Offset: 0x0006D2F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggle the MeshDataManager";
	}

	// Token: 0x06001182 RID: 4482 RVA: 0x0006F0FB File Offset: 0x0006D2FB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "mdm";
	}

	// Token: 0x06001183 RID: 4483 RVA: 0x0006F102 File Offset: 0x0006D302
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		MeshDataManager.Enabled = !MeshDataManager.Enabled;
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("MeshDataManager " + (MeshDataManager.Enabled ? "enabled" : "disabled") + ".");
	}
}
