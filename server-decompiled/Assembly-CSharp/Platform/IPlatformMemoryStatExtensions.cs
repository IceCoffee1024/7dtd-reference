using System;

namespace Platform
{
	// Token: 0x02001BB2 RID: 7090
	public static class IPlatformMemoryStatExtensions
	{
		// Token: 0x0600D40F RID: 54287 RVA: 0x004CB62C File Offset: 0x004C982C
		public static IPlatformMemoryStat<T> AddColumnSetHandler<T>(this IPlatformMemoryStat<T> stat, PlatformMemoryColumnChangedHandler<T> handler)
		{
			stat.ColumnSetAfter += handler;
			return stat;
		}

		// Token: 0x0600D410 RID: 54288 RVA: 0x004CB638 File Offset: 0x004C9838
		public static IPlatformMemoryStat<T> WithUpdatePeak<T>(this IPlatformMemoryStat<T> stat) where T : IComparable<T>
		{
			return stat.AddColumnSetHandler<T>(delegate(MemoryStatColumn column, T value)
			{
				if (column != MemoryStatColumn.Current)
				{
					return;
				}
				T t;
				if (!stat.TryGet(MemoryStatColumn.Peak, out t) || value.CompareTo(t) > 0)
				{
					stat.Set(MemoryStatColumn.Peak, value);
				}
			});
		}

		// Token: 0x0600D411 RID: 54289 RVA: 0x004CB66C File Offset: 0x004C986C
		public static IPlatformMemoryStat<T> WithUpdateMin<T>(this IPlatformMemoryStat<T> stat) where T : IComparable<T>
		{
			return stat.AddColumnSetHandler<T>(delegate(MemoryStatColumn column, T value)
			{
				if (column != MemoryStatColumn.Current)
				{
					return;
				}
				T t;
				if (!stat.TryGet(MemoryStatColumn.Min, out t) || value.CompareTo(t) < 0)
				{
					stat.Set(MemoryStatColumn.Min, value);
				}
			});
		}

		// Token: 0x0600D412 RID: 54290 RVA: 0x004CB69D File Offset: 0x004C989D
		public static bool TryGetCurrentAndLast<T>(this IPlatformMemoryStat<T> stat, MemoryStatColumn column, out T current, out T last)
		{
			if (!stat.TryGet(column, out current))
			{
				last = default(T);
				return false;
			}
			return stat.TryGetLast(column, out last);
		}

		// Token: 0x0600D413 RID: 54291 RVA: 0x004CB6BC File Offset: 0x004C98BC
		public static bool HasColumnChanged<T>(this IPlatformMemoryStat<T> stat, MemoryStatColumn column, PlatformMemoryStatHasChangedSignificantly<T> checkCurrentVsLast)
		{
			T t;
			T t2;
			return stat.TryGetCurrentAndLast<T>(column, out t, out t2) && checkCurrentVsLast(t, t2);
		}

		// Token: 0x0600D414 RID: 54292 RVA: 0x004CB6E0 File Offset: 0x004C98E0
		public static bool HasColumnIncreased<T>(this IPlatformMemoryStat<T> stat, MemoryStatColumn column) where T : IComparable<T>
		{
			return stat.HasColumnChanged<T>(column, (T current, T last) => current.CompareTo(last) > 0);
		}

		// Token: 0x0600D415 RID: 54293 RVA: 0x004CB708 File Offset: 0x004C9908
		public static bool HasColumnDecreased<T>(this IPlatformMemoryStat<T> stat, MemoryStatColumn column) where T : IComparable<T>
		{
			return stat.HasColumnChanged<T>(column, (T current, T last) => current.CompareTo(last) < 0);
		}

		// Token: 0x0600D416 RID: 54294 RVA: 0x004CB730 File Offset: 0x004C9930
		public static bool HasBytesChangedSignificantly(this IPlatformMemoryStat<long> stat, MemoryStatColumn column)
		{
			return stat.HasColumnChanged<long>(column, delegate(long current, long last)
			{
				long num = Math.Abs(current - last);
				long num2;
				long num3;
				if (stat.TryGet(MemoryStatColumn.Limit, out num2) && last > num2 / 2L)
				{
					num3 = Math.Abs(num2 - last);
				}
				else
				{
					num3 = Math.Abs(last);
				}
				return num >= num3 / 128L;
			});
		}
	}
}
