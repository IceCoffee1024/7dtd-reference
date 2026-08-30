using System;
using System.IO;

namespace SDF
{
	// Token: 0x02001672 RID: 5746
	public class SdfBinary : SdfTag
	{
		// Token: 0x0600B3EC RID: 46060 RVA: 0x00432B81 File Offset: 0x00430D81
		public SdfBinary(string _name, string _value)
		{
			base.TagType = SdfTagType.Binary;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B3ED RID: 46061 RVA: 0x00432B9E File Offset: 0x00430D9E
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write((short)Utils.ToBase64(base.Value.ToString()).Length);
			bw.Write(Utils.ToBase64((string)base.Value));
		}
	}
}
