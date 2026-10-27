using System;

// Token: 0x020012F9 RID: 4857
public interface Serializable
{
	// Token: 0x06009977 RID: 39287
	byte[] Serialize();

	// Token: 0x1700125B RID: 4699
	// (get) Token: 0x06009978 RID: 39288
	// (set) Token: 0x06009979 RID: 39289
	bool IsDirty { get; set; }
}
