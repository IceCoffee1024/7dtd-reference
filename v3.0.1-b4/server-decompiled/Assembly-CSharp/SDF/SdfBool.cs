using System;
using System.IO;

namespace SDF
{
	// Token: 0x02001671 RID: 5745
	public class SdfBool : SdfTag
	{
		// Token: 0x0600B3EA RID: 46058 RVA: 0x00432B4C File Offset: 0x00430D4C
		public SdfBool(string _name, bool _value)
		{
			base.TagType = SdfTagType.Bool;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B3EB RID: 46059 RVA: 0x00432B6E File Offset: 0x00430D6E
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write((bool)base.Value);
		}
	}
}
