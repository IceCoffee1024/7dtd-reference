using System;

namespace Platform
{
	// Token: 0x02001B57 RID: 6999
	public interface IPlatformMemoryStat<T> : IPlatformMemoryStat
	{
		// Token: 0x14000122 RID: 290
		// (add) Token: 0x0600D0FF RID: 53503
		// (remove) Token: 0x0600D100 RID: 53504
		event PlatformMemoryColumnChangedHandler<T> ColumnSetAfter;

		// Token: 0x1700199D RID: 6557
		// (get) Token: 0x0600D101 RID: 53505
		// (set) Token: 0x0600D102 RID: 53506
		PlatformMemoryRenderValue<T> RenderValue { get; set; }

		// Token: 0x1700199E RID: 6558
		// (get) Token: 0x0600D103 RID: 53507
		// (set) Token: 0x0600D104 RID: 53508
		PlatformMemoryRenderDelta<T> RenderDelta { get; set; }

		// Token: 0x0600D105 RID: 53509
		void Set(MemoryStatColumn column, T value);

		// Token: 0x0600D106 RID: 53510
		bool TryGet(MemoryStatColumn column, out T value);

		// Token: 0x0600D107 RID: 53511
		bool TryGetLast(MemoryStatColumn column, out T value);
	}
}
