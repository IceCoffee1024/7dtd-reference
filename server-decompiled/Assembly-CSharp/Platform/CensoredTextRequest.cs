using System;

namespace Platform
{
	// Token: 0x02001BD8 RID: 7128
	public struct CensoredTextRequest
	{
		// Token: 0x17001AC0 RID: 6848
		// (get) Token: 0x0600D4B6 RID: 54454 RVA: 0x004CC281 File Offset: 0x004CA481
		public readonly string Input { get; }

		// Token: 0x17001AC1 RID: 6849
		// (get) Token: 0x0600D4B7 RID: 54455 RVA: 0x004CC289 File Offset: 0x004CA489
		// (set) Token: 0x0600D4B8 RID: 54456 RVA: 0x004CC291 File Offset: 0x004CA491
		public int CensoredLength { readonly get; set; }

		// Token: 0x17001AC2 RID: 6850
		// (get) Token: 0x0600D4B9 RID: 54457 RVA: 0x004CC29A File Offset: 0x004CA49A
		public readonly Action<CensoredTextResult> Callback { get; }

		// Token: 0x0600D4BA RID: 54458 RVA: 0x004CC2A2 File Offset: 0x004CA4A2
		public CensoredTextRequest(string _input, Action<CensoredTextResult> _callback)
		{
			this.Input = _input;
			this.CensoredLength = _input.Length;
			this.Callback = _callback;
		}
	}
}
