using System;
using UnityEngine;

// Token: 0x02000BAB RID: 2987
public class MovableSharedChunkObserver : IDisposable
{
	// Token: 0x06005A5B RID: 23131 RVA: 0x0022B134 File Offset: 0x00229334
	public MovableSharedChunkObserver(SharedChunkObserverCache _observerCache)
	{
		this.cache = _observerCache;
	}

	// Token: 0x06005A5C RID: 23132 RVA: 0x0022B144 File Offset: 0x00229344
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~MovableSharedChunkObserver()
	{
		this.Dispose();
	}

	// Token: 0x06005A5D RID: 23133 RVA: 0x0022B170 File Offset: 0x00229370
	public void SetPosition(Vector3 newPosition)
	{
		Vector2i vector2i = new Vector2i(World.toChunkXZ(Utils.Fastfloor(newPosition.x)), World.toChunkXZ(Utils.Fastfloor(newPosition.z)));
		if (this.observer == null || this.observer.ChunkPos != vector2i)
		{
			if (this.observer != null)
			{
				this.observer.Dispose();
			}
			this.observer = this.cache.GetSharedObserverForChunk(vector2i);
		}
	}

	// Token: 0x06005A5E RID: 23134 RVA: 0x0022B1E4 File Offset: 0x002293E4
	public void Dispose()
	{
		if (this.observer != null)
		{
			this.observer.Dispose();
			this.observer = null;
		}
	}

	// Token: 0x0400460F RID: 17935
	[PublicizedFrom(EAccessModifier.Private)]
	public ISharedChunkObserver observer;

	// Token: 0x04004610 RID: 17936
	[PublicizedFrom(EAccessModifier.Private)]
	public SharedChunkObserverCache cache;
}
