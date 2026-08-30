using System;

namespace Platform
{
	// Token: 0x02001B7E RID: 7038
	public interface IPlatformMemoryStat<T> : IPlatformMemoryStat
	{
		// Token: 0x14000123 RID: 291
		// (add) Token: 0x0600D28A RID: 53898
		// (remove) Token: 0x0600D28B RID: 53899
		event PlatformMemoryColumnChangedHandler<T> ColumnSetAfter;

		// Token: 0x17001A0F RID: 6671
		// (get) Token: 0x0600D28C RID: 53900
		// (set) Token: 0x0600D28D RID: 53901
		PlatformMemoryRenderValue<T> RenderValue { get; set; }

		// Token: 0x17001A10 RID: 6672
		// (get) Token: 0x0600D28E RID: 53902
		// (set) Token: 0x0600D28F RID: 53903
		PlatformMemoryRenderDelta<T> RenderDelta { get; set; }

		// Token: 0x0600D290 RID: 53904
		void Set(MemoryStatColumn column, T value);

		// Token: 0x0600D291 RID: 53905
		bool TryGet(MemoryStatColumn column, out T value);

		// Token: 0x0600D292 RID: 53906
		bool TryGetLast(MemoryStatColumn column, out T value);
	}
}
