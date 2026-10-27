using System;
using System.IO;

namespace SDF
{
	// Token: 0x020016DF RID: 5855
	public class SdfFloat : SdfTag
	{
		// Token: 0x0600B792 RID: 46994 RVA: 0x0043F7A4 File Offset: 0x0043D9A4
		public SdfFloat(string _name, float _value)
		{
			base.TagType = SdfTagType.Float;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B793 RID: 46995 RVA: 0x0043F7C6 File Offset: 0x0043D9C6
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write((float)base.Value);
		}
	}
}
