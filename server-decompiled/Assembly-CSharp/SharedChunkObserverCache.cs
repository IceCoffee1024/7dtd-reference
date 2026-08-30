using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B9A RID: 2970
public class SharedChunkObserverCache
{
	// Token: 0x17000963 RID: 2403
	// (get) Token: 0x060059D7 RID: 22999 RVA: 0x002284B0 File Offset: 0x002266B0
	public IThreadingSemantics ThreadingSemantics
	{
		get
		{
			return this.threadingSemantics;
		}
	}

	// Token: 0x060059D8 RID: 23000 RVA: 0x002284B8 File Offset: 0x002266B8
	public SharedChunkObserverCache(ChunkManager _chunkManager, int _viewDim, IThreadingSemantics _threadingSemantics)
	{
		this.chunkManager = _chunkManager;
		this.viewDim = _viewDim;
		this.threadingSemantics = _threadingSemantics;
	}

	// Token: 0x060059D9 RID: 23001 RVA: 0x002284E0 File Offset: 0x002266E0
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

	// Token: 0x060059DA RID: 23002 RVA: 0x00228518 File Offset: 0x00226718
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

	// Token: 0x04004576 RID: 17782
	[PublicizedFrom(EAccessModifier.Private)]
	public IThreadingSemantics threadingSemantics;

	// Token: 0x04004577 RID: 17783
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Vector2i, SharedChunkObserverCache.SharedChunkObserver> observers = new Dictionary<Vector2i, SharedChunkObserverCache.SharedChunkObserver>();

	// Token: 0x04004578 RID: 17784
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkManager chunkManager;

	// Token: 0x04004579 RID: 17785
	[PublicizedFrom(EAccessModifier.Private)]
	public int viewDim;

	// Token: 0x02000B9B RID: 2971
	[PublicizedFrom(EAccessModifier.Private)]
	public class SharedChunkObserver : ISharedChunkObserver, IDisposable
	{
		// Token: 0x060059DB RID: 23003 RVA: 0x00228566 File Offset: 0x00226766
		public SharedChunkObserver(SharedChunkObserverCache _cache, ChunkManager.ChunkObserver _chunkRef, SharedChunkObserverCache.SharedChunkObserver.RemoveObserver _removeObserverDelegate, Vector2i _chunkPos)
		{
			this.cache = _cache;
			this.chunkRef = _chunkRef;
			this.removeObserver = _removeObserverDelegate;
			this.chunkPos = _chunkPos;
			this.refCount = 1;
		}

		// Token: 0x060059DC RID: 23004 RVA: 0x00228592 File Offset: 0x00226792
		public void Reference()
		{
			if (this.cache.ThreadingSemantics.InterlockedAdd(ref this.refCount, 1) < 2)
			{
				throw new Exception("Synchronization error: shared chunk observer was already disposed with a ref count of zero!");
			}
		}

		// Token: 0x060059DD RID: 23005 RVA: 0x002285B9 File Offset: 0x002267B9
		public void Dispose()
		{
			if (this.cache.ThreadingSemantics.InterlockedAdd(ref this.refCount, -1) == 0)
			{
				this.removeObserver(this);
			}
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x060059DE RID: 23006 RVA: 0x002285E0 File Offset: 0x002267E0
		public Vector2i ChunkPos
		{
			get
			{
				return this.chunkPos;
			}
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x060059DF RID: 23007 RVA: 0x002285E8 File Offset: 0x002267E8
		public SharedChunkObserverCache Owner
		{
			get
			{
				return this.cache;
			}
		}

		// Token: 0x0400457A RID: 17786
		public Vector2i chunkPos;

		// Token: 0x0400457B RID: 17787
		public int refCount;

		// Token: 0x0400457C RID: 17788
		public ChunkManager.ChunkObserver chunkRef;

		// Token: 0x0400457D RID: 17789
		[PublicizedFrom(EAccessModifier.Private)]
		public SharedChunkObserverCache cache;

		// Token: 0x0400457E RID: 17790
		[PublicizedFrom(EAccessModifier.Private)]
		public SharedChunkObserverCache.SharedChunkObserver.RemoveObserver removeObserver;

		// Token: 0x02000B9C RID: 2972
		// (Invoke) Token: 0x060059E1 RID: 23009
		public delegate void RemoveObserver(SharedChunkObserverCache.SharedChunkObserver observer);
	}
}
