using System;

namespace Platform
{
	// Token: 0x02001BB0 RID: 7088
	public interface IPlatformMemoryStat<T> : IPlatformMemoryStat
	{
		// Token: 0x14000122 RID: 290
		// (add) Token: 0x0600D402 RID: 54274
		// (remove) Token: 0x0600D403 RID: 54275
		event PlatformMemoryColumnChangedHandler<T> ColumnSetAfter;

		// Token: 0x17001AB5 RID: 6837
		// (get) Token: 0x0600D404 RID: 54276
		// (set) Token: 0x0600D405 RID: 54277
		PlatformMemoryRenderValue<T> RenderValue { get; set; }

		// Token: 0x17001AB6 RID: 6838
		// (get) Token: 0x0600D406 RID: 54278
		// (set) Token: 0x0600D407 RID: 54279
		PlatformMemoryRenderDelta<T> RenderDelta { get; set; }

		// Token: 0x0600D408 RID: 54280
		void Set(MemoryStatColumn column, T value);

		// Token: 0x0600D409 RID: 54281
		bool TryGet(MemoryStatColumn column, out T value);

		// Token: 0x0600D40A RID: 54282
		bool TryGetLast(MemoryStatColumn column, out T value);
	}
}
