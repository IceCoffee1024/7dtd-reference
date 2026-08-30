using System;

// Token: 0x02000103 RID: 259
[Flags]
public enum EBlockRotationClasses
{
	// Token: 0x04000703 RID: 1795
	None = 0,
	// Token: 0x04000704 RID: 1796
	Basic90 = 1,
	// Token: 0x04000705 RID: 1797
	Headfirst = 2,
	// Token: 0x04000706 RID: 1798
	Sideways = 4,
	// Token: 0x04000707 RID: 1799
	Basic45 = 8,
	// Token: 0x04000708 RID: 1800
	Basic90And45 = 9,
	// Token: 0x04000709 RID: 1801
	No45 = 7,
	// Token: 0x0400070A RID: 1802
	Advanced = 6,
	// Token: 0x0400070B RID: 1803
	All = 15
}
