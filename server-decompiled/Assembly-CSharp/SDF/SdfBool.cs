using System;
using System.IO;

namespace SDF
{
	// Token: 0x020016E2 RID: 5858
	public class SdfBool : SdfTag
	{
		// Token: 0x0600B798 RID: 47000 RVA: 0x0043F888 File Offset: 0x0043DA88
		public SdfBool(string _name, bool _value)
		{
			base.TagType = SdfTagType.Bool;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B799 RID: 47001 RVA: 0x0043F8AA File Offset: 0x0043DAAA
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write((bool)base.Value);
		}
	}
}
