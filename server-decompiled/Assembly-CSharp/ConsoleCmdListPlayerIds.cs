using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200023B RID: 571
[Preserve]
public class ConsoleCmdListPlayerIds : ConsoleCmdAbstract
{
	// Token: 0x0600110B RID: 4363 RVA: 0x0006C98A File Offset: 0x0006AB8A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "listplayerids", "lpi" };
	}

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x0600110C RID: 4364 RVA: 0x00061906 File Offset: 0x0005FB06
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x0600110D RID: 4365 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x0600110E RID: 4366 RVA: 0x0006C9A2 File Offset: 0x0006ABA2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Lists all players with their IDs for ingame commands";
	}

	// Token: 0x0600110F RID: 4367 RVA: 0x0006C9AC File Offset: 0x0006ABAC
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		World world = GameManager.Instance.World;
		int num = 0;
		foreach (KeyValuePair<int, EntityPlayer> keyValuePair in world.Players.dict)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("{0}. id={1}, {2}", ++num, keyValuePair.Value.entityId, keyValuePair.Value.EntityName));
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Total of " + world.Players.list.Count.ToString() + " in the game");
	}
}
