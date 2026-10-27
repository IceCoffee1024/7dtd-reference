using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002B3 RID: 691
[Preserve]
public class ConsoleCmdTeleportPoiRelative : ConsoleCmdTeleportsAbs
{
	// Token: 0x060013D4 RID: 5076 RVA: 0x00078F9F File Offset: 0x0007719F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Teleport the local player within the current POI";
	}

	// Token: 0x060013D5 RID: 5077 RVA: 0x00078FA6 File Offset: 0x000771A6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "\r\n\t\t\tUsage:\r\n\t\t\t|  1. teleportpoirelative <x> <y> <z> [view direction]\r\n\t\t\t|1. Teleports the local player to the specified location relative to the bounds of the current POI. View\r\n\t\t\t|direction is an optional specifier to select the direction you want to look into after teleporting. This\r\n\t\t\t|can be either of n, ne, e, se, s, sw, w, nw or north, northeast, etc.\r\n\t\t\t".Unindent(true);
	}

	// Token: 0x060013D6 RID: 5078 RVA: 0x00078FB3 File Offset: 0x000771B3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "teleportpoirelative", "tppr" };
	}

	// Token: 0x060013D7 RID: 5079 RVA: 0x00078FCC File Offset: 0x000771CC
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!_senderInfo.IsLocalGame && _senderInfo.RemoteClientInfo == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command can only be used on clients");
			return;
		}
		PrefabInstance prefab = base.GetExecutingEntityPlayer(_senderInfo).prefab;
		if (prefab == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Player has to be within the bounds of a prefab!");
			return;
		}
		if (_params.Count < 3 || _params.Count > 4)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Wrong number of arguments, expected 3 to 4, found " + _params.Count.ToString() + ".");
			return;
		}
		Vector3i worldPositionOfPoiOffset;
		if (!base.TryParseV3i(_params, 0, out worldPositionOfPoiOffset))
		{
			return;
		}
		Vector3? vector = ((_params.Count == 4) ? base.TryParseViewDirection(_params[3]) : null);
		worldPositionOfPoiOffset = prefab.GetWorldPositionOfPoiOffset(worldPositionOfPoiOffset);
		base.ExecuteTeleport(_senderInfo.RemoteClientInfo, worldPositionOfPoiOffset, vector);
	}
}
