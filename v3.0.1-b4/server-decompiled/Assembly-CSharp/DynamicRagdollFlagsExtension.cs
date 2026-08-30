using System;

// Token: 0x0200046D RID: 1133
public static class DynamicRagdollFlagsExtension
{
	// Token: 0x06002215 RID: 8725 RVA: 0x000CDD7B File Offset: 0x000CBF7B
	public static bool HasFlag(this DynamicRagdollFlags flag, DynamicRagdollFlags checkFlag)
	{
		return (flag & checkFlag) == checkFlag;
	}
}
