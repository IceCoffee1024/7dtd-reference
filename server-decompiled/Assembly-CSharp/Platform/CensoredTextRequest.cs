using System;

namespace Platform
{
	// Token: 0x02001BA6 RID: 7078
	public struct CensoredTextRequest
	{
		// Token: 0x17001A1A RID: 6682
		// (get) Token: 0x0600D33E RID: 54078 RVA: 0x004C7799 File Offset: 0x004C5999
		public readonly string Input { get; }

		// Token: 0x17001A1B RID: 6683
		// (get) Token: 0x0600D33F RID: 54079 RVA: 0x004C77A1 File Offset: 0x004C59A1
		// (set) Token: 0x0600D340 RID: 54080 RVA: 0x004C77A9 File Offset: 0x004C59A9
		public int CensoredLength { readonly get; set; }

		// Token: 0x17001A1C RID: 6684
		// (get) Token: 0x0600D341 RID: 54081 RVA: 0x004C77B2 File Offset: 0x004C59B2
		public readonly Action<CensoredTextResult> Callback { get; }

		// Token: 0x0600D342 RID: 54082 RVA: 0x004C77BA File Offset: 0x004C59BA
		public CensoredTextRequest(string _input, Action<CensoredTextResult> _callback)
		{
			this.Input = _input;
			this.CensoredLength = _input.Length;
			this.Callback = _callback;
		}
	}
}
