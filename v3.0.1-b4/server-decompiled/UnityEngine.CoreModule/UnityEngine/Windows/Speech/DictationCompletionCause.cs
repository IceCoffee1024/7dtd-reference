using System;

namespace UnityEngine.Windows.Speech
{
	// Token: 0x020002D5 RID: 725
	public enum DictationCompletionCause
	{
		// Token: 0x04000A1E RID: 2590
		Complete,
		// Token: 0x04000A1F RID: 2591
		AudioQualityFailure,
		// Token: 0x04000A20 RID: 2592
		Canceled,
		// Token: 0x04000A21 RID: 2593
		TimeoutExceeded,
		// Token: 0x04000A22 RID: 2594
		PauseLimitExceeded,
		// Token: 0x04000A23 RID: 2595
		NetworkFailure,
		// Token: 0x04000A24 RID: 2596
		MicrophoneUnavailable,
		// Token: 0x04000A25 RID: 2597
		UnknownError
	}
}
