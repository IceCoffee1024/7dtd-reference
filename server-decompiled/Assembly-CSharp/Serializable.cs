using System;

// Token: 0x020012E7 RID: 4839
public interface Serializable
{
	// Token: 0x0600991F RID: 39199
	byte[] Serialize();

	// Token: 0x1700124F RID: 4687
	// (get) Token: 0x06009920 RID: 39200
	// (set) Token: 0x06009921 RID: 39201
	bool IsDirty { get; set; }
}
