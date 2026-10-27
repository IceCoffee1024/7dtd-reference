using System;
using System.Collections.Generic;

// Token: 0x020007A3 RID: 1955
public class PackagesSentInfoEntry
{
	// Token: 0x04002F9C RID: 12188
	public List<NetPackageInfo> packages;

	// Token: 0x04002F9D RID: 12189
	public int count;

	// Token: 0x04002F9E RID: 12190
	public long uncompressedSize;

	// Token: 0x04002F9F RID: 12191
	public long compressedSize;

	// Token: 0x04002FA0 RID: 12192
	public bool bCompressed;

	// Token: 0x04002FA1 RID: 12193
	public float timestamp;

	// Token: 0x04002FA2 RID: 12194
	public string client;
}
