using System;
using System.IO;

namespace SDF
{
	// Token: 0x02001673 RID: 5747
	public class SdfByteArray : SdfTag
	{
		// Token: 0x0600B3EE RID: 46062 RVA: 0x00432BD2 File Offset: 0x00430DD2
		public SdfByteArray(string _name, byte[] _value)
		{
			base.TagType = SdfTagType.ByteArray;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B3EF RID: 46063 RVA: 0x00432BEF File Offset: 0x00430DEF
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write(((byte[])base.Value).Length);
			bw.Write((byte[])base.Value);
		}
	}
}
