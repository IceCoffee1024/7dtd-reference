using System;
using System.IO;

namespace SDF
{
	// Token: 0x02001697 RID: 5783
	public class SdfBool : SdfTag
	{
		// Token: 0x0600B56D RID: 46445 RVA: 0x004388C4 File Offset: 0x00436AC4
		public SdfBool(string _name, bool _value)
		{
			base.TagType = SdfTagType.Bool;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B56E RID: 46446 RVA: 0x004388E6 File Offset: 0x00436AE6
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write((bool)base.Value);
		}
	}
}
