using System;
using System.IO;

namespace SDF
{
	// Token: 0x02001674 RID: 5748
	public abstract class SdfTag
	{
		// Token: 0x17001593 RID: 5523
		// (get) Token: 0x0600B3F0 RID: 46064 RVA: 0x00432C15 File Offset: 0x00430E15
		// (set) Token: 0x0600B3F1 RID: 46065 RVA: 0x00432C1D File Offset: 0x00430E1D
		public SdfTagType TagType { get; set; }

		// Token: 0x17001594 RID: 5524
		// (get) Token: 0x0600B3F2 RID: 46066 RVA: 0x00432C26 File Offset: 0x00430E26
		// (set) Token: 0x0600B3F3 RID: 46067 RVA: 0x00432C2E File Offset: 0x00430E2E
		public string Name { get; set; }

		// Token: 0x17001595 RID: 5525
		// (get) Token: 0x0600B3F4 RID: 46068 RVA: 0x00432C37 File Offset: 0x00430E37
		// (set) Token: 0x0600B3F5 RID: 46069 RVA: 0x00432C3F File Offset: 0x00430E3F
		public object Value { get; set; }

		// Token: 0x0600B3F6 RID: 46070
		public abstract void WritePayload(BinaryWriter bw);

		// Token: 0x0600B3F7 RID: 46071 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public SdfTag()
		{
		}
	}
}
