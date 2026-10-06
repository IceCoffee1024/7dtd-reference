using System;
using System.IO;

namespace SDF
{
	// Token: 0x020016C0 RID: 5824
	public class SdfFloat : SdfTag
	{
		// Token: 0x0600B6BE RID: 46782 RVA: 0x0043CD00 File Offset: 0x0043AF00
		public SdfFloat(string _name, float _value)
		{
			base.TagType = SdfTagType.Float;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B6BF RID: 46783 RVA: 0x0043CD22 File Offset: 0x0043AF22
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write((float)base.Value);
		}
	}
}
