using System;
using System.IO;

namespace SDF
{
	// Token: 0x0200166F RID: 5743
	public class SdfInt : SdfTag
	{
		// Token: 0x0600B3E6 RID: 46054 RVA: 0x00432A9D File Offset: 0x00430C9D
		public SdfInt(string _name, int _value)
		{
			base.TagType = SdfTagType.Int;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B3E7 RID: 46055 RVA: 0x00432ABF File Offset: 0x00430CBF
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write((int)base.Value);
		}
	}
}
