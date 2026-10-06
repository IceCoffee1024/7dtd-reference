using System;
using System.IO;

namespace SDF
{
	// Token: 0x020016C4 RID: 5828
	public class SdfBinary : SdfTag
	{
		// Token: 0x0600B6C6 RID: 46790 RVA: 0x0043CE19 File Offset: 0x0043B019
		public SdfBinary(string _name, string _value)
		{
			base.TagType = SdfTagType.Binary;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B6C7 RID: 46791 RVA: 0x0043CE36 File Offset: 0x0043B036
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write((short)Utils.ToBase64(base.Value.ToString()).Length);
			bw.Write(Utils.ToBase64((string)base.Value));
		}
	}
}
