using System;

namespace UnityEngine.Pool
{
	// Token: 0x020003C1 RID: 961
	public class GenericPool<T> where T : class, new()
	{
		// Token: 0x060020E6 RID: 8422 RVA: 0x00036832 File Offset: 0x00034A32
		public static T Get()
		{
			return GenericPool<T>.s_Pool.Get();
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x0003683E File Offset: 0x00034A3E
		public static PooledObject<T> Get(out T value)
		{
			return GenericPool<T>.s_Pool.Get(out value);
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x0003684B File Offset: 0x00034A4B
		public static void Release(T toRelease)
		{
			GenericPool<T>.s_Pool.Release(toRelease);
		}

		// Token: 0x04000ADF RID: 2783
		internal static readonly ObjectPool<T> s_Pool = new ObjectPool<T>(() => new T(), null, null, null, true, 10, 10000);
	}
}
