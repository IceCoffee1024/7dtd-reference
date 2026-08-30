using System;

namespace UnityEngine.Pool
{
	// Token: 0x020003C8 RID: 968
	public struct PooledObject<T> : IDisposable where T : class
	{
		// Token: 0x06002108 RID: 8456 RVA: 0x00036D66 File Offset: 0x00034F66
		public PooledObject(T value, IObjectPool<T> pool)
		{
			this.m_ToReturn = value;
			this.m_Pool = pool;
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x00036D77 File Offset: 0x00034F77
		void IDisposable.Dispose()
		{
			this.m_Pool.Release(this.m_ToReturn);
		}

		// Token: 0x04000AF4 RID: 2804
		private readonly T m_ToReturn;

		// Token: 0x04000AF5 RID: 2805
		private readonly IObjectPool<T> m_Pool;
	}
}
