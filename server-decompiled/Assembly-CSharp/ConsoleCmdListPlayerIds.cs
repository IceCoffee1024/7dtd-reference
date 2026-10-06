using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000239 RID: 569
[Preserve]
public class ConsoleCmdListPlayerIds : ConsoleCmdAbstract
{
	// Token: 0x06001107 RID: 4359 RVA: 0x0006C7BA File Offset: 0x0006A9BA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "listplayerids", "lpi" };
	}

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x06001108 RID: 4360 RVA: 0x00061642 File Offset: 0x0005F842
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x06001109 RID: 4361 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x0600110A RID: 4362 RVA: 0x0006C7D2 File Offset: 0x0006A9D2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Lists all players with their IDs for ingame commands";
	}

	// Token: 0x0600110B RID: 4363 RVA: 0x0006C7DC File Offset: 0x0006A9DC
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
