using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020001FC RID: 508
[Preserve]
public class ConsoleCmdChunkReset : ConsoleCmdAbstract
{
	// Token: 0x06000F76 RID: 3958 RVA: 0x0006497F File Offset: 0x00062B7F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "chunkreset", "cr" };
	}

	// Token: 0x06000F77 RID: 3959 RVA: 0x00064997 File Offset: 0x00062B97
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "resets the specified chunks";
	}

	// Token: 0x06000F78 RID: 3960 RVA: 0x0006499E File Offset: 0x00062B9E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n  1. chunkreset <x1> <z1> <x2> <z2>\n  2. chunkreset [f]\n1. Rebuilds the chunks that contain the given coordinate range.\n2. Can only be executed by a player in the ingame console! Behaviour depends on whether the\n   player is currently within the bounds of a POI:\n   Within a POI: The POI is reset.\n   Not within a POI: The chunk the player is in and the eight chunks around that one are\n     rebuilt. Not deco! Does not reload POI data!\n   d - regen deco\n   f - fully regenerates chunks (may cause double entities!)\n   u, utimed, ue - Unload chunks or entities\n   nq - Enqueue a 3x3 group of chunks centred around the player to be reset when unsynced, unless they are otherwise protected from the chunk reset system.";
	}

	// Token: 0x06000F79 RID: 3961 RVA: 0x000649A5 File Offset: 0x00062BA5
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GameManager.Instance.StartCoroutine(this.execute(_params, _senderInfo));
	}

	// Token: 0x06000F7A RID: 3962 RVA: 0x000649BA File Offset: 0x00062BBA
	public IEnumerator execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		World world = GameManager.Instance.World;
		if (_params.Count >= 2)
		{
			int num;
			if (!int.TryParse(_params[0], out num))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("x1 is not a valid integer");
				yield break;
			}
			int num2;
			if (!int.TryParse(_params[1], out num2))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("z1 is not a valid integer");
				yield break;
			}
			int num3 = num;
			int num4 = num2;
			if (_params.Count >= 3 && !int.TryParse(_params[2], out num3))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("x2 is not a valid integer");
				yield break;
			}
			if (_params.Count >= 4 && !int.TryParse(_params[3], out num4))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("z2 is not a valid integer");
				yield break;
			}
			Vector2i vector2i = new Vector2i((num <= num3) ? num : num3, (num2 <= num4) ? num2 : num4);
			Vector2i vector2i2 = new Vector2i((num <= num3) ? num3 : num, (num2 <= num4) ? num4 : num2);
			if (vector2i2.x - vector2i.x > 16384 || vector2i2.y - vector2i.y > 16384)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("area too big");
				yield break;
			}
			vector2i = World.toChunkXZ(vector2i);
			vector2i2 = World.toChunkXZ(vector2i2);
			HashSetLong hashSetLong = new HashSetLong();
			for (int i = vector2i.x; i <= vector2i2.x; i++)
			{
				for (int j = vector2i.y; j <= vector2i2.y; j++)
				{
					hashSetLong.Add(WorldChunkCache.MakeChunkKey(i, j));
				}
			}
			ChunkCluster chunkCache = world.ChunkCache;
			ChunkProviderGenerateWorld chunkProviderGenerateWorld = world.ChunkCache.ChunkProvider as ChunkProviderGenerateWorld;
			if (chunkProviderGenerateWorld != null)
			{
				LockManager.Instance.ForceUnlockByChunk(hashSetLong);
				chunkProviderGenerateWorld.RemoveChunks(hashSetLong);
				foreach (long num5 in hashSetLong)
				{
					if (!chunkProviderGenerateWorld.GenerateSingleChunk(chunkCache, num5, true))
					{
						SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Failed regenerating chunk at position {0}/{1}", WorldChunkCache.extractX(num5) << 4, WorldChunkCache.extractZ(num5) << 4));
					}
				}
				GameManager.Instance.World.m_ChunkManager.ResendChunksToClients(hashSetLong);
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Reset chunks covering area {0}/{1} to {2}/{3} (chunk coordinates {4} to {5}).", new object[] { num, num2, num3, num4, vector2i, vector2i2 }));
			}
			else
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Can not reset chunks on this game");
			}
		}
		else
		{
			if (_params.Count > 1 || (_senderInfo.RemoteClientInfo == null && (!_senderInfo.IsLocalGame || GameManager.IsDedicatedServer)))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Invalid arguments, please see command help.");
				yield break;
			}
			Vector3 vector;
			if (_senderInfo.RemoteClientInfo != null)
			{
				vector = world.Players.dict[_senderInfo.RemoteClientInfo.entityId].position;
			}
			else
			{
				vector = world.GetLocalPlayers()[0].position;
			}
			int num = World.toChunkXZ((int)vector.x) - 1;
			int num2 = World.toChunkXZ((int)vector.z) - 1;
			int num3 = num + 2;
			int num4 = num2 + 2;
			HashSetLong hashSetLong2 = new HashSetLong();
			for (int k = num; k <= num3; k++)
			{
				for (int l = num2; l <= num4; l++)
				{
					hashSetLong2.Add(WorldChunkCache.MakeChunkKey(k, l));
				}
			}
			if (_params.Count == 1)
			{
				ChunkCluster chunkCache2 = world.ChunkCache;
				ChunkProviderGenerateWorld chunkProviderGenerateWorld2 = chunkCache2.ChunkProvider as ChunkProviderGenerateWorld;
				if (chunkProviderGenerateWorld2 == null)
				{
					yield break;
				}
				if (_params[0] == "nq")
				{
					foreach (long num6 in hashSetLong2)
					{
						chunkProviderGenerateWorld2.RequestChunkReset(num6);
					}
					yield break;
				}
				if (_params[0] == "d")
				{
					LockManager.Instance.ForceUnlockByChunk(hashSetLong2);
					foreach (long num7 in hashSetLong2)
					{
						Chunk chunkSync = chunkCache2.GetChunkSync(num7);
						if (chunkSync != null)
						{
							chunkSync.NeedsLightDecoration = true;
							chunkSync.NeedsLightCalculation = true;
						}
					}
					GameManager.Instance.World.m_ChunkManager.ResendChunksToClients(hashSetLong2);
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Generate deco around player");
				}
				else if (_params[0] == "f")
				{
					LockManager.Instance.ForceUnlockByChunk(hashSetLong2);
					foreach (long num8 in hashSetLong2)
					{
						if (!chunkProviderGenerateWorld2.GenerateSingleChunk(chunkCache2, num8, true))
						{
							SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Failed regenerating chunk at position {0}/{1}", WorldChunkCache.extractX(num8) << 4, WorldChunkCache.extractZ(num8) << 4));
						}
					}
					GameManager.Instance.World.m_ChunkManager.ResendChunksToClients(hashSetLong2);
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Generate chunks around player");
				}
				else
				{
					if (_params[0] == "r")
					{
						using (HashSetLong.Enumerator enumerator = hashSetLong2.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								long num9 = enumerator.Current;
								Chunk chunkSync2 = chunkCache2.GetChunkSync(num9);
								if (chunkSync2 != null)
								{
									chunkSync2.NeedsRegeneration = true;
								}
							}
							yield break;
						}
					}
					if (_params[0] == "u")
					{
						foreach (long num10 in hashSetLong2)
						{
							world.m_ChunkManager.RemoveChunk(num10);
						}
						SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Unload around player");
					}
					else if (_params[0] == "utimed")
					{
						if (this.unloadTimed != null)
						{
							GameManager.Instance.StopCoroutine(this.unloadTimed);
							this.unloadTimed = null;
						}
						else
						{
							this.unloadTimed = GameManager.Instance.StartCoroutine(this.UnloadTimed());
						}
						SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Unload timed at player {0}", new object[] { this.unloadTimed != null });
					}
					else if (_params[0] == "ue")
					{
						foreach (long num11 in hashSetLong2)
						{
							Chunk chunkSync3 = chunkCache2.GetChunkSync(num11);
							if (chunkSync3 != null)
							{
								for (int m = 0; m < chunkSync3.entityLists.Length; m++)
								{
									List<Entity> list = chunkSync3.entityLists[m];
									for (int n = list.Count - 1; n >= 0; n--)
									{
										Entity entity = list[n];
										if (!entity.bWillRespawn && (!(entity.AttachedMainEntity != null) || !entity.AttachedMainEntity.bWillRespawn))
										{
											world.unloadEntity(entity, EnumRemoveEntityReason.Unloaded);
											list.RemoveAt(n);
										}
									}
								}
							}
						}
						SingletonMonoBehaviour<SdtdConsole>.Instance.Output("UnloadEntities around player");
					}
				}
			}
			else
			{
				DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
				List<PrefabInstance> prefabsFromWorldPosInside;
				if (dynamicPrefabDecorator != null && (prefabsFromWorldPosInside = dynamicPrefabDecorator.GetPrefabsFromWorldPosInside(vector, FastTags<TagGroup.Global>.none)) != null)
				{
					yield return world.ResetPOIS(prefabsFromWorldPosInside, QuestEventManager.manualResetTag, -1, null, null);
				}
				else
				{
					LockManager.Instance.ForceUnlockByChunk(hashSetLong2);
					world.RebuildTerrain(hashSetLong2, Vector3i.zero, Vector3i.zero, false, true, true, false);
					GameManager.Instance.World.m_ChunkManager.ResendChunksToClients(hashSetLong2);
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Reset chunks around player");
				}
			}
		}
		yield break;
	}

	// Token: 0x06000F7B RID: 3963 RVA: 0x000649D7 File Offset: 0x00062BD7
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator UnloadTimed()
	{
		int num4;
		for (int i = 0; i < 99999; i = num4)
		{
			World world = GameManager.Instance.World;
			if (world == null)
			{
				break;
			}
			EntityPlayerLocal primaryPlayer = world.GetPrimaryPlayer();
			if (!primaryPlayer)
			{
				break;
			}
			Vector3i blockPosition = primaryPlayer.GetBlockPosition();
			for (int j = -16; j <= 16; j += 16)
			{
				for (int k = -16; k <= 16; k += 16)
				{
					int num = World.toChunkXZ(blockPosition.x + k);
					int num2 = World.toChunkXZ(blockPosition.z + j);
					long num3 = WorldChunkCache.MakeChunkKey(num, num2);
					world.m_ChunkManager.RemoveChunk(num3);
				}
			}
			yield return new WaitForSeconds(1.5f);
			num4 = i + 1;
		}
		this.unloadTimed = null;
		yield break;
	}

	// Token: 0x04000C94 RID: 3220
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine unloadTimed;
}
