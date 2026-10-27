using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020001F4 RID: 500
[Preserve]
public class ConsoleCmdCCPhysics : ConsoleCmdAbstract
{
	// Token: 0x06000F4C RID: 3916 RVA: 0x00063D78 File Offset: 0x00061F78
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "ccphysics" };
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x06000F4D RID: 3917 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x06000F4E RID: 3918 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06000F4F RID: 3919 RVA: 0x00063D88 File Offset: 0x00061F88
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Enables or disables changes to CCPhysics layer interactions. Reloading the game session may be necessary to fully apply if changed.";
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x06000F50 RID: 3920 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x06000F51 RID: 3921 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x06000F52 RID: 3922 RVA: 0x00061906 File Offset: 0x0005FB06
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06000F53 RID: 3923 RVA: 0x00063D90 File Offset: 0x00061F90
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			ConsoleCmdCCPhysics.EnableCCPhysicsChanges = !ConsoleCmdCCPhysics.EnableCCPhysicsChanges;
		}
		else
		{
			string text = _params[0].ToLowerInvariant();
			if (!(text == "on"))
			{
				if (!(text == "off"))
				{
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Unrecognised subcommand. Use 'on' or 'off' subcommand, or none to toggle.");
					return;
				}
				ConsoleCmdCCPhysics.EnableCCPhysicsChanges = false;
			}
			else
			{
				ConsoleCmdCCPhysics.EnableCCPhysicsChanges = true;
			}
		}
		Physics.IgnoreLayerCollision(20, 15, ConsoleCmdCCPhysics.EnableCCPhysicsChanges);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Physics layer interaction changes " + (ConsoleCmdCCPhysics.EnableCCPhysicsChanges ? "enabled" : "disabled") + ". Reloading the game session may be necessary to fully apply.");
	}

	// Token: 0x04000C8B RID: 3211
	public static bool ClientVerification = true;

	// Token: 0x04000C8C RID: 3212
	public static bool AllowSendToPeer = true;

	// Token: 0x04000C8D RID: 3213
	public static bool AllowReceiveFromPeer = true;

	// Token: 0x04000C8E RID: 3214
	public static bool DisablePeerAction = false;

	// Token: 0x04000C8F RID: 3215
	public static bool EnableCCPhysicsChanges = true;
}
