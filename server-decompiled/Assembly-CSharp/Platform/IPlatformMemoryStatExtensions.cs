using System;

namespace Platform
{
	// Token: 0x02001BE3 RID: 7139
	public static class IPlatformMemoryStatExtensions
	{
		// Token: 0x0600D523 RID: 54563 RVA: 0x004CF154 File Offset: 0x004CD354
		public static IPlatformMemoryStat<T> AddColumnSetHandler<T>(this IPlatformMemoryStat<T> stat, PlatformMemoryColumnChangedHandler<T> handler)
		{
			stat.ColumnSetAfter += handler;
			return stat;
		}

		// Token: 0x0600D524 RID: 54564 RVA: 0x004CF160 File Offset: 0x004CD360
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

		// Token: 0x0600D525 RID: 54565 RVA: 0x004CF194 File Offset: 0x004CD394
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

		// Token: 0x0600D526 RID: 54566 RVA: 0x004CF1C5 File Offset: 0x004CD3C5
		public static bool TryGetCurrentAndLast<T>(this IPlatformMemoryStat<T> stat, MemoryStatColumn column, out T current, out T last)
		{
			if (!stat.TryGet(column, out current))
			{
				last = default(T);
				return false;
			}
			return stat.TryGetLast(column, out last);
		}

		// Token: 0x0600D527 RID: 54567 RVA: 0x004CF1E4 File Offset: 0x004CD3E4
		public static bool HasColumnChanged<T>(this IPlatformMemoryStat<T> stat, MemoryStatColumn column, PlatformMemoryStatHasChangedSignificantly<T> checkCurrentVsLast)
		{
			T t;
			T t2;
			return stat.TryGetCurrentAndLast<T>(column, out t, out t2) && checkCurrentVsLast(t, t2);
		}

		// Token: 0x0600D528 RID: 54568 RVA: 0x004CF208 File Offset: 0x004CD408
		public static bool HasColumnIncreased<T>(this IPlatformMemoryStat<T> stat, MemoryStatColumn column) where T : IComparable<T>
		{
			return stat.HasColumnChanged<T>(column, (T current, T last) => current.CompareTo(last) > 0);
		}

		// Token: 0x0600D529 RID: 54569 RVA: 0x004CF230 File Offset: 0x004CD430
		public static bool HasColumnDecreased<T>(this IPlatformMemoryStat<T> stat, MemoryStatColumn column) where T : IComparable<T>
		{
			return stat.HasColumnChanged<T>(column, (T current, T last) => current.CompareTo(last) < 0);
		}

		// Token: 0x0600D52A RID: 54570 RVA: 0x004CF258 File Offset: 0x004CD458
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
