using System;
using System.IO;

namespace SDF
{
	// Token: 0x02001699 RID: 5785
	public class SdfByteArray : SdfTag
	{
		// Token: 0x0600B571 RID: 46449 RVA: 0x0043894A File Offset: 0x00436B4A
		public SdfByteArray(string _name, byte[] _value)
		{
			base.TagType = SdfTagType.ByteArray;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B572 RID: 46450 RVA: 0x00438967 File Offset: 0x00436B67
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write(((byte[])base.Value).Length);
			bw.Write((byte[])base.Value);
		}
	}
}
