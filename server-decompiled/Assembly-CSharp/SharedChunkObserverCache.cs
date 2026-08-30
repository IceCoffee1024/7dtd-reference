using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BAC RID: 2988
public class SharedChunkObserverCache
{
	// Token: 0x17000975 RID: 2421
	// (get) Token: 0x06005A5F RID: 23135 RVA: 0x0022B200 File Offset: 0x00229400
	public IThreadingSemantics ThreadingSemantics
	{
		get
		{
			return this.threadingSemantics;
		}
	}

	// Token: 0x06005A60 RID: 23136 RVA: 0x0022B208 File Offset: 0x00229408
	public SharedChunkObserverCache(ChunkManager _chunkManager, int _viewDim, IThreadingSemantics _threadingSemantics)
	{
		this.chunkManager = _chunkManager;
		this.viewDim = _viewDim;
		this.threadingSemantics = _threadingSemantics;
	}

	// Token: 0x06005A61 RID: 23137 RVA: 0x0022B230 File Offset: 0x00229430
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

	// Token: 0x06005A62 RID: 23138 RVA: 0x0022B268 File Offset: 0x00229468
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

	// Token: 0x04004611 RID: 17937
	[PublicizedFrom(EAccessModifier.Private)]
	public IThreadingSemantics threadingSemantics;

	// Token: 0x04004612 RID: 17938
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Vector2i, SharedChunkObserverCache.SharedChunkObserver> observers = new Dictionary<Vector2i, SharedChunkObserverCache.SharedChunkObserver>();

	// Token: 0x04004613 RID: 17939
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkManager chunkManager;

	// Token: 0x04004614 RID: 17940
	[PublicizedFrom(EAccessModifier.Private)]
	public int viewDim;

	// Token: 0x02000BAD RID: 2989
	[PublicizedFrom(EAccessModifier.Private)]
	public class SharedChunkObserver : ISharedChunkObserver, IDisposable
	{
		// Token: 0x06005A63 RID: 23139 RVA: 0x0022B2B6 File Offset: 0x002294B6
		public SharedChunkObserver(SharedChunkObserverCache _cache, ChunkManager.ChunkObserver _chunkRef, SharedChunkObserverCache.SharedChunkObserver.RemoveObserver _removeObserverDelegate, Vector2i _chunkPos)
		{
			this.cache = _cache;
			this.chunkRef = _chunkRef;
			this.removeObserver = _removeObserverDelegate;
			this.chunkPos = _chunkPos;
			this.refCount = 1;
		}

		// Token: 0x06005A64 RID: 23140 RVA: 0x0022B2E2 File Offset: 0x002294E2
		public void Reference()
		{
			if (this.cache.ThreadingSemantics.InterlockedAdd(ref this.refCount, 1) < 2)
			{
				throw new Exception("Synchronization error: shared chunk observer was already disposed with a ref count of zero!");
			}
		}

		// Token: 0x06005A65 RID: 23141 RVA: 0x0022B309 File Offset: 0x00229509
		public void Dispose()
		{
			if (this.cache.ThreadingSemantics.InterlockedAdd(ref this.refCount, -1) == 0)
			{
				this.removeObserver(this);
			}
		}

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06005A66 RID: 23142 RVA: 0x0022B330 File Offset: 0x00229530
		public Vector2i ChunkPos
		{
			get
			{
				return this.chunkPos;
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06005A67 RID: 23143 RVA: 0x0022B338 File Offset: 0x00229538
		public SharedChunkObserverCache Owner
		{
			get
			{
				return this.cache;
			}
		}

		// Token: 0x04004615 RID: 17941
		public Vector2i chunkPos;

		// Token: 0x04004616 RID: 17942
		public int refCount;

		// Token: 0x04004617 RID: 17943
		public ChunkManager.ChunkObserver chunkRef;

		// Token: 0x04004618 RID: 17944
		[PublicizedFrom(EAccessModifier.Private)]
		public SharedChunkObserverCache cache;

		// Token: 0x04004619 RID: 17945
		[PublicizedFrom(EAccessModifier.Private)]
		public SharedChunkObserverCache.SharedChunkObserver.RemoveObserver removeObserver;

		// Token: 0x02000BAE RID: 2990
		// (Invoke) Token: 0x06005A69 RID: 23145
		public delegate void RemoveObserver(SharedChunkObserverCache.SharedChunkObserver observer);
	}
}
