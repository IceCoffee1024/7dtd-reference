using System;

// Token: 0x020012AD RID: 4781
public interface Serializable
{
	// Token: 0x06009778 RID: 38776
	byte[] Serialize();

	// Token: 0x170011D1 RID: 4561
	// (get) Token: 0x06009779 RID: 38777
	// (set) Token: 0x0600977A RID: 38778
	bool IsDirty { get; set; }
}
