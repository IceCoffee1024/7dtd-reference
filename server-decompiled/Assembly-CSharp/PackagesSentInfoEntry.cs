using System;
using System.Collections.Generic;

// Token: 0x02000798 RID: 1944
public class PackagesSentInfoEntry
{
	// Token: 0x04002F86 RID: 12166
	public List<NetPackageInfo> packages;

	// Token: 0x04002F87 RID: 12167
	public int count;

	// Token: 0x04002F88 RID: 12168
	public long uncompressedSize;

	// Token: 0x04002F89 RID: 12169
	public long compressedSize;

	// Token: 0x04002F8A RID: 12170
	public bool bCompressed;

	// Token: 0x04002F8B RID: 12171
	public float timestamp;

	// Token: 0x04002F8C RID: 12172
	public string client;
}
