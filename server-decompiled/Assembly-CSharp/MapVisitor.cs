using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200145B RID: 5211
public class MapVisitor
{
	// Token: 0x14000102 RID: 258
	// (add) Token: 0x0600A35C RID: 41820 RVA: 0x003D290C File Offset: 0x003D0B0C
	// (remove) Token: 0x0600A35D RID: 41821 RVA: 0x003D2944 File Offset: 0x003D0B44
	public event MapVisitor.VisitChunkDelegate OnVisitChunk;

	// Token: 0x14000103 RID: 259
	// (add) Token: 0x0600A35E RID: 41822 RVA: 0x003D297C File Offset: 0x003D0B7C
	// (remove) Token: 0x0600A35F RID: 41823 RVA: 0x003D29B4 File Offset: 0x003D0BB4
	public event MapVisitor.VisitMapDoneDelegate OnVisitMapDone;

	// Token: 0x17001367 RID: 4967
	// (get) Token: 0x0600A360 RID: 41824 RVA: 0x003D29E9 File Offset: 0x003D0BE9
	public Vector3i ChunkPosStart
	{
		get
		{
			return this.chunkPos1;
		}
	}

	// Token: 0x17001368 RID: 4968
	// (get) Token: 0x0600A361 RID: 41825 RVA: 0x003D29F1 File Offset: 0x003D0BF1
	public Vector3i ChunkPosEnd
	{
		get
		{
			return this.chunkPos2;
		}
	}

	// Token: 0x17001369 RID: 4969
	// (get) Token: 0x0600A362 RID: 41826 RVA: 0x003D29F9 File Offset: 0x003D0BF9
	public Vector3i WorldPosStart
	{
		get
		{
			return new Vector3i(this.chunkPos1.x << 4, 0, this.chunkPos1.z << 4);
		}
	}

	// Token: 0x1700136A RID: 4970
	// (get) Token: 0x0600A363 RID: 41827 RVA: 0x003D2A1B File Offset: 0x003D0C1B
	public Vector3i WorldPosEnd
	{
		get
		{
			return new Vector3i((this.chunkPos2.x + 1 << 4) - 1, 255, (this.chunkPos2.z + 1 << 4) - 1);
		}
	}

	// Token: 0x0600A364 RID: 41828 RVA: 0x003D2A4C File Offset: 0x003D0C4C
	public MapVisitor(Vector3i _worldPos1, Vector3i _worldPos2)
	{
		int x = _worldPos1.x;
		int z = _worldPos1.z;
		int x2 = _worldPos2.x;
		int z2 = _worldPos2.z;
		this.chunkPos1 = new Vector3i(World.toChunkXZ((x <= x2) ? x : x2), 0, World.toChunkXZ((z <= z2) ? z : z2));
		this.chunkPos2 = new Vector3i(World.toChunkXZ((x <= x2) ? x2 : x), 0, World.toChunkXZ((z <= z2) ? z2 : z));
	}

	// Token: 0x0600A365 RID: 41829 RVA: 0x003D2AC7 File Offset: 0x003D0CC7
	public void Start()
	{
		if (!this.hasBeenStarted)
		{
			this.coroutine = ThreadManager.StartCoroutine(this.visitCo());
			this.hasBeenStarted = true;
		}
	}

	// Token: 0x0600A366 RID: 41830 RVA: 0x003D2AE9 File Offset: 0x003D0CE9
	public void Stop()
	{
		if (this.hasBeenStarted && this.coroutine != null)
		{
			ThreadManager.StopCoroutine(this.coroutine);
			GameManager.Instance.RemoveChunkObserver(this.observer);
			this.observer = null;
			this.coroutine = null;
		}
	}

	// Token: 0x0600A367 RID: 41831 RVA: 0x003D2B24 File Offset: 0x003D0D24
	public bool IsRunning()
	{
		return this.coroutine != null;
	}

	// Token: 0x0600A368 RID: 41832 RVA: 0x003D2B2F File Offset: 0x003D0D2F
	public bool HasBeenStarted()
	{
		return this.hasBeenStarted;
	}

