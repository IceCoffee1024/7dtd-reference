using System;

namespace Platform
{
	// Token: 0x02001B80 RID: 7040
	public struct CensoredTextResult
	{
		// Token: 0x170019AB RID: 6571
		// (get) Token: 0x0600D1B7 RID: 53687 RVA: 0x004C13E2 File Offset: 0x004BF5E2
		public readonly bool Success { get; }

		// Token: 0x170019AC RID: 6572
		// (get) Token: 0x0600D1B8 RID: 53688 RVA: 0x004C13EA File Offset: 0x004BF5EA
		public readonly string OriginalText { get; }

		// Token: 0x170019AD RID: 6573
		// (get) Token: 0x0600D1B9 RID: 53689 RVA: 0x004C13F2 File Offset: 0x004BF5F2
		public readonly string CensoredText { get; }

		// Token: 0x0600D1BA RID: 53690 RVA: 0x004C13FA File Offset: 0x004BF5FA
		public CensoredTextResult(bool _success, string _originalText, string _censoredText)
		{
			this.Success = _success;
			this.OriginalText = _originalText;
			this.CensoredText = _censoredText;
		}
	}
}
