using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000233 RID: 563
[Preserve]
public class ConsoleCmdListPlayerIds : ConsoleCmdAbstract
{
	// Token: 0x060010FE RID: 4350 RVA: 0x0006BF79 File Offset: 0x0006A179
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "listplayerids", "lpi" };
	}

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x060010FF RID: 4351 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06001100 RID: 4352 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001101 RID: 4353 RVA: 0x0006BF91 File Offset: 0x0006A191
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Lists all players with their IDs for ingame commands";
	}

	// Token: 0x06001102 RID: 4354 RVA: 0x0006BF98 File Offset: 0x0006A198
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
