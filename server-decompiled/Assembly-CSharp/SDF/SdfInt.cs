using System;
using System.IO;

namespace SDF
{
	// Token: 0x020016E0 RID: 5856
	public class SdfInt : SdfTag
	{
		// Token: 0x0600B794 RID: 46996 RVA: 0x0043F7D9 File Offset: 0x0043D9D9
		public SdfInt(string _name, int _value)
		{
			base.TagType = SdfTagType.Int;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B795 RID: 46997 RVA: 0x0043F7FB File Offset: 0x0043D9FB
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write((int)base.Value);
		}
	}
}
