using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000244 RID: 580
[Preserve]
public class ConsoleCmdMeshDataManager : ConsoleCmdAbstract
{
	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06001168 RID: 4456 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x06001169 RID: 4457 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x0600116A RID: 4458 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x0600116B RID: 4459 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x0600116C RID: 4460 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x0600116D RID: 4461 RVA: 0x0006E604 File Offset: 0x0006C804
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "meshdatamanager", "mdm" };
	}

	// Token: 0x0600116E RID: 4462 RVA: 0x0006E61C File Offset: 0x0006C81C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggle the MeshDataManager";
	}

	// Token: 0x0600116F RID: 4463 RVA: 0x0006E623 File Offset: 0x0006C823
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "mdm";
	}

	// Token: 0x06001170 RID: 4464 RVA: 0x0006E62A File Offset: 0x0006C82A
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		MeshDataManager.Enabled = !MeshDataManager.Enabled;
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("MeshDataManager " + (MeshDataManager.Enabled ? "enabled" : "disabled") + ".");
	}
}
