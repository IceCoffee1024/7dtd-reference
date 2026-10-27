using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BC9 RID: 3017
public class SharedChunkObserverCache
{
	// Token: 0x1700099D RID: 2461
	// (get) Token: 0x06005A9D RID: 23197 RVA: 0x0022FA58 File Offset: 0x0022DC58
	public IThreadingSemantics ThreadingSemantics
	{
		get
		{
			return this.threadingSemantics;
		}
	}

	// Token: 0x06005A9E RID: 23198 RVA: 0x0022FA60 File Offset: 0x0022DC60
	public SharedChunkObserverCache(ChunkManager _chunkManager, int _viewDim, IThreadingSemantics _threadingSemantics)
	{
		this.chunkManager = _chunkManager;
		this.viewDim = _viewDim;
		this.threadingSemantics = _threadingSemantics;
	}

	// Token: 0x06005A9F RID: 23199 RVA: 0x0022FA88 File Offset: 0x0022DC88
	public ISharedChunkObserver GetSharedObserverForChunk(Vector2i chunkPos)
	{
		return this.threadingSemantics.Synchronize<SharedChunkObserverCache.SharedChunkObserver>(delegate
		{
			SharedChunkObserverCache.SharedChunkObserver sharedChunkObserver;
			if (this.observers.TryGetValue(chunkPos, out sharedChunkObserver) && sharedChunkObserver.refCount < 1)
			{
				sharedChunkObserver = null;
			}
			if (sharedChunkObserver != null)
			{
				sharedChunkObserver.Reference();
			}
			else
			{
				sharedChunkObserver = new SharedChunkObserverCache.SharedChunkObserver(this, this.chunkManager.AddChunkObserver(new Vector3((float)(chunkPos.x << 4), 0f, (float)(chunkPos.y << 4)), false, this.viewDim, -1), new SharedChunkObserverCache.SharedChunkObserver.RemoveObserver(this.removeChunkObserver), chunkPos);
				this.observers[chunkPos] = sharedChunkObserver;
			}
			return sharedChunkObserver;
		});
	}

	// Token: 0x06005AA0 RID: 23200 RVA: 0x0022FAC0 File Offset: 0x0022DCC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void removeChunkObserver(SharedChunkObserverCache.SharedChunkObserver observer)
	{
		this.threadingSemantics.Synchronize(delegate
		{
			if (this.observers[observer.chunkPos] == observer)
			{
				this.observers.Remove(observer.chunkPos);
			}
		});
		this.chunkManager.RemoveChunkObserver(observer.chunkRef);
	}

	// Token: 0x04004675 RID: 18037
	[PublicizedFrom(EAccessModifier.Private)]
	public IThreadingSemantics threadingSemantics;

	// Token: 0x04004676 RID: 18038
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Vector2i, SharedChunkObserverCache.SharedChunkObserver> observers = new Dictionary<Vector2i, SharedChunkObserverCache.SharedChunkObserver>();

	// Token: 0x04004677 RID: 18039
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkManager chunkManager;

	// Token: 0x04004678 RID: 18040
	[PublicizedFrom(EAccessModifier.Private)]
	public int viewDim;

	// Token: 0x02000BCA RID: 3018
	[PublicizedFrom(EAccessModifier.Private)]
	public class SharedChunkObserver : ISharedChunkObserver, IDisposable
	{
		// Token: 0x06005AA1 RID: 23201 RVA: 0x0022FB0E File Offset: 0x0022DD0E
		public SharedChunkObserver(SharedChunkObserverCache _cache, ChunkManager.ChunkObserver _chunkRef, SharedChunkObserverCache.SharedChunkObserver.RemoveObserver _removeObserverDelegate, Vector2i _chunkPos)
		{
			this.cache = _cache;
			this.chunkRef = _chunkRef;
			this.removeObserver = _removeObserverDelegate;
			this.chunkPos = _chunkPos;
			this.refCount = 1;
		}

		// Token: 0x06005AA2 RID: 23202 RVA: 0x0022FB3A File Offset: 0x0022DD3A
		public void Reference()
		{
			if (this.cache.ThreadingSemantics.InterlockedAdd(ref this.refCount, 1) < 2)
			{
				throw new Exception("Synchronization error: shared chunk observer was already disposed with a ref count of zero!");
			}
		}

		// Token: 0x06005AA3 RID: 23203 RVA: 0x0022FB61 File Offset: 0x0022DD61
		public void Dispose()
		{
			if (this.cache.ThreadingSemantics.InterlockedAdd(ref this.refCount, -1) == 0)
			{
				this.removeObserver(this);
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06005AA4 RID: 23204 RVA: 0x0022FB88 File Offset: 0x0022DD88
		public Vector2i ChunkPos
		{
			get
			{
				return this.chunkPos;
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06005AA5 RID: 23205 RVA: 0x0022FB90 File Offset: 0x0022DD90
		public SharedChunkObserverCache Owner
		{
			get
			{
				return this.cache;
			}
		}

		// Token: 0x04004679 RID: 18041
		public Vector2i chunkPos;

		// Token: 0x0400467A RID: 18042
		public int refCount;

		// Token: 0x0400467B RID: 18043
		public ChunkManager.ChunkObserver chunkRef;

		// Token: 0x0400467C RID: 18044
		[PublicizedFrom(EAccessModifier.Private)]
		public SharedChunkObserverCache cache;

		// Token: 0x0400467D RID: 18045
		[PublicizedFrom(EAccessModifier.Private)]
		public SharedChunkObserverCache.SharedChunkObserver.RemoveObserver removeObserver;

		// Token: 0x02000BCB RID: 3019
		// (Invoke) Token: 0x06005AA7 RID: 23207
		public delegate void RemoveObserver(SharedChunkObserverCache.SharedChunkObserver observer);
	}
}
