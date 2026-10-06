using System;
using System.IO;

namespace SDF
{
	// Token: 0x020016C3 RID: 5827
	public class SdfBool : SdfTag
	{
		// Token: 0x0600B6C4 RID: 46788 RVA: 0x0043CDE4 File Offset: 0x0043AFE4
		public SdfBool(string _name, bool _value)
		{
			base.TagType = SdfTagType.Bool;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B6C5 RID: 46789 RVA: 0x0043CE06 File Offset: 0x0043B006
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write((bool)base.Value);
		}
	}
}
