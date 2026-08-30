using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001420 RID: 5152
public class MapVisitor
{
	// Token: 0x14000102 RID: 258
	// (add) Token: 0x0600A1B3 RID: 41395 RVA: 0x003C89B8 File Offset: 0x003C6BB8
	// (remove) Token: 0x0600A1B4 RID: 41396 RVA: 0x003C89F0 File Offset: 0x003C6BF0
	public event MapVisitor.VisitChunkDelegate OnVisitChunk;

	// Token: 0x14000103 RID: 259
	// (add) Token: 0x0600A1B5 RID: 41397 RVA: 0x003C8A28 File Offset: 0x003C6C28
	// (remove) Token: 0x0600A1B6 RID: 41398 RVA: 0x003C8A60 File Offset: 0x003C6C60
	public event MapVisitor.VisitMapDoneDelegate OnVisitMapDone;

	// Token: 0x170012E9 RID: 4841
	// (get) Token: 0x0600A1B7 RID: 41399 RVA: 0x003C8A95 File Offset: 0x003C6C95
	public Vector3i ChunkPosStart
	{
		get
		{
			return this.chunkPos1;
		}
	}

	// Token: 0x170012EA RID: 4842
	// (get) Token: 0x0600A1B8 RID: 41400 RVA: 0x003C8A9D File Offset: 0x003C6C9D
	public Vector3i ChunkPosEnd
	{
		get
		{
			return this.chunkPos2;
		}
	}

	// Token: 0x170012EB RID: 4843
	// (get) Token: 0x0600A1B9 RID: 41401 RVA: 0x003C8AA5 File Offset: 0x003C6CA5
	public Vector3i WorldPosStart
	{
		get
		{
			return new Vector3i(this.chunkPos1.x << 4, 0, this.chunkPos1.z << 4);
		}
	}

	// Token: 0x170012EC RID: 4844
	// (get) Token: 0x0600A1BA RID: 41402 RVA: 0x003C8AC7 File Offset: 0x003C6CC7
	public Vector3i WorldPosEnd
	{
		get
		{
			return new Vector3i((this.chunkPos2.x + 1 << 4) - 1, 255, (this.chunkPos2.z + 1 << 4) - 1);
		}
	}

	// Token: 0x0600A1BB RID: 41403 RVA: 0x003C8AF8 File Offset: 0x003C6CF8
	public MapVisitor(Vector3i _worldPos1, Vector3i _worldPos2)
	{
		int x = _worldPos1.x;
		int z = _worldPos1.z;
		int x2 = _worldPos2.x;
		int z2 = _worldPos2.z;
		this.chunkPos1 = new Vector3i(World.toChunkXZ((x <= x2) ? x : x2), 0, World.toChunkXZ((z <= z2) ? z : z2));
		this.chunkPos2 = new Vector3i(World.toChunkXZ((x <= x2) ? x2 : x), 0, World.toChunkXZ((z <= z2) ? z2 : z));
	}

	// Token: 0x0600A1BC RID: 41404 RVA: 0x003C8B73 File Offset: 0x003C6D73
	public void Start()
	{
		if (!this.hasBeenStarted)
		{
			this.coroutine = ThreadManager.StartCoroutine(this.visitCo());
			this.hasBeenStarted = true;
		}
	}

	// Token: 0x0600A1BD RID: 41405 RVA: 0x003C8B95 File Offset: 0x003C6D95
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

	// Token: 0x0600A1BE RID: 41406 RVA: 0x003C8BD0 File Offset: 0x003C6DD0
	public bool IsRunning()
	{
		return this.coroutine != null;
	}

	// Token: 0x0600A1BF RID: 41407 RVA: 0x003C8BDB File Offset: 0x003C6DDB
	public bool HasBeenStarted()
	{
		return this.hasBeenStarted;
	}

	// Token: 0x0600A1C0 RID: 41408 RVA: 0x003C8BE3 File Offset: 0x003C6DE3
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

	// Token: 0x0600A1C1 RID: 41409 RVA: 0x003C8BF2 File Offset: 0x003C6DF2
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 chunkPosToBlockPos(int _x, int _z)
	{
		return new Vector3((float)this.chunkXZtoBlockXZ(_x), 0f, (float)this.chunkXZtoBlockXZ(_z));
	}

	// Token: 0x0600A1C2 RID: 41410 RVA: 0x003C8C0E File Offset: 0x003C6E0E
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkXZtoBlockXZ(int _xz)
	{
		if (_xz >= 0)
		{
			return _xz * 16;
		}
		return _xz * 16 + 1;
	}

	// Token: 0x0400793B RID: 31035
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine coroutine;

	// Token: 0x0400793C RID: 31036
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkManager.ChunkObserver observer;

	// Token: 0x0400793D RID: 31037
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Vector3i chunkPos1;

	// Token: 0x0400793E RID: 31038
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Vector3i chunkPos2;

	// Token: 0x0400793F RID: 31039
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasBeenStarted;

	// Token: 0x02001421 RID: 5153
	// (Invoke) Token: 0x0600A1C4 RID: 41412
	public delegate void VisitChunkDelegate(Chunk _chunk, int _chunksVisited, int _chunksTotal, float _elapsedSeconds);

	// Token: 0x02001422 RID: 5154
	// (Invoke) Token: 0x0600A1C8 RID: 41416
	public delegate void VisitMapDoneDelegate(int _chunks, float _elapsedSeconds);
}
