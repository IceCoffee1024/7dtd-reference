using System;
using System.IO;

namespace SDF
{
	// Token: 0x020016E4 RID: 5860
	public class SdfByteArray : SdfTag
	{
		// Token: 0x0600B79C RID: 47004 RVA: 0x0043F90E File Offset: 0x0043DB0E
		public SdfByteArray(string _name, byte[] _value)
		{
			base.TagType = SdfTagType.ByteArray;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B79D RID: 47005 RVA: 0x0043F92B File Offset: 0x0043DB2B
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write(((byte[])base.Value).Length);
			bw.Write((byte[])base.Value);
		}
	}
}
