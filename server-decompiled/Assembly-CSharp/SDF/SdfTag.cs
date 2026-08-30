using System;
using System.IO;

namespace SDF
{
	// Token: 0x0200169A RID: 5786
	public abstract class SdfTag
	{
		// Token: 0x17001605 RID: 5637
		// (get) Token: 0x0600B573 RID: 46451 RVA: 0x0043898D File Offset: 0x00436B8D
		// (set) Token: 0x0600B574 RID: 46452 RVA: 0x00438995 File Offset: 0x00436B95
		public SdfTagType TagType { get; set; }

		// Token: 0x17001606 RID: 5638
		// (get) Token: 0x0600B575 RID: 46453 RVA: 0x0043899E File Offset: 0x00436B9E
		// (set) Token: 0x0600B576 RID: 46454 RVA: 0x004389A6 File Offset: 0x00436BA6
		public string Name { get; set; }

		// Token: 0x17001607 RID: 5639
		// (get) Token: 0x0600B577 RID: 46455 RVA: 0x004389AF File Offset: 0x00436BAF
		// (set) Token: 0x0600B578 RID: 46456 RVA: 0x004389B7 File Offset: 0x00436BB7
		public object Value { get; set; }

		// Token: 0x0600B579 RID: 46457
		public abstract void WritePayload(BinaryWriter bw);

		// Token: 0x0600B57A RID: 46458 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public SdfTag()
		{
		}
	}
}
