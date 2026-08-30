using System;

namespace Platform
{
	// Token: 0x02001BA7 RID: 7079
	public struct CensoredTextResult
	{
		// Token: 0x17001A1D RID: 6685
		// (get) Token: 0x0600D343 RID: 54083 RVA: 0x004C77D6 File Offset: 0x004C59D6
		public readonly bool Success { get; }

		// Token: 0x17001A1E RID: 6686
		// (get) Token: 0x0600D344 RID: 54084 RVA: 0x004C77DE File Offset: 0x004C59DE
		public readonly string OriginalText { get; }

		// Token: 0x17001A1F RID: 6687
		// (get) Token: 0x0600D345 RID: 54085 RVA: 0x004C77E6 File Offset: 0x004C59E6
		public readonly string CensoredText { get; }

		// Token: 0x0600D346 RID: 54086 RVA: 0x004C77EE File Offset: 0x004C59EE
		public CensoredTextResult(bool _success, string _originalText, string _censoredText)
		{
			this.Success = _success;
			this.OriginalText = _originalText;
			this.CensoredText = _censoredText;
		}
	}
}
