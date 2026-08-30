using System;

// Token: 0x020012C7 RID: 4807
public interface Serializable
{
	// Token: 0x06009873 RID: 39027
	byte[] Serialize();

	// Token: 0x1700120A RID: 4618
	// (get) Token: 0x06009874 RID: 39028
	// (set) Token: 0x06009875 RID: 39029
	bool IsDirty { get; set; }
}
