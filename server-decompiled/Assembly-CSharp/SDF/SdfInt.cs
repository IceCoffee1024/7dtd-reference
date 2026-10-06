using System;
using System.IO;

namespace SDF
{
	// Token: 0x020016C1 RID: 5825
	public class SdfInt : SdfTag
	{
		// Token: 0x0600B6C0 RID: 46784 RVA: 0x0043CD35 File Offset: 0x0043AF35
		public SdfInt(string _name, int _value)
		{
			base.TagType = SdfTagType.Int;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B6C1 RID: 46785 RVA: 0x0043CD57 File Offset: 0x0043AF57
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write((int)base.Value);
		}
	}
}
