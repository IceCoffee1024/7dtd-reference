using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000236 RID: 566
[Preserve]
public class ConsoleCmdListEntities : ConsoleCmdAbstract
{
	// Token: 0x1700019D RID: 413
	// (get) Token: 0x060010F5 RID: 4341 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700019E RID: 414
	// (get) Token: 0x060010F6 RID: 4342 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x060010F7 RID: 4343 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060010F8 RID: 4344 RVA: 0x0006C4A8 File Offset: 0x0006A6A8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "listents", "le" };
	}

	// Token: 0x060010F9 RID: 4345 RVA: 0x0006C4C0 File Offset: 0x0006A6C0
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		int num = 0;
		for (int i = GameManager.Instance.World.Entities.list.Count - 1; i >= 0; i--)
		{
			Entity entity = GameManager.Instance.World.Entities.list[i];
			EntityAlive entityAlive = null;
			if (entity is EntityAlive)
			{
				entityAlive = (EntityAlive)entity;
			}
			SdtdConsole instance = SingletonMonoBehaviour<SdtdConsole>.Instance;
			string[] array = new string[17];
			int num2 = 0;
			int num3;
			num = (num3 = num + 1);
			array[num2] = num3.ToString();
			array[1] = ". id=";
			array[2] = entity.entityId.ToString();
			array[3] = ", ";
			array[4] = entity.ToString();
			array[5] = ", pos=";
			array[6] = entity.GetPosition().ToCultureInvariantString();
			array[7] = ", rot=";
			array[8] = entity.rotation.ToCultureInvariantString();
			array[9] = ", lifetime=";
			array[10] = ((entity.lifetime == float.MaxValue) ? "float.Max" : entity.lifetime.ToCultureInvariantString("0.0"));
			array[11] = ", remote=";
			array[12] = entity.isEntityRemote.ToString();
			array[13] = ", dead=";
			array[14] = entity.IsDead().ToString();
			array[15] = ", ";
			array[16] = ((entityAlive != null) ? ("health=" + entityAlive.Health.ToString()) : "");
			instance.Output(string.Concat(array));
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Total of " + GameManager.Instance.World.Entities.Count.ToString() + " in the game");
	}

	// Token: 0x060010FA RID: 4346 RVA: 0x0006C672 File Offset: 0x0006A872
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "lists all entities";
	}
}
