using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002B1 RID: 689
[Preserve]
public class ConsoleCmdTeleportPlayer : ConsoleCmdTeleportsAbs
{
	// Token: 0x1700021B RID: 539
	// (get) Token: 0x060013C9 RID: 5065 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x00078E09 File Offset: 0x00077009
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Teleport a given player";
	}

	// Token: 0x060013CB RID: 5067 RVA: 0x00078E10 File Offset: 0x00077010
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n  1. teleportplayer <user id / player name / entity id> <x> <y> <z> [view direction]\n  2. teleportplayer <user id / player name / entity id> <target user id / player name / entity id>\n1. Teleports the player given by his UserID, player name or entity id (as given by e.g. \"lpi\")\n   to the specified location. Use y = -1 to spawn on ground. Optional argument view direction\n   allows you to speify a direction you want to look in after teleporting. This can be either\n   of n, ne, e, se, s, sw, w, nw or north, northeast, etc.\n2. As 1, but destination given by another (online) player";
	}

	// Token: 0x060013CC RID: 5068 RVA: 0x00078E17 File Offset: 0x00077017
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "teleportplayer", "tele" };
	}

	// Token: 0x060013CD RID: 5069 RVA: 0x00078E30 File Offset: 0x00077030
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count != 2 && _params.Count != 4 && _params.Count != 5)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Wrong number of arguments, expected 2, 4 or 5, found " + _params.Count.ToString() + ".");
			return;
		}
		ClientInfo clientInfo = ConsoleHelper.ParseParamIdOrName(_params[0], true, false);
		Vector3 vector = default(Vector3);
		Vector3? vector2 = null;
		if (clientInfo == null && (GameManager.IsDedicatedServer || !ConsoleHelper.ParamIsLocalPlayer(_params[0], true, false)))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Playername or entity/userid id not found.");
			return;
		}
		if (_params.Count >= 4)
		{
			Vector3i vector3i;
			if (!base.TryParseV3i(_params, 1, out vector3i))
			{
				return;
			}
			vector = vector3i;
			vector2 = ((_params.Count == 5) ? base.TryParseViewDirection(_params[4]) : null);
		}
		else if (_params.Count == 2 && !base.TryGetDestinationFromPlayer(_params[1], out vector))
		{
			return;
		}
		base.ExecuteTeleport(clientInfo, vector, vector2);
	}
}
