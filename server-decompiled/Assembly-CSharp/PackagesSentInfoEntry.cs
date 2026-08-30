using System;
using System.Collections.Generic;

// Token: 0x0200078E RID: 1934
public class PackagesSentInfoEntry
{
	// Token: 0x04002F0D RID: 12045
	public List<NetPackageInfo> packages;

	// Token: 0x04002F0E RID: 12046
	public int count;

	// Token: 0x04002F0F RID: 12047
	public long uncompressedSize;

	// Token: 0x04002F10 RID: 12048
	public long compressedSize;

	// Token: 0x04002F11 RID: 12049
	public bool bCompressed;

	// Token: 0x04002F12 RID: 12050
	public float timestamp;

	// Token: 0x04002F13 RID: 12051
	public string client;
}
