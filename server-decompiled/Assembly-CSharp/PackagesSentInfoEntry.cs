using System;
using System.Collections.Generic;

// Token: 0x0200079B RID: 1947
public class PackagesSentInfoEntry
{
	// Token: 0x04002F80 RID: 12160
	public List<NetPackageInfo> packages;

	// Token: 0x04002F81 RID: 12161
	public int count;

	// Token: 0x04002F82 RID: 12162
	public long uncompressedSize;

	// Token: 0x04002F83 RID: 12163
	public long compressedSize;

	// Token: 0x04002F84 RID: 12164
	public bool bCompressed;

	// Token: 0x04002F85 RID: 12165
	public float timestamp;

	// Token: 0x04002F86 RID: 12166
	public string client;
}
