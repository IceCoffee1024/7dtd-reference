using System;
using UnityEngine;

// Token: 0x02000BBD RID: 3005
public class MovableSharedChunkObserver : IDisposable
{
	// Token: 0x06005A74 RID: 23156 RVA: 0x0022E430 File Offset: 0x0022C630
	public MovableSharedChunkObserver(SharedChunkObserverCache _observerCache)
	{
		this.cache = _observerCache;
	}

	// Token: 0x06005A75 RID: 23157 RVA: 0x0022E440 File Offset: 0x0022C640
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~MovableSharedChunkObserver()
	{
		this.Dispose();
	}

	// Token: 0x06005A76 RID: 23158 RVA: 0x0022E46C File Offset: 0x0022C66C
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

	// Token: 0x06005A77 RID: 23159 RVA: 0x0022E4E0 File Offset: 0x0022C6E0
	public void Dispose()
	{
		if (this.observer != null)
		{
			this.observer.Dispose();
			this.observer = null;
		}
	}

	// Token: 0x0400464E RID: 17998
	[PublicizedFrom(EAccessModifier.Private)]
	public ISharedChunkObserver observer;

	// Token: 0x0400464F RID: 17999
	[PublicizedFrom(EAccessModifier.Private)]
	public SharedChunkObserverCache cache;
}
