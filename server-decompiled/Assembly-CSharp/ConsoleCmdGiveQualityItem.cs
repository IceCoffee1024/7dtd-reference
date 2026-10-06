using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000225 RID: 549
[Preserve]
public class ConsoleCmdGiveQualityItem : ConsoleCmdAbstract
{
	// Token: 0x0600108E RID: 4238 RVA: 0x00069809 File Offset: 0x00067A09
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "giveself" };
	}

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x0600108F RID: 4239 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x06001090 RID: 4240 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001091 RID: 4241 RVA: 0x0006981C File Offset: 0x00067A1C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (GameManager.IsDedicatedServer)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Cannot execute giveself on dedicated server, please execute as a client");
			return;
		}
		if (_params.Count < 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("giveself requires an itemname as parameter");
			return;
		}
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		ItemValue item = ItemClass.GetItem(_params[0], true);
		if (item.IsEmpty())
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Unknown itemname given");
			return;
		}
		int num = 6;
		if (_params.Count > 1 && !int.TryParse(_params[1], out num))
		{
			num = 6;
		}
		int num2 = 1;
		if (_params.Count > 2 && !int.TryParse(_params[2], out num2))
		{
			num2 = 1;
		}
		bool flag = false;
		if (_params.Count > 3 && !StringParsers.TryParseBool(_params[3], out flag))
		{
			flag = false;
		}
		bool flag2 = true;
		if (_params.Count > 4 && !StringParsers.TryParseBool(_params[4], out flag2))
		{
			flag2 = true;
		}
		for (int i = 0; i < num2; i++)
		{
			ItemStack itemStack = new ItemStack(new ItemValue(item.type, num, num, flag2, null, 1f), 1);
			if (!flag)
			{
				GameManager.Instance.ItemDropServer(itemStack, primaryPlayer.position, Vector3.zero, -1, 60f, false);
			}
			else
			{
				primaryPlayer.bag.AddItem(itemStack);
			}
		}
	}

	// Token: 0x06001092 RID: 4242 RVA: 0x00069968 File Offset: 0x00067B68
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "usage: giveself itemName [qualityLevel=" + 6.ToString() + "] [count=1] [putInInventory=false] [spawnWithMods=true]";
	}
}
