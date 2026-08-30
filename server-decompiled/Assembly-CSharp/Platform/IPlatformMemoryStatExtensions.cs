using System;

namespace Platform
{
	// Token: 0x02001B80 RID: 7040
	public static class IPlatformMemoryStatExtensions
	{
		// Token: 0x0600D297 RID: 53911 RVA: 0x004C6B44 File Offset: 0x004C4D44
		public static IPlatformMemoryStat<T> AddColumnSetHandler<T>(this IPlatformMemoryStat<T> stat, PlatformMemoryColumnChangedHandler<T> handler)
		{
			stat.ColumnSetAfter += handler;
			return stat;
		}

		// Token: 0x0600D298 RID: 53912 RVA: 0x004C6B50 File Offset: 0x004C4D50
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

		// Token: 0x0600D299 RID: 53913 RVA: 0x004C6B84 File Offset: 0x004C4D84
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

		// Token: 0x0600D29A RID: 53914 RVA: 0x004C6BB5 File Offset: 0x004C4DB5
		public static bool TryGetCurrentAndLast<T>(this IPlatformMemoryStat<T> stat, MemoryStatColumn column, out T current, out T last)
		{
			if (!stat.TryGet(column, out current))
			{
				last = default(T);
				return false;
			}
			return stat.TryGetLast(column, out last);
		}

		// Token: 0x0600D29B RID: 53915 RVA: 0x004C6BD4 File Offset: 0x004C4DD4
		public static bool HasColumnChanged<T>(this IPlatformMemoryStat<T> stat, MemoryStatColumn column, PlatformMemoryStatHasChangedSignificantly<T> checkCurrentVsLast)
		{
			T t;
			T t2;
			return stat.TryGetCurrentAndLast<T>(column, out t, out t2) && checkCurrentVsLast(t, t2);
		}

		// Token: 0x0600D29C RID: 53916 RVA: 0x004C6BF8 File Offset: 0x004C4DF8
		public static bool HasColumnIncreased<T>(this IPlatformMemoryStat<T> stat, MemoryStatColumn column) where T : IComparable<T>
		{
			return stat.HasColumnChanged<T>(column, (T current, T last) => current.CompareTo(last) > 0);
		}

		// Token: 0x0600D29D RID: 53917 RVA: 0x004C6C20 File Offset: 0x004C4E20
		public static bool HasColumnDecreased<T>(this IPlatformMemoryStat<T> stat, MemoryStatColumn column) where T : IComparable<T>
		{
			return stat.HasColumnChanged<T>(column, (T current, T last) => current.CompareTo(last) < 0);
		}

		// Token: 0x0600D29E RID: 53918 RVA: 0x004C6C48 File Offset: 0x004C4E48
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
