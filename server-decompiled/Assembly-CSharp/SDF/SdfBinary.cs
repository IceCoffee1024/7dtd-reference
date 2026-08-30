using System;
using System.IO;

namespace SDF
{
	// Token: 0x02001698 RID: 5784
	public class SdfBinary : SdfTag
	{
		// Token: 0x0600B56F RID: 46447 RVA: 0x004388F9 File Offset: 0x00436AF9
		public SdfBinary(string _name, string _value)
		{
			base.TagType = SdfTagType.Binary;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B570 RID: 46448 RVA: 0x00438916 File Offset: 0x00436B16
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write((short)Utils.ToBase64(base.Value.ToString()).Length);
			bw.Write(Utils.ToBase64((string)base.Value));
		}
	}
}
