using System;

// Token: 0x02000106 RID: 262
[Flags]
public enum EBlockRotationClasses
{
	// Token: 0x04000708 RID: 1800
	None = 0,
	// Token: 0x04000709 RID: 1801
	Basic90 = 1,
	// Token: 0x0400070A RID: 1802
	Headfirst = 2,
	// Token: 0x0400070B RID: 1803
	Sideways = 4,
	// Token: 0x0400070C RID: 1804
	Basic45 = 8,
	// Token: 0x0400070D RID: 1805
	Basic90And45 = 9,
	// Token: 0x0400070E RID: 1806
	No45 = 7,
	// Token: 0x0400070F RID: 1807
	Advanced = 6,
	// Token: 0x04000710 RID: 1808
	All = 15
}
