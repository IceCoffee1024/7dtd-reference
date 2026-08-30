using System;
using System.Collections.Generic;

namespace UnityEngine.Pool
{
	// Token: 0x020003BC RID: 956
	public class CollectionPool<TCollection, TItem> where TCollection : class, ICollection<TItem>, new()
	{
		// Token: 0x060020DA RID: 8410 RVA: 0x00036799 File Offset: 0x00034999
		public static TCollection Get()
		{
			return CollectionPool<TCollection, TItem>.s_Pool.Get();
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x000367A5 File Offset: 0x000349A5
		public static PooledObject<TCollection> Get(out TCollection value)
		{
			return CollectionPool<TCollection, TItem>.s_Pool.Get(out value);
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x000367B2 File Offset: 0x000349B2
		public static void Release(TCollection toRelease)
		{
			CollectionPool<TCollection, TItem>.s_Pool.Release(toRelease);
		}

		// Token: 0x04000ADD RID: 2781
		internal static readonly ObjectPool<TCollection> s_Pool = new ObjectPool<TCollection>(() => new TCollection(), null, delegate(TCollection l)
		{
			l.Clear();
		}, null, true, 10, 10000);
	}
}
