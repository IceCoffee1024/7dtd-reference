using System;
using System.IO;

namespace SDF
{
	// Token: 0x020016C5 RID: 5829
	public class SdfByteArray : SdfTag
	{
		// Token: 0x0600B6C8 RID: 46792 RVA: 0x0043CE6A File Offset: 0x0043B06A
		public SdfByteArray(string _name, byte[] _value)
		{
			base.TagType = SdfTagType.ByteArray;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B6C9 RID: 46793 RVA: 0x0043CE87 File Offset: 0x0043B087
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write(((byte[])base.Value).Length);
			bw.Write((byte[])base.Value);
		}
	}
}
