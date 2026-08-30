using System;

namespace Platform
{
	// Token: 0x02001B7F RID: 7039
	public struct CensoredTextRequest
	{
		// Token: 0x170019A8 RID: 6568
		// (get) Token: 0x0600D1B2 RID: 53682 RVA: 0x004C13A5 File Offset: 0x004BF5A5
		public readonly string Input { get; }

		// Token: 0x170019A9 RID: 6569
		// (get) Token: 0x0600D1B3 RID: 53683 RVA: 0x004C13AD File Offset: 0x004BF5AD
		// (set) Token: 0x0600D1B4 RID: 53684 RVA: 0x004C13B5 File Offset: 0x004BF5B5
		public int CensoredLength { readonly get; set; }

		// Token: 0x170019AA RID: 6570
		// (get) Token: 0x0600D1B5 RID: 53685 RVA: 0x004C13BE File Offset: 0x004BF5BE
		public readonly Action<CensoredTextResult> Callback { get; }

		// Token: 0x0600D1B6 RID: 53686 RVA: 0x004C13C6 File Offset: 0x004BF5C6
		public CensoredTextRequest(string _input, Action<CensoredTextResult> _callback)
		{
			this.Input = _input;
			this.CensoredLength = _input.Length;
			this.Callback = _callback;
		}
	}
}
