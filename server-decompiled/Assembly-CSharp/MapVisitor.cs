using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200146D RID: 5229
public class MapVisitor
{
	// Token: 0x14000102 RID: 258
	// (add) Token: 0x0600A3B4 RID: 41908 RVA: 0x003D4D10 File Offset: 0x003D2F10
	// (remove) Token: 0x0600A3B5 RID: 41909 RVA: 0x003D4D48 File Offset: 0x003D2F48
	public event MapVisitor.VisitChunkDelegate OnVisitChunk;

	// Token: 0x14000103 RID: 259
	// (add) Token: 0x0600A3B6 RID: 41910 RVA: 0x003D4D80 File Offset: 0x003D2F80
	// (remove) Token: 0x0600A3B7 RID: 41911 RVA: 0x003D4DB8 File Offset: 0x003D2FB8
	public event MapVisitor.VisitMapDoneDelegate OnVisitMapDone;

	// Token: 0x17001373 RID: 4979
	// (get) Token: 0x0600A3B8 RID: 41912 RVA: 0x003D4DED File Offset: 0x003D2FED
	public Vector3i ChunkPosStart
	{
		get
		{
			return this.chunkPos1;
		}
	}

	// Token: 0x17001374 RID: 4980
	// (get) Token: 0x0600A3B9 RID: 41913 RVA: 0x003D4DF5 File Offset: 0x003D2FF5
	public Vector3i ChunkPosEnd
	{
		get
		{
			return this.chunkPos2;
		}
	}

	// Token: 0x17001375 RID: 4981
	// (get) Token: 0x0600A3BA RID: 41914 RVA: 0x003D4DFD File Offset: 0x003D2FFD
	public Vector3i WorldPosStart
	{
		get
		{
			return new Vector3i(this.chunkPos1.x << 4, 0, this.chunkPos1.z << 4);
		}
	}

	// Token: 0x17001376 RID: 4982
	// (get) Token: 0x0600A3BB RID: 41915 RVA: 0x003D4E1F File Offset: 0x003D301F
	public Vector3i WorldPosEnd
	{
		get
		{
			return new Vector3i((this.chunkPos2.x + 1 << 4) - 1, 255, (this.chunkPos2.z + 1 << 4) - 1);
		}
	}

	// Token: 0x0600A3BC RID: 41916 RVA: 0x003D4E50 File Offset: 0x003D3050
	public MapVisitor(Vector3i _worldPos1, Vector3i _worldPos2)
	{
		int x = _worldPos1.x;
		int z = _worldPos1.z;
		int x2 = _worldPos2.x;
		int z2 = _worldPos2.z;
		this.chunkPos1 = new Vector3i(World.toChunkXZ((x <= x2) ? x : x2), 0, World.toChunkXZ((z <= z2) ? z : z2));
		this.chunkPos2 = new Vector3i(World.toChunkXZ((x <= x2) ? x2 : x), 0, World.toChunkXZ((z <= z2) ? z2 : z));
	}

	// Token: 0x0600A3BD RID: 41917 RVA: 0x003D4ECB File Offset: 0x003D30CB
	public void Start()
	{
		if (!this.hasBeenStarted)
		{
			this.coroutine = ThreadManager.StartCoroutine(this.visitCo());
			this.hasBeenStarted = true;
		}
	}

	// Token: 0x0600A3BE RID: 41918 RVA: 0x003D4EED File Offset: 0x003D30ED
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

	// Token: 0x0600A3BF RID: 41919 RVA: 0x003D4F28 File Offset: 0x003D3128
	public bool IsRunning()
	{
		return this.coroutine != null;
	}

	// Token: 0x0600A3C0 RID: 41920 RVA: 0x003D4F33 File Offset: 0x003D3133
	public bool HasBeenStarted()
	{
		return this.hasBeenStarted;
	}

	// Token: 0x0600A3C1 RID: 41921 RVA: 0x003D4F3B File Offset: 0x003D313B
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

	// Token: 0x0600A3C2 RID: 41922 RVA: 0x003D4F4A File Offset: 0x003D314A
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 chunkPosToBlockPos(int _x, int _z)
	{
		return new Vector3((float)this.chunkXZtoBlockXZ(_x), 0f, (float)this.chunkXZtoBlockXZ(_z));
	}

	// Token: 0x0600A3C3 RID: 41923 RVA: 0x003D4F66 File Offset: 0x003D3166
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkXZtoBlockXZ(int _xz)
	{
		if (_xz >= 0)
		{
			return _xz * 16;
		}
		return _xz * 16 + 1;
	}

	// Token: 0x04007AD0 RID: 31440
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine coroutine;

	// Token: 0x04007AD1 RID: 31441
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkManager.ChunkObserver observer;

	// Token: 0x04007AD2 RID: 31442
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Vector3i chunkPos1;

	// Token: 0x04007AD3 RID: 31443
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Vector3i chunkPos2;

	// Token: 0x04007AD4 RID: 31444
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasBeenStarted;

	// Token: 0x0200146E RID: 5230
	// (Invoke) Token: 0x0600A3C5 RID: 41925
	public delegate void VisitChunkDelegate(Chunk _chunk, int _chunksVisited, int _chunksTotal, float _elapsedSeconds);

	// Token: 0x0200146F RID: 5231
	// (Invoke) Token: 0x0600A3C9 RID: 41929
	public delegate void VisitMapDoneDelegate(int _chunks, float _elapsedSeconds);
}
