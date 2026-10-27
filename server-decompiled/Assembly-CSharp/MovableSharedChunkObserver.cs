using System;
using UnityEngine;

// Token: 0x02000BC8 RID: 3016
public class MovableSharedChunkObserver : IDisposable
{
	// Token: 0x06005A99 RID: 23193 RVA: 0x0022F98C File Offset: 0x0022DB8C
	public MovableSharedChunkObserver(SharedChunkObserverCache _observerCache)
	{
		this.cache = _observerCache;
	}

	// Token: 0x06005A9A RID: 23194 RVA: 0x0022F99C File Offset: 0x0022DB9C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~MovableSharedChunkObserver()
	{
		this.Dispose();
	}

	// Token: 0x06005A9B RID: 23195 RVA: 0x0022F9C8 File Offset: 0x0022DBC8
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

	// Token: 0x06005A9C RID: 23196 RVA: 0x0022FA3C File Offset: 0x0022DC3C
	public void Dispose()
	{
		if (this.observer != null)
		{
			this.observer.Dispose();
			this.observer = null;
		}
	}

	// Token: 0x04004673 RID: 18035
	[PublicizedFrom(EAccessModifier.Private)]
	public ISharedChunkObserver observer;

	// Token: 0x04004674 RID: 18036
	[PublicizedFrom(EAccessModifier.Private)]
	public SharedChunkObserverCache cache;
}
