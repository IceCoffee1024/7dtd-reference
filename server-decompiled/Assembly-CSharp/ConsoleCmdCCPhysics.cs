using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020001EF RID: 495
[Preserve]
public class ConsoleCmdCCPhysics : ConsoleCmdAbstract
{
	// Token: 0x06000F4A RID: 3914 RVA: 0x0006357C File Offset: 0x0006177C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "ccphysics" };
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06000F4B RID: 3915 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x06000F4C RID: 3916 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06000F4D RID: 3917 RVA: 0x0006358C File Offset: 0x0006178C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Enables or disables changes to CCPhysics layer interactions. Reloading the game session may be necessary to fully apply if changed.";
	}

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06000F4E RID: 3918 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06000F4F RID: 3919 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000F50 RID: 3920 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06000F51 RID: 3921 RVA: 0x00063594 File Offset: 0x00061794
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

	// Token: 0x04000C83 RID: 3203
	public static bool ClientVerification = true;

	// Token: 0x04000C84 RID: 3204
	public static bool AllowSendToPeer = true;

	// Token: 0x04000C85 RID: 3205
	public static bool AllowReceiveFromPeer = true;

	// Token: 0x04000C86 RID: 3206
	public static bool DisablePeerAction = false;

	// Token: 0x04000C87 RID: 3207
	public static bool EnableCCPhysicsChanges = true;
}
