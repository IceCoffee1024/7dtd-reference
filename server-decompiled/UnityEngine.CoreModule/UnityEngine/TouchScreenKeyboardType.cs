using System;

namespace UnityEngine
{
	// Token: 0x02000295 RID: 661
	public enum TouchScreenKeyboardType
	{
		// Token: 0x04000963 RID: 2403
		Default,
		// Token: 0x04000964 RID: 2404
		ASCIICapable,
		// Token: 0x04000965 RID: 2405
		NumbersAndPunctuation,
		// Token: 0x04000966 RID: 2406
		URL,
		// Token: 0x04000967 RID: 2407
		NumberPad,
		// Token: 0x04000968 RID: 2408
		PhonePad,
		// Token: 0x04000969 RID: 2409
		NamePhonePad,
		// Token: 0x0400096A RID: 2410
		EmailAddress,
		// Token: 0x0400096B RID: 2411
		[Obsolete("Wii U is no longer supported as of Unity 2018.1.")]
		NintendoNetworkAccount,
		// Token: 0x0400096C RID: 2412
		Social,
		// Token: 0x0400096D RID: 2413
		Search,
		// Token: 0x0400096E RID: 2414
		DecimalPad,
		// Token: 0x0400096F RID: 2415
		OneTimeCode
	}
}
