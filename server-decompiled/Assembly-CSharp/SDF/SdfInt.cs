using System;
using System.IO;

namespace SDF
{
	// Token: 0x02001695 RID: 5781
	public class SdfInt : SdfTag
	{
		// Token: 0x0600B569 RID: 46441 RVA: 0x00438815 File Offset: 0x00436A15
		public SdfInt(string _name, int _value)
		{
			base.TagType = SdfTagType.Int;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B56A RID: 46442 RVA: 0x00438837 File Offset: 0x00436A37
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write((int)base.Value);
		}
	}
}
