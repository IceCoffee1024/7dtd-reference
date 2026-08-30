using System;

namespace UnityEngine.Windows.Speech
{
	// Token: 0x020002D3 RID: 723
	public enum SpeechError
	{
		// Token: 0x04000A0F RID: 2575
		NoError,
		// Token: 0x04000A10 RID: 2576
		TopicLanguageNotSupported,
		// Token: 0x04000A11 RID: 2577
		GrammarLanguageMismatch,
		// Token: 0x04000A12 RID: 2578
		GrammarCompilationFailure,
		// Token: 0x04000A13 RID: 2579
		AudioQualityFailure,
		// Token: 0x04000A14 RID: 2580
		PauseLimitExceeded,
		// Token: 0x04000A15 RID: 2581
		TimeoutExceeded,
		// Token: 0x04000A16 RID: 2582
		NetworkFailure,
		// Token: 0x04000A17 RID: 2583
		MicrophoneUnavailable,
		// Token: 0x04000A18 RID: 2584
		UnknownError
	}
}