	// Token: 0x0600A369 RID: 41833 RVA: 0x003D2B37 File Offset: 0x003D0D37
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator visitCo()
	{
		int viewDim = 8;
		int num = this.chunkPos2.x - this.chunkPos1.x + 1;
		int num2 = this.chunkPos2.z - this.chunkPos1.z + 1;
		int chunksTotal = num * num2;
		int chunksDone = 0;
		float startTime = Time.time;
		int curChunkX = Math.Min(this.chunkPos1.x + viewDim, this.chunkPos2.x);
		int curChunkZ = Math.Min(this.chunkPos1.z + viewDim, this.chunkPos2.z);
		this.observer = GameManager.Instance.AddChunkObserver(this.chunkPosToBlockPos(curChunkX, curChunkZ), false, viewDim, -1);
		yield return null;
		while (curChunkX - viewDim <= this.chunkPos2.x && curChunkZ - viewDim <= this.chunkPos2.z)
		{
			this.observer.SetPosition(this.chunkPosToBlockPos(curChunkX, curChunkZ));
			int num3;
			for (int xOffset = -viewDim; xOffset <= viewDim; xOffset = num3 + 1)
			{
				for (int zOffset = -viewDim; zOffset <= viewDim; zOffset = num3 + 1)
				{
					if (curChunkX + xOffset >= this.chunkPos1.x && curChunkZ + zOffset >= this.chunkPos1.z && curChunkX + xOffset <= this.chunkPos2.x && curChunkZ + zOffset <= this.chunkPos2.z)
					{
						Chunk chunk;
						while ((chunk = GameManager.Instance.World.GetChunkSync(curChunkX + xOffset, curChunkZ + zOffset) as Chunk) == null || chunk.NeedsDecoration)
						{
							yield return null;
						}
						num3 = chunksDone;
						chunksDone = num3 + 1;
						if (this.OnVisitChunk != null)
						{
							float num4 = Time.time - startTime;
							this.OnVisitChunk(chunk, chunksDone, chunksTotal, num4);
						}
					}
					num3 = zOffset;
				}
				num3 = xOffset;
			}
			curChunkX += viewDim * 2 + 1;
			if (curChunkX - viewDim > this.chunkPos2.x)
			{
				curChunkX = Math.Min(this.chunkPos1.x + viewDim, this.chunkPos2.x);
				curChunkZ += viewDim * 2 + 1;
			}
		}
		yield return null;
		GameManager.Instance.RemoveChunkObserver(this.observer);
		this.observer = null;
		float num5 = Time.time - startTime;
		if (this.OnVisitMapDone != null)
		{
			this.OnVisitMapDone(chunksDone, num5);
		}
		this.coroutine = null;
		yield break;
	}

	// Token: 0x0600A36A RID: 41834 RVA: 0x003D2B46 File Offset: 0x003D0D46
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 chunkPosToBlockPos(int _x, int _z)
	{
		return new Vector3((float)this.chunkXZtoBlockXZ(_x), 0f, (float)this.chunkXZtoBlockXZ(_z));
	}

	// Token: 0x0600A36B RID: 41835 RVA: 0x003D2B62 File Offset: 0x003D0D62
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkXZtoBlockXZ(int _xz)
	{
		if (_xz >= 0)
		{
			return _xz * 16;
		}
		return _xz * 16 + 1;
	}

	// Token: 0x04007A8F RID: 31375
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine coroutine;

	// Token: 0x04007A90 RID: 31376
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkManager.ChunkObserver observer;

	// Token: 0x04007A91 RID: 31377
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Vector3i chunkPos1;

	// Token: 0x04007A92 RID: 31378
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Vector3i chunkPos2;

	// Token: 0x04007A93 RID: 31379
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasBeenStarted;

	// Token: 0x0200145C RID: 5212
	// (Invoke) Token: 0x0600A36D RID: 41837
	public delegate void VisitChunkDelegate(Chunk _chunk, int _chunksVisited, int _chunksTotal, float _elapsedSeconds);

	// Token: 0x0200145D RID: 5213
	// (Invoke) Token: 0x0600A371 RID: 41841
	public delegate void VisitMapDoneDelegate(int _chunks, float _elapsedSeconds);
}
