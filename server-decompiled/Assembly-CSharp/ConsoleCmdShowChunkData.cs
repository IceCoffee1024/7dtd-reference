using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200028A RID: 650
[Preserve]
public class ConsoleCmdShowChunkData : ConsoleCmdAbstract
{
	// Token: 0x17000208 RID: 520
	// (get) Token: 0x06001310 RID: 4880 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x06001311 RID: 4881 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x06001312 RID: 4882 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001313 RID: 4883 RVA: 0x000756A3 File Offset: 0x000738A3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showchunkdata", "sc" };
	}

	// Token: 0x06001314 RID: 4884 RVA: 0x000756BC File Offset: 0x000738BC
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		foreach (KeyValuePair<int, EntityPlayer> keyValuePair in GameManager.Instance.World.Players.dict)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Player: " + keyValuePair.Value.EntityName);
			Chunk chunk = (Chunk)GameManager.Instance.World.GetChunkFromWorldPos(Utils.Fastfloor(keyValuePair.Value.position.x), Utils.Fastfloor(keyValuePair.Value.position.y), Utils.Fastfloor(keyValuePair.Value.position.z));
			if (chunk != null)
			{
				SdtdConsole instance = SingletonMonoBehaviour<SdtdConsole>.Instance;
				string text = " On Chunk: ";
				Chunk chunk2 = chunk;
				instance.Output(text + ((chunk2 != null) ? chunk2.ToString() : null) + " Mem used: " + chunk.GetUsedMem().ToString());
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(" Tile Entities:");
				DictionaryList<Vector3i, TileEntity> tileEntities = chunk.GetTileEntities();
				for (int i = 0; i < tileEntities.list.Count; i++)
				{
					TileEntity tileEntity = tileEntities.list[i];
					SdtdConsole instance2 = SingletonMonoBehaviour<SdtdConsole>.Instance;
					string text2 = "  - ";
					TileEntity tileEntity2 = tileEntity;
					instance2.Output(text2 + ((tileEntity2 != null) ? tileEntity2.ToString() : null));
				}
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(" Entities:");
				foreach (List<Entity> list in chunk.entityLists)
				{
					for (int k = 0; k < list.Count; k++)
					{
						Entity entity = list[k];
						SdtdConsole instance3 = SingletonMonoBehaviour<SdtdConsole>.Instance;
						string text3 = "  - ";
						Entity entity2 = entity;
						instance3.Output(text3 + ((entity2 != null) ? entity2.ToString() : null));
					}
				}
				SdtdConsole instance4 = SingletonMonoBehaviour<SdtdConsole>.Instance;
				string text4 = " DominantBiome: ";
				BiomeDefinition biome = GameManager.Instance.World.Biomes.GetBiome(chunk.DominantBiome);
				instance4.Output(text4 + ((biome != null) ? biome.ToString() : null));
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(" AreaMasterDominantBiome: " + ((chunk.AreaMasterDominantBiome != byte.MaxValue) ? GameManager.Instance.World.Biomes.GetBiome(chunk.AreaMasterDominantBiome).ToString() : "-"));
			}
		}
	}

	// Token: 0x06001315 RID: 4885 RVA: 0x00075930 File Offset: 0x00073B30
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "shows some date of the current chunk";
	}
}
