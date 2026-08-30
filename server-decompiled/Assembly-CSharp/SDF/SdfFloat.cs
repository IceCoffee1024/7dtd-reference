using System;
using System.IO;

namespace SDF
{
	// Token: 0x0200166E RID: 5742
	public class SdfFloat : SdfTag
	{
		// Token: 0x0600B3E4 RID: 46052 RVA: 0x00432A68 File Offset: 0x00430C68
		public SdfFloat(string _name, float _value)
		{
			base.TagType = SdfTagType.Float;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B3E5 RID: 46053 RVA: 0x00432A8A File Offset: 0x00430C8A
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write((float)base.Value);
		}
	}
}
