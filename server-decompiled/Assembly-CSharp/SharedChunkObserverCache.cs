using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BBE RID: 3006
public class SharedChunkObserverCache
{
	// Token: 0x17000997 RID: 2455
	// (get) Token: 0x06005A78 RID: 23160 RVA: 0x0022E4FC File Offset: 0x0022C6FC
	public IThreadingSemantics ThreadingSemantics
	{
		get
		{
			return this.threadingSemantics;
		}
	}

	// Token: 0x06005A79 RID: 23161 RVA: 0x0022E504 File Offset: 0x0022C704
	public SharedChunkObserverCache(ChunkManager _chunkManager, int _viewDim, IThreadingSemantics _threadingSemantics)
	{
		this.chunkManager = _chunkManager;
		this.viewDim = _viewDim;
		this.threadingSemantics = _threadingSemantics;
	}

	// Token: 0x06005A7A RID: 23162 RVA: 0x0022E52C File Offset: 0x0022C72C
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

	// Token: 0x06005A7B RID: 23163 RVA: 0x0022E564 File Offset: 0x0022C764
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

	// Token: 0x04004650 RID: 18000
	[PublicizedFrom(EAccessModifier.Private)]
	public IThreadingSemantics threadingSemantics;

	// Token: 0x04004651 RID: 18001
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Vector2i, SharedChunkObserverCache.SharedChunkObserver> observers = new Dictionary<Vector2i, SharedChunkObserverCache.SharedChunkObserver>();

	// Token: 0x04004652 RID: 18002
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkManager chunkManager;

	// Token: 0x04004653 RID: 18003
	[PublicizedFrom(EAccessModifier.Private)]
	public int viewDim;

	// Token: 0x02000BBF RID: 3007
	[PublicizedFrom(EAccessModifier.Private)]
	public class SharedChunkObserver : ISharedChunkObserver, IDisposable
	{
		// Token: 0x06005A7C RID: 23164 RVA: 0x0022E5B2 File Offset: 0x0022C7B2
		public SharedChunkObserver(SharedChunkObserverCache _cache, ChunkManager.ChunkObserver _chunkRef, SharedChunkObserverCache.SharedChunkObserver.RemoveObserver _removeObserverDelegate, Vector2i _chunkPos)
		{
			this.cache = _cache;
			this.chunkRef = _chunkRef;
			this.removeObserver = _removeObserverDelegate;
			this.chunkPos = _chunkPos;
			this.refCount = 1;
		}

		// Token: 0x06005A7D RID: 23165 RVA: 0x0022E5DE File Offset: 0x0022C7DE
		public void Reference()
		{
			if (this.cache.ThreadingSemantics.InterlockedAdd(ref this.refCount, 1) < 2)
			{
				throw new Exception("Synchronization error: shared chunk observer was already disposed with a ref count of zero!");
			}
		}

		// Token: 0x06005A7E RID: 23166 RVA: 0x0022E605 File Offset: 0x0022C805
		public void Dispose()
		{
			if (this.cache.ThreadingSemantics.InterlockedAdd(ref this.refCount, -1) == 0)
			{
				this.removeObserver(this);
			}
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06005A7F RID: 23167 RVA: 0x0022E62C File Offset: 0x0022C82C
		public Vector2i ChunkPos
		{
			get
			{
				return this.chunkPos;
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06005A80 RID: 23168 RVA: 0x0022E634 File Offset: 0x0022C834
		public SharedChunkObserverCache Owner
		{
			get
			{
				return this.cache;
			}
		}

		// Token: 0x04004654 RID: 18004
		public Vector2i chunkPos;

		// Token: 0x04004655 RID: 18005
		public int refCount;

		// Token: 0x04004656 RID: 18006
		public ChunkManager.ChunkObserver chunkRef;

		// Token: 0x04004657 RID: 18007
		[PublicizedFrom(EAccessModifier.Private)]
		public SharedChunkObserverCache cache;

		// Token: 0x04004658 RID: 18008
		[PublicizedFrom(EAccessModifier.Private)]
		public SharedChunkObserverCache.SharedChunkObserver.RemoveObserver removeObserver;

		// Token: 0x02000BC0 RID: 3008
		// (Invoke) Token: 0x06005A82 RID: 23170
		public delegate void RemoveObserver(SharedChunkObserverCache.SharedChunkObserver observer);
	}
}
