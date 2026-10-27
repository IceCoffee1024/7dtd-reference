using System;

namespace Platform
{
	// Token: 0x02001C0A RID: 7178
	public struct CensoredTextResult
	{
		// Token: 0x17001B0B RID: 6923
		// (get) Token: 0x0600D5CF RID: 54735 RVA: 0x004CFDE6 File Offset: 0x004CDFE6
		public readonly bool Success { get; }

		// Token: 0x17001B0C RID: 6924
		// (get) Token: 0x0600D5D0 RID: 54736 RVA: 0x004CFDEE File Offset: 0x004CDFEE
		public readonly string OriginalText { get; }

		// Token: 0x17001B0D RID: 6925
		// (get) Token: 0x0600D5D1 RID: 54737 RVA: 0x004CFDF6 File Offset: 0x004CDFF6
		public readonly string CensoredText { get; }

		// Token: 0x0600D5D2 RID: 54738 RVA: 0x004CFDFE File Offset: 0x004CDFFE
		public CensoredTextResult(bool _success, string _originalText, string _censoredText)
		{
			this.Success = _success;
			this.OriginalText = _originalText;
			this.CensoredText = _censoredText;
		}
	}
}
