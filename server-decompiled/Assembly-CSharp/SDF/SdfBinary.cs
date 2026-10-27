using System;
using System.IO;

namespace SDF
{
	// Token: 0x020016E3 RID: 5859
	public class SdfBinary : SdfTag
	{
		// Token: 0x0600B79A RID: 47002 RVA: 0x0043F8BD File Offset: 0x0043DABD
		public SdfBinary(string _name, string _value)
		{
			base.TagType = SdfTagType.Binary;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B79B RID: 47003 RVA: 0x0043F8DA File Offset: 0x0043DADA
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write((short)Utils.ToBase64(base.Value.ToString()).Length);
			bw.Write(Utils.ToBase64((string)base.Value));
		}
	}
}
