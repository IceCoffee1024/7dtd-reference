using System;
using System.IO;

namespace SDF
{
	// Token: 0x020016E5 RID: 5861
	public abstract class SdfTag
	{
		// Token: 0x170016EE RID: 5870
		// (get) Token: 0x0600B79E RID: 47006 RVA: 0x0043F951 File Offset: 0x0043DB51
		// (set) Token: 0x0600B79F RID: 47007 RVA: 0x0043F959 File Offset: 0x0043DB59
		public SdfTagType TagType { get; set; }

		// Token: 0x170016EF RID: 5871
		// (get) Token: 0x0600B7A0 RID: 47008 RVA: 0x0043F962 File Offset: 0x0043DB62
		// (set) Token: 0x0600B7A1 RID: 47009 RVA: 0x0043F96A File Offset: 0x0043DB6A
		public string Name { get; set; }

		// Token: 0x170016F0 RID: 5872
		// (get) Token: 0x0600B7A2 RID: 47010 RVA: 0x0043F973 File Offset: 0x0043DB73
		// (set) Token: 0x0600B7A3 RID: 47011 RVA: 0x0043F97B File Offset: 0x0043DB7B
		public object Value { get; set; }

		// Token: 0x0600B7A4 RID: 47012
		public abstract void WritePayload(BinaryWriter bw);

		// Token: 0x0600B7A5 RID: 47013 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public SdfTag()
		{
		}
	}
}
