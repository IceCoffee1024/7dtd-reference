using System;

namespace UnityEngine.Pool
{
	// Token: 0x020003C5 RID: 965
	public class LinkedPool<T> : IDisposable, IPool, IObjectPool<T> where T : class
	{
		// Token: 0x060020F5 RID: 8437 RVA: 0x0003688C File Offset: 0x00034A8C
		public LinkedPool(Func<T> createFunc, Action<T> actionOnGet = null, Action<T> actionOnRelease = null, Action<T> actionOnDestroy = null, bool collectionCheck = true, int maxSize = 10000)
		{
			bool flag = createFunc == null;
			if (flag)
			{
				throw new ArgumentNullException("createFunc");
			}
			bool flag2 = maxSize <= 0;
			if (flag2)
			{
				throw new ArgumentException("maxSize", "Max size must be greater than 0");
			}
			this.m_CreateFunc = createFunc;
			this.m_ActionOnGet = actionOnGet;
			this.m_ActionOnRelease = actionOnRelease;
			this.m_ActionOnDestroy = actionOnDestroy;
			this.m_Limit = maxSize;
			this.m_CollectionCheck = collectionCheck;
			PoolManager.Register(this);
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x060020F6 RID: 8438 RVA: 0x00036904 File Offset: 0x00034B04
		// (set) Token: 0x060020F7 RID: 8439 RVA: 0x0003690C File Offset: 0x00034B0C
		public int CountInactive { get; private set; }

		// Token: 0x060020F8 RID: 8440 RVA: 0x00036918 File Offset: 0x00034B18
		public T Get()
		{
			T t = default(T);
			bool flag = this.m_PoolFirst == null;
			if (flag)
			{
				t = this.m_CreateFunc();
			}
			else
			{
				LinkedPool<T>.LinkedPoolItem poolFirst = this.m_PoolFirst;
				t = poolFirst.value;
				this.m_PoolFirst = poolFirst.poolNext;
				poolFirst.poolNext = this.m_NextAvailableListItem;
				this.m_NextAvailableListItem = poolFirst;
				this.m_NextAvailableListItem.value = default(T);
				int num = this.CountInactive - 1;
				this.CountInactive = num;
			}
			Action<T> actionOnGet = this.m_ActionOnGet;
			if (actionOnGet != null)
			{
				actionOnGet(t);
			}
			return t;
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x000369B8 File Offset: 0x00034BB8
		public PooledObject<T> Get(out T v)
		{
			return new PooledObject<T>(v = this.Get(), this);
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x000369DC File Offset: 0x00034BDC
		public void Release(T item)
		{
			Action<T> actionOnRelease = this.m_ActionOnRelease;
			if (actionOnRelease != null)
			{
				actionOnRelease(item);
			}
			bool flag = this.CountInactive < this.m_Limit;
			if (flag)
			{
				LinkedPool<T>.LinkedPoolItem linkedPoolItem = this.m_NextAvailableListItem;
				bool flag2 = linkedPoolItem == null;
				if (flag2)
				{
					linkedPoolItem = new LinkedPool<T>.LinkedPoolItem();
				}
				else
				{
					this.m_NextAvailableListItem = linkedPoolItem.poolNext;
				}
				linkedPoolItem.value = item;
				linkedPoolItem.poolNext = this.m_PoolFirst;
				this.m_PoolFirst = linkedPoolItem;
				int num = this.CountInactive + 1;
				this.CountInactive = num;
			}
			else
			{
				Action<T> actionOnDestroy = this.m_ActionOnDestroy;
				if (actionOnDestroy != null)
				{
					actionOnDestroy(item);
				}
			}
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x00036A7C File Offset: 0x00034C7C
		public void Clear()
		{
			bool flag = this.m_ActionOnDestroy != null;
			if (flag)
			{
				for (LinkedPool<T>.LinkedPoolItem linkedPoolItem = this.m_PoolFirst; linkedPoolItem != null; linkedPoolItem = linkedPoolItem.poolNext)
				{
					this.m_ActionOnDestroy(linkedPoolItem.value);
				}
			}
			this.m_PoolFirst = null;
			this.m_NextAvailableListItem = null;
			this.CountInactive = 0;
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x00036ADB File Offset: 0x00034CDB
		public void Dispose()
		{
			this.Clear();
		}

		// Token: 0x04000AE1 RID: 2785
		private readonly Func<T> m_CreateFunc;

		// Token: 0x04000AE2 RID: 2786
		private readonly Action<T> m_ActionOnGet;

		// Token: 0x04000AE3 RID: 2787
		private readonly Action<T> m_ActionOnRelease;

		// Token: 0x04000AE4 RID: 2788
		private readonly Action<T> m_ActionOnDestroy;

		// Token: 0x04000AE5 RID: 2789
		private readonly int m_Limit;

		// Token: 0x04000AE6 RID: 2790
		internal LinkedPool<T>.LinkedPoolItem m_PoolFirst;

		// Token: 0x04000AE7 RID: 2791
		internal LinkedPool<T>.LinkedPoolItem m_NextAvailableListItem;

		// Token: 0x04000AE8 RID: 2792
		private bool m_CollectionCheck;

		// Token: 0x020003C6 RID: 966
		internal class LinkedPoolItem
		{
			// Token: 0x04000AEA RID: 2794
			internal LinkedPool<T>.LinkedPoolItem poolNext;

			// Token: 0x04000AEB RID: 2795
			internal T value;
		}
	}
}
