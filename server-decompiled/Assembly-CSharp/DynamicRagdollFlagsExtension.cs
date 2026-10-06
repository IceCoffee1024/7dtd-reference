using System;

// Token: 0x02000473 RID: 1139
public static class DynamicRagdollFlagsExtension
{
	// Token: 0x06002221 RID: 8737 RVA: 0x000CE5CB File Offset: 0x000CC7CB
	public static bool HasFlag(this DynamicRagdollFlags flag, DynamicRagdollFlags checkFlag)
	{
		return (flag & checkFlag) == checkFlag;
	}
}
