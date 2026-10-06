using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000291 RID: 657
[Preserve]
public class ConsoleCmdShowChunkData : ConsoleCmdAbstract
{
	// Token: 0x17000201 RID: 513
	// (get) Token: 0x0600131F RID: 4895 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000202 RID: 514
	// (get) Token: 0x06001320 RID: 4896 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x17000203 RID: 515
	// (get) Token: 0x06001321 RID: 4897 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001322 RID: 4898 RVA: 0x00075FB3 File Offset: 0x000741B3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showchunkdata", "sc" };
	}

	// Token: 0x06001323 RID: 4899 RVA: 0x00075FCC File Offset: 0x000741CC
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

	// Token: 0x06001324 RID: 4900 RVA: 0x00076240 File Offset: 0x00074440
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "shows some date of the current chunk";
	}
}
