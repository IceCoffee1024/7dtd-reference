using System;

namespace Platform
{
	// Token: 0x02001BD9 RID: 7129
	public struct CensoredTextResult
	{
		// Token: 0x17001AC3 RID: 6851
		// (get) Token: 0x0600D4BB RID: 54459 RVA: 0x004CC2BE File Offset: 0x004CA4BE
		public readonly bool Success { get; }

		// Token: 0x17001AC4 RID: 6852
		// (get) Token: 0x0600D4BC RID: 54460 RVA: 0x004CC2C6 File Offset: 0x004CA4C6
		public readonly string OriginalText { get; }

		// Token: 0x17001AC5 RID: 6853
		// (get) Token: 0x0600D4BD RID: 54461 RVA: 0x004CC2CE File Offset: 0x004CA4CE
		public readonly string CensoredText { get; }

		// Token: 0x0600D4BE RID: 54462 RVA: 0x004CC2D6 File Offset: 0x004CA4D6
		public CensoredTextResult(bool _success, string _originalText, string _censoredText)
		{
			this.Success = _success;
			this.OriginalText = _originalText;
			this.CensoredText = _censoredText;
		}
	}
}
