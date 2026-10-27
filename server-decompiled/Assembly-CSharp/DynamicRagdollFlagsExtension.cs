using System;

// Token: 0x02000475 RID: 1141
public static class DynamicRagdollFlagsExtension
{
	// Token: 0x06002226 RID: 8742 RVA: 0x000CE977 File Offset: 0x000CCB77
	public static bool HasFlag(this DynamicRagdollFlags flag, DynamicRagdollFlags checkFlag)
	{
		return (flag & checkFlag) == checkFlag;
	}
}
