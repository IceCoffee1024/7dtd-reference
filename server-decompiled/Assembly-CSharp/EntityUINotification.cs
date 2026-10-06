using System;
using UnityEngine;

// Token: 0x020001DA RID: 474
public interface EntityUINotification
{
	// Token: 0x17000114 RID: 276
	// (get) Token: 0x06000E87 RID: 3719
	BuffValue Buff { get; }

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x06000E88 RID: 3720
	string Icon { get; }

	// Token: 0x06000E89 RID: 3721
	Color GetColor();

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x06000E8A RID: 3722
	float CurrentValue { get; }

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06000E8B RID: 3723
	string Units { get; }

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x06000E8C RID: 3724
	EnumEntityUINotificationDisplayMode DisplayMode { get; }

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x06000E8D RID: 3725
	EnumEntityUINotificationSubject Subject { get; }
}
