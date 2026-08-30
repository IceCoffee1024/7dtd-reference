using System;
using UnityEngine;

// Token: 0x020001D7 RID: 471
public interface EntityUINotification
{
	// Token: 0x1700011C RID: 284
	// (get) Token: 0x06000E89 RID: 3721
	BuffValue Buff { get; }

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06000E8A RID: 3722
	string Icon { get; }

	// Token: 0x06000E8B RID: 3723
	Color GetColor();

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x06000E8C RID: 3724
	float CurrentValue { get; }

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x06000E8D RID: 3725
	string Units { get; }

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x06000E8E RID: 3726
	EnumEntityUINotificationDisplayMode DisplayMode { get; }

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x06000E8F RID: 3727
	EnumEntityUINotificationSubject Subject { get; }
}
