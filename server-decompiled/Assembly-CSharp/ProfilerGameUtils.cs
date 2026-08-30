using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001503 RID: 5379
public static class ProfilerGameUtils
{
	// Token: 0x0600A8F1 RID: 43249 RVA: 0x003F1A64 File Offset: 0x003EFC64
	public static bool TryGetFlyingPlayer(out EntityPlayerLocal player)
	{
		World world = GameManager.Instance.World;
		if (world == null)
		{
			player = null;
			return false;
		}
		player = world.GetPrimaryPlayer();
		if (player == null)
		{
			player = null;
			return false;
		}
		if (player.AttachedToEntity)
		{
			player.Detach();
		}
		player.IsGodMode.Value = true;
		player.IsNoCollisionMode.Value = true;
		player.IsFlyMode.Value = true;
		player.Buffs.AddBuff("god", -1, true, false, -1f);
		return true;
	}

	// Token: 0x0600A8F2 RID: 43250 RVA: 0x003F1AF2 File Offset: 0x003EFCF2
	public static IEnumerator WaitForSingleChunkToLoad(int chunkX, int chunkZ, ChunkConditions.Delegate chunkCondition)
	{
		ChunkCluster cc = GameManager.Instance.World.ChunkCache;
		Chunk chunk = null;
		while (chunk == null)
		{
			chunk = cc.GetChunkSync(chunkX, chunkZ);
			yield return null;
		}
		ProfilerGameUtils.WaitForSingleChunkToLoad(chunk, chunkCondition);
		yield break;
	}

	// Token: 0x0600A8F3 RID: 43251 RVA: 0x003F1B0F File Offset: 0x003EFD0F
	public static IEnumerator WaitForSingleChunkToLoad(Chunk chunk, ChunkConditions.Delegate chunkCondition)
	{
		if (chunk == null)
		{
			yield break;
		}
		float startTime = Time.realtimeSinceStartup;
		while (!chunkCondition(chunk))
		{
			if (Time.realtimeSinceStartup - startTime > 30f)
			{
				yield break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600A8F4 RID: 43252 RVA: 0x003F1B25 File Offset: 0x003EFD25
	public static IEnumerator WaitForChunksAroundObserverToLoad(ChunkManager.ChunkObserver observer, ChunkConditions.Delegate chunkCondition)
	{
		ChunkCluster cc = GameManager.Instance.World.ChunkCache;
		bool isLoaded = false;
		float startTime = Time.realtimeSinceStartup;
		while (!isLoaded)
		{
			isLoaded = true;
			for (int i = 0; i <= observer.viewDim - 2; i++)
			{
				foreach (long num in observer.chunksAround.buckets.array[i])
				{
					Chunk chunkSync = cc.GetChunkSync(num);
					if (chunkSync == null)
					{
						isLoaded = false;
						break;
					}
					if (!chunkCondition(chunkSync))
					{
						isLoaded = false;
						break;
					}
				}
			}
			if (Time.realtimeSinceStartup - startTime > 30f)
			{
				isLoaded = true;
				Debug.LogErrorFormat("Could not load Chunks at player chunk pos ({0},{1}). Forcing continue", new object[]
				{
					observer.curChunkPos.x,
					observer.curChunkPos.z
				});
			}
			yield return false;
		}
		yield break;
	}
}
