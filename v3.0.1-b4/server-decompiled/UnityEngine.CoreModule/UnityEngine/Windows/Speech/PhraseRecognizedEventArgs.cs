using System;

namespace UnityEngine.Windows.Speech
{
	// Token: 0x020002D7 RID: 727
	public struct PhraseRecognizedEventArgs
	{
		// Token: 0x06001EBD RID: 7869 RVA: 0x00032802 File Offset: 0x00030A02
		internal PhraseRecognizedEventArgs(string text, ConfidenceLevel confidence, SemanticMeaning[] semanticMeanings, DateTime phraseStartTime, TimeSpan phraseDuration)
		{
			this.text = text;
			this.confidence = confidence;
			this.semanticMeanings = semanticMeanings;
			this.phraseStartTime = phraseStartTime;
			this.phraseDuration = phraseDuration;
		}

		// Token: 0x04000A28 RID: 2600
		public readonly ConfidenceLevel confidence;

		// Token: 0x04000A29 RID: 2601
		public readonly SemanticMeaning[] semanticMeanings;

		// Token: 0x04000A2A RID: 2602
		public readonly string text;

		// Token: 0x04000A2B RID: 2603
		public readonly DateTime phraseStartTime;

		// Token: 0x04000A2C RID: 2604
		public readonly TimeSpan phraseDuration;
	}
}
