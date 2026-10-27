using System;

namespace Platform
{
	// Token: 0x02001BE1 RID: 7137
	public interface IPlatformMemoryStat<T> : IPlatformMemoryStat
	{
		// Token: 0x14000122 RID: 290
		// (add) Token: 0x0600D516 RID: 54550
		// (remove) Token: 0x0600D517 RID: 54551
		event PlatformMemoryColumnChangedHandler<T> ColumnSetAfter;

		// Token: 0x17001AFD RID: 6909
		// (get) Token: 0x0600D518 RID: 54552
		// (set) Token: 0x0600D519 RID: 54553
		PlatformMemoryRenderValue<T> RenderValue { get; set; }

		// Token: 0x17001AFE RID: 6910
		// (get) Token: 0x0600D51A RID: 54554
		// (set) Token: 0x0600D51B RID: 54555
		PlatformMemoryRenderDelta<T> RenderDelta { get; set; }

		// Token: 0x0600D51C RID: 54556
		void Set(MemoryStatColumn column, T value);

		// Token: 0x0600D51D RID: 54557
		bool TryGet(MemoryStatColumn column, out T value);

		// Token: 0x0600D51E RID: 54558
		bool TryGetLast(MemoryStatColumn column, out T value);
	}
}
