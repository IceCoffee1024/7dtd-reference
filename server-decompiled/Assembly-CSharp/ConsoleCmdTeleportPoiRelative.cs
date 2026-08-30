using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002AA RID: 682
[Preserve]
public class ConsoleCmdTeleportPoiRelative : ConsoleCmdTeleportsAbs
{
	// Token: 0x060013C1 RID: 5057 RVA: 0x000784BF File Offset: 0x000766BF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Teleport the local player within the current POI";
	}

	// Token: 0x060013C2 RID: 5058 RVA: 0x000784C6 File Offset: 0x000766C6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "\n\t\t\tUsage:\n\t\t\t|  1. teleportpoirelative <x> <y> <z> [view direction]\n\t\t\t|1. Teleports the local player to the specified location relative to the bounds of the current POI. View\n\t\t\t|direction is an optional specifier to select the direction you want to look into after teleporting. This\n\t\t\t|can be either of n, ne, e, se, s, sw, w, nw or north, northeast, etc.\n\t\t\t".Unindent(true);
	}

	// Token: 0x060013C3 RID: 5059 RVA: 0x000784D3 File Offset: 0x000766D3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "teleportpoirelative", "tppr" };
	}

	// Token: 0x060013C4 RID: 5060 RVA: 0x000784EC File Offset: 0x000766EC
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
