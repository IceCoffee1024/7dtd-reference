using System;

namespace UnityEngine.Search
{
	// Token: 0x02000310 RID: 784
	[Flags]
	public enum SearchViewFlags
	{
		// Token: 0x04000A8D RID: 2701
		None = 0,
		// Token: 0x04000A8E RID: 2702
		Debug = 16,
		// Token: 0x04000A8F RID: 2703
		NoIndexing = 32,
		// Token: 0x04000A90 RID: 2704
		Packages = 256,
		// Token: 0x04000A91 RID: 2705
		OpenLeftSidePanel = 2048,
		// Token: 0x04000A92 RID: 2706
		OpenInspectorPreview = 4096,
		// Token: 0x04000A93 RID: 2707
		Centered = 8192,
		// Token: 0x04000A94 RID: 2708
		HideSearchBar = 16384,
		// Token: 0x04000A95 RID: 2709
		CompactView = 32768,
		// Token: 0x04000A96 RID: 2710
		ListView = 65536,
		// Token: 0x04000A97 RID: 2711
		GridView = 131072,
		// Token: 0x04000A98 RID: 2712
		TableView = 262144,
		// Token: 0x04000A99 RID: 2713
		EnableSearchQuery = 524288,
		// Token: 0x04000A9A RID: 2714
		DisableInspectorPreview = 1048576,
		// Token: 0x04000A9B RID: 2715
		DisableSavedSearchQuery = 2097152,
		// Token: 0x04000A9C RID: 2716
		OpenInBuilderMode = 4194304,
		// Token: 0x04000A9D RID: 2717
		OpenInTextMode = 8388608,
		// Token: 0x04000A9E RID: 2718
		DisableBuilderModeToggle = 16777216,
		// Token: 0x04000A9F RID: 2719
		Borderless = 33554432,
		// Token: 0x04000AA0 RID: 2720
		DisableQueryHelpers = 67108864,
		// Token: 0x04000AA1 RID: 2721
		DisableNoResultTips = 134217728,
		// Token: 0x04000AA2 RID: 2722
		IgnoreSavedSearches = 268435456,
		// Token: 0x04000AA3 RID: 2723
		ObjectPicker = 536870912,
		// Token: 0x04000AA4 RID: 2724
		ObjectPickerAdvancedUI = 1073741824,
		// Token: 0x04000AA5 RID: 2725
		ContextSwitchPreservedMask = 33560576
	}
}
