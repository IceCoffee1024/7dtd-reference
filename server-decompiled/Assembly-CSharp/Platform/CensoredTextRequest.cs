using System;

namespace Platform
{
	// Token: 0x02001C09 RID: 7177
	public struct CensoredTextRequest
	{
		// Token: 0x17001B08 RID: 6920
		// (get) Token: 0x0600D5CA RID: 54730 RVA: 0x004CFDA9 File Offset: 0x004CDFA9
		public readonly string Input { get; }

		// Token: 0x17001B09 RID: 6921
		// (get) Token: 0x0600D5CB RID: 54731 RVA: 0x004CFDB1 File Offset: 0x004CDFB1
		// (set) Token: 0x0600D5CC RID: 54732 RVA: 0x004CFDB9 File Offset: 0x004CDFB9
		public int CensoredLength { readonly get; set; }

		// Token: 0x17001B0A RID: 6922
		// (get) Token: 0x0600D5CD RID: 54733 RVA: 0x004CFDC2 File Offset: 0x004CDFC2
		public readonly Action<CensoredTextResult> Callback { get; }

		// Token: 0x0600D5CE RID: 54734 RVA: 0x004CFDCA File Offset: 0x004CDFCA
		public CensoredTextRequest(string _input, Action<CensoredTextResult> _callback)
		{
			this.Input = _input;
			this.CensoredLength = _input.Length;
			this.Callback = _callback;
		}
	}
}
