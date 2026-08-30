using System;
using System.Collections.Generic;

namespace UnityEngine.Pool
{
	// Token: 0x020003C9 RID: 969
	internal static class PoolManager
	{
		// Token: 0x0600210A RID: 8458 RVA: 0x00036D8C File Offset: 0x00034F8C
		public static void Reset()
		{
			for (int i = PoolManager.s_WeakPoolReferences.Count - 1; i >= 0; i--)
			{
				IPool pool;
				bool flag = PoolManager.s_WeakPoolReferences[i].TryGetTarget(out pool);
				if (flag)
				{
					pool.Clear();
				}
				else
				{
					PoolManager.s_WeakPoolReferences.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x00036DE9 File Offset: 0x00034FE9
		public static void Register(IPool pool)
		{
			PoolManager.s_WeakPoolReferences.Add(new WeakReference<IPool>(pool));
		}

		// Token: 0x04000AF6 RID: 2806
		private static readonly List<WeakReference<IPool>> s_WeakPoolReferences = new List<WeakReference<IPool>>();
	}
}
