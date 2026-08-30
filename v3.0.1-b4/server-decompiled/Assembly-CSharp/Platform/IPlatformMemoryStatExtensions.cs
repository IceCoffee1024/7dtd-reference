using System;

namespace Platform
{
	// Token: 0x02001B59 RID: 7001
	public static class IPlatformMemoryStatExtensions
	{
		// Token: 0x0600D10C RID: 53516 RVA: 0x004C0750 File Offset: 0x004BE950
		public static IPlatformMemoryStat<T> AddColumnSetHandler<T>(this IPlatformMemoryStat<T> stat, PlatformMemoryColumnChangedHandler<T> handler)
		{
			stat.ColumnSetAfter += handler;
			return stat;
		}

		// Token: 0x0600D10D RID: 53517 RVA: 0x004C075C File Offset: 0x004BE95C
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

		// Token: 0x0600D10E RID: 53518 RVA: 0x004C0790 File Offset: 0x004BE990
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

		// Token: 0x0600D10F RID: 53519 RVA: 0x004C07C1 File Offset: 0x004BE9C1
		public static bool TryGetCurrentAndLast<T>(this IPlatformMemoryStat<T> stat, MemoryStatColumn column, out T current, out T last)
		{
			if (!stat.TryGet(column, out current))
			{
				last = default(T);
				return false;
			}
			return stat.TryGetLast(column, out last);
		}

		// Token: 0x0600D110 RID: 53520 RVA: 0x004C07E0 File Offset: 0x004BE9E0
		public static bool HasColumnChanged<T>(this IPlatformMemoryStat<T> stat, MemoryStatColumn column, PlatformMemoryStatHasChangedSignificantly<T> checkCurrentVsLast)
		{
			T t;
			T t2;
			return stat.TryGetCurrentAndLast<T>(column, out t, out t2) && checkCurrentVsLast(t, t2);
		}

		// Token: 0x0600D111 RID: 53521 RVA: 0x004C0804 File Offset: 0x004BEA04
		public static bool HasColumnIncreased<T>(this IPlatformMemoryStat<T> stat, MemoryStatColumn column) where T : IComparable<T>
		{
			return stat.HasColumnChanged<T>(column, (T current, T last) => current.CompareTo(last) > 0);
		}

		// Token: 0x0600D112 RID: 53522 RVA: 0x004C082C File Offset: 0x004BEA2C
		public static bool HasColumnDecreased<T>(this IPlatformMemoryStat<T> stat, MemoryStatColumn column) where T : IComparable<T>
		{
			return stat.HasColumnChanged<T>(column, (T current, T last) => current.CompareTo(last) < 0);
		}

		// Token: 0x0600D113 RID: 53523 RVA: 0x004C0854 File Offset: 0x004BEA54
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
