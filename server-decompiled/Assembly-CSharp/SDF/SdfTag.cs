using System;
using System.IO;

namespace SDF
{
	// Token: 0x020016C6 RID: 5830
	public abstract class SdfTag
	{
		// Token: 0x170016A7 RID: 5799
		// (get) Token: 0x0600B6CA RID: 46794 RVA: 0x0043CEAD File Offset: 0x0043B0AD
		// (set) Token: 0x0600B6CB RID: 46795 RVA: 0x0043CEB5 File Offset: 0x0043B0B5
		public SdfTagType TagType { get; set; }

		// Token: 0x170016A8 RID: 5800
		// (get) Token: 0x0600B6CC RID: 46796 RVA: 0x0043CEBE File Offset: 0x0043B0BE
		// (set) Token: 0x0600B6CD RID: 46797 RVA: 0x0043CEC6 File Offset: 0x0043B0C6
		public string Name { get; set; }

		// Token: 0x170016A9 RID: 5801
		// (get) Token: 0x0600B6CE RID: 46798 RVA: 0x0043CECF File Offset: 0x0043B0CF
		// (set) Token: 0x0600B6CF RID: 46799 RVA: 0x0043CED7 File Offset: 0x0043B0D7
		public object Value { get; set; }

		// Token: 0x0600B6D0 RID: 46800
		public abstract void WritePayload(BinaryWriter bw);

		// Token: 0x0600B6D1 RID: 46801 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public SdfTag()
		{
		}
	}
}
