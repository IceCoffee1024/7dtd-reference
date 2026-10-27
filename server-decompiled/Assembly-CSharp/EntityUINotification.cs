using System;
using UnityEngine;

// Token: 0x020001DC RID: 476
public interface EntityUINotification
{
	// Token: 0x17000114 RID: 276
	// (get) Token: 0x06000E8B RID: 3723
	BuffValue Buff { get; }

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x06000E8C RID: 3724
	string Icon { get; }

	// Token: 0x06000E8D RID: 3725
	Color GetColor();

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x06000E8E RID: 3726
	float CurrentValue { get; }

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06000E8F RID: 3727
	string Units { get; }

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x06000E90 RID: 3728
	EnumEntityUINotificationDisplayMode DisplayMode { get; }

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x06000E91 RID: 3729
	EnumEntityUINotificationSubject Subject { get; }
}
