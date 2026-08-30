using System;

namespace UnityEngine.Pool
{
	// Token: 0x020003CA RID: 970
	public static class UnsafeGenericPool<T> where T : class, new()
	{
		// Token: 0x0600210D RID: 8461 RVA: 0x00036E09 File Offset: 0x00035009
		public static T Get()
		{
			return UnsafeGenericPool<T>.s_Pool.Get();
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x00036E15 File Offset: 0x00035015
		public static PooledObject<T> Get(out T value)
		{
			return UnsafeGenericPool<T>.s_Pool.Get(out value);
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x00036E22 File Offset: 0x00035022
		public static void Release(T toRelease)
		{
			UnsafeGenericPool<T>.s_Pool.Release(toRelease);
		}

		// Token: 0x04000AF7 RID: 2807
		internal static readonly ObjectPool<T> s_Pool = new ObjectPool<T>(() => new T(), null, null, null, false, 10, 10000);
	}
}
