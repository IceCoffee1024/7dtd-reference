using System;
using UnityEngine;

// Token: 0x02000B99 RID: 2969
public class MovableSharedChunkObserver : IDisposable
{
	// Token: 0x060059D3 RID: 22995 RVA: 0x002283E4 File Offset: 0x002265E4
	public MovableSharedChunkObserver(SharedChunkObserverCache _observerCache)
	{
		this.cache = _observerCache;
	}

	// Token: 0x060059D4 RID: 22996 RVA: 0x002283F4 File Offset: 0x002265F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~MovableSharedChunkObserver()
	{
		this.Dispose();
	}

	// Token: 0x060059D5 RID: 22997 RVA: 0x00228420 File Offset: 0x00226620
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

	// Token: 0x060059D6 RID: 22998 RVA: 0x00228494 File Offset: 0x00226694
	public void Dispose()
	{
		if (this.observer != null)
		{
			this.observer.Dispose();
			this.observer = null;
		}
	}

	// Token: 0x04004574 RID: 17780
	[PublicizedFrom(EAccessModifier.Private)]
	public ISharedChunkObserver observer;

	// Token: 0x04004575 RID: 17781
	[PublicizedFrom(EAccessModifier.Private)]
	public SharedChunkObserverCache cache;
}
