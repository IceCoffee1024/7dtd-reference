using System;

namespace UnityEngine.Pool
{
	// Token: 0x020003C3 RID: 963
	public interface IObjectPool<T> where T : class
	{
		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x060020EE RID: 8430
		int CountInactive { get; }

		// Token: 0x060020EF RID: 8431
		T Get();

		// Token: 0x060020F0 RID: 8432
		PooledObject<T> Get(out T v);

		// Token: 0x060020F1 RID: 8433
		void Release(T element);

		// Token: 0x060020F2 RID: 8434
		void Clear();
	}
}
