using System;
using System.Collections.Generic;

namespace UnityEngine.Pool
{
	// Token: 0x020003C7 RID: 967
	public class ObjectPool<T> : IDisposable, IPool, IObjectPool<T> where T : class
	{
		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x060020FE RID: 8446 RVA: 0x00036AE5 File Offset: 0x00034CE5
		// (set) Token: 0x060020FF RID: 8447 RVA: 0x00036AED File Offset: 0x00034CED
		public int CountAll { get; private set; }

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06002100 RID: 8448 RVA: 0x00036AF8 File Offset: 0x00034CF8
		public int CountActive
		{
			get
			{
				return this.CountAll - this.CountInactive;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06002101 RID: 8449 RVA: 0x00036B18 File Offset: 0x00034D18
		public int CountInactive
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x00036B38 File Offset: 0x00034D38
		public ObjectPool(Func<T> createFunc, Action<T> actionOnGet = null, Action<T> actionOnRelease = null, Action<T> actionOnDestroy = null, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
		{
			bool flag = createFunc == null;
			if (flag)
			{
				throw new ArgumentNullException("createFunc");
			}
			bool flag2 = maxSize <= 0;
			if (flag2)
			{
				throw new ArgumentException("Max Size must be greater than 0", "maxSize");
			}
			this.m_List = new List<T>(defaultCapacity);
			this.m_CreateFunc = createFunc;
			this.m_MaxSize = maxSize;
			this.m_ActionOnGet = actionOnGet;
			this.m_ActionOnRelease = actionOnRelease;
			this.m_ActionOnDestroy = actionOnDestroy;
			this.m_CollectionCheck = collectionCheck;
			PoolManager.Register(this);
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x00036BC0 File Offset: 0x00034DC0
		public T Get()
		{
			bool flag = this.m_List.Count == 0;
			T t;
			if (flag)
			{
				t = this.m_CreateFunc();
				int countAll = this.CountAll;
				this.CountAll = countAll + 1;
			}
			else
			{
				int num = this.m_List.Count - 1;
				t = this.m_List[num];
				this.m_List.RemoveAt(num);
			}
			Action<T> actionOnGet = this.m_ActionOnGet;
			if (actionOnGet != null)
			{
				actionOnGet(t);
			}
			return t;
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x00036C48 File Offset: 0x00034E48
		public PooledObject<T> Get(out T v)
		{
			return new PooledObject<T>(v = this.Get(), this);
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x00036C6C File Offset: 0x00034E6C
		public void Release(T element)
		{
			Action<T> actionOnRelease = this.m_ActionOnRelease;
			if (actionOnRelease != null)
			{
				actionOnRelease(element);
			}
			bool flag = this.CountInactive < this.m_MaxSize;
			if (flag)
			{
				this.m_List.Add(element);
			}
			else
			{
				int countAll = this.CountAll;
				this.CountAll = countAll - 1;
				Action<T> actionOnDestroy = this.m_ActionOnDestroy;
				if (actionOnDestroy != null)
				{
					actionOnDestroy(element);
				}
			}
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x00036CD8 File Offset: 0x00034ED8
		public void Clear()
		{
			bool flag = this.m_ActionOnDestroy != null;
			if (flag)
			{
				foreach (T t in this.m_List)
				{
					this.m_ActionOnDestroy(t);
				}
			}
			this.m_List.Clear();
			this.CountAll = 0;
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x00036D5C File Offset: 0x00034F5C
		public void Dispose()
		{
			this.Clear();
		}

		// Token: 0x04000AEC RID: 2796
		internal readonly List<T> m_List;

		// Token: 0x04000AED RID: 2797
		private readonly Func<T> m_CreateFunc;

		// Token: 0x04000AEE RID: 2798
		private readonly Action<T> m_ActionOnGet;

		// Token: 0x04000AEF RID: 2799
		private readonly Action<T> m_ActionOnRelease;

		// Token: 0x04000AF0 RID: 2800
		private readonly Action<T> m_ActionOnDestroy;

		// Token: 0x04000AF1 RID: 2801
		private readonly int m_MaxSize;

		// Token: 0x04000AF2 RID: 2802
		internal bool m_CollectionCheck;
	}
}
