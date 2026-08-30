using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200151E RID: 5406
public static class ProfilerGameUtils
{
	// Token: 0x0600A9EE RID: 43502 RVA: 0x003F6DA4 File Offset: 0x003F4FA4
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

	// Token: 0x0600A9EF RID: 43503 RVA: 0x003F6E32 File Offset: 0x003F5032
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

	// Token: 0x0600A9F0 RID: 43504 RVA: 0x003F6E4F File Offset: 0x003F504F
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

	// Token: 0x0600A9F1 RID: 43505 RVA: 0x003F6E65 File Offset: 0x003F5065
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
