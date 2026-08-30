using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200143B RID: 5179
public class MapVisitor
{
	// Token: 0x14000103 RID: 259
	// (add) Token: 0x0600A2B0 RID: 41648 RVA: 0x003CDCFC File Offset: 0x003CBEFC
	// (remove) Token: 0x0600A2B1 RID: 41649 RVA: 0x003CDD34 File Offset: 0x003CBF34
	public event MapVisitor.VisitChunkDelegate OnVisitChunk;

	// Token: 0x14000104 RID: 260
	// (add) Token: 0x0600A2B2 RID: 41650 RVA: 0x003CDD6C File Offset: 0x003CBF6C
	// (remove) Token: 0x0600A2B3 RID: 41651 RVA: 0x003CDDA4 File Offset: 0x003CBFA4
	public event MapVisitor.VisitMapDoneDelegate OnVisitMapDone;

	// Token: 0x17001322 RID: 4898
	// (get) Token: 0x0600A2B4 RID: 41652 RVA: 0x003CDDD9 File Offset: 0x003CBFD9
	public Vector3i ChunkPosStart
	{
		get
		{
			return this.chunkPos1;
		}
	}

	// Token: 0x17001323 RID: 4899
	// (get) Token: 0x0600A2B5 RID: 41653 RVA: 0x003CDDE1 File Offset: 0x003CBFE1
	public Vector3i ChunkPosEnd
	{
		get
		{
			return this.chunkPos2;
		}
	}

	// Token: 0x17001324 RID: 4900
	// (get) Token: 0x0600A2B6 RID: 41654 RVA: 0x003CDDE9 File Offset: 0x003CBFE9
	public Vector3i WorldPosStart
	{
		get
		{
			return new Vector3i(this.chunkPos1.x << 4, 0, this.chunkPos1.z << 4);
		}
	}

	// Token: 0x17001325 RID: 4901
	// (get) Token: 0x0600A2B7 RID: 41655 RVA: 0x003CDE0B File Offset: 0x003CC00B
	public Vector3i WorldPosEnd
	{
		get
		{
			return new Vector3i((this.chunkPos2.x + 1 << 4) - 1, 255, (this.chunkPos2.z + 1 << 4) - 1);
		}
	}

	// Token: 0x0600A2B8 RID: 41656 RVA: 0x003CDE3C File Offset: 0x003CC03C
	public MapVisitor(Vector3i _worldPos1, Vector3i _worldPos2)
	{
		int x = _worldPos1.x;
		int z = _worldPos1.z;
		int x2 = _worldPos2.x;
		int z2 = _worldPos2.z;
		this.chunkPos1 = new Vector3i(World.toChunkXZ((x <= x2) ? x : x2), 0, World.toChunkXZ((z <= z2) ? z : z2));
		this.chunkPos2 = new Vector3i(World.toChunkXZ((x <= x2) ? x2 : x), 0, World.toChunkXZ((z <= z2) ? z2 : z));
	}

	// Token: 0x0600A2B9 RID: 41657 RVA: 0x003CDEB7 File Offset: 0x003CC0B7
	public void Start()
	{
		if (!this.hasBeenStarted)
		{
			this.coroutine = ThreadManager.StartCoroutine(this.visitCo());
			this.hasBeenStarted = true;
		}
	}

	// Token: 0x0600A2BA RID: 41658 RVA: 0x003CDED9 File Offset: 0x003CC0D9
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

	// Token: 0x0600A2BB RID: 41659 RVA: 0x003CDF14 File Offset: 0x003CC114
	public bool IsRunning()
	{
		return this.coroutine != null;
	}

	// Token: 0x0600A2BC RID: 41660 RVA: 0x003CDF1F File Offset: 0x003CC11F
	public bool HasBeenStarted()
	{
		return this.hasBeenStarted;
	}

	// Token: 0x0600A2BD RID: 41661 RVA: 0x003CDF27 File Offset: 0x003CC127
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

	// Token: 0x0600A2BE RID: 41662 RVA: 0x003CDF36 File Offset: 0x003CC136
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 chunkPosToBlockPos(int _x, int _z)
	{
		return new Vector3((float)this.chunkXZtoBlockXZ(_x), 0f, (float)this.chunkXZtoBlockXZ(_z));
	}

	// Token: 0x0600A2BF RID: 41663 RVA: 0x003CDF52 File Offset: 0x003CC152
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkXZtoBlockXZ(int _xz)
	{
		if (_xz >= 0)
		{
			return _xz * 16;
		}
		return _xz * 16 + 1;
	}

	// Token: 0x04007A14 RID: 31252
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine coroutine;

	// Token: 0x04007A15 RID: 31253
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkManager.ChunkObserver observer;

	// Token: 0x04007A16 RID: 31254
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Vector3i chunkPos1;

	// Token: 0x04007A17 RID: 31255
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Vector3i chunkPos2;

	// Token: 0x04007A18 RID: 31256
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasBeenStarted;

	// Token: 0x0200143C RID: 5180
	// (Invoke) Token: 0x0600A2C1 RID: 41665
	public delegate void VisitChunkDelegate(Chunk _chunk, int _chunksVisited, int _chunksTotal, float _elapsedSeconds);

	// Token: 0x0200143D RID: 5181
	// (Invoke) Token: 0x0600A2C5 RID: 41669
	public delegate void VisitMapDoneDelegate(int _chunks, float _elapsedSeconds);
}
