using System;
using System.IO;

namespace SDF
{
	// Token: 0x02001694 RID: 5780
	public class SdfFloat : SdfTag
	{
		// Token: 0x0600B567 RID: 46439 RVA: 0x004387E0 File Offset: 0x004369E0
		public SdfFloat(string _name, float _value)
		{
			base.TagType = SdfTagType.Float;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B568 RID: 46440 RVA: 0x00438802 File Offset: 0x00436A02
		public override void WritePayload(BinaryWriter bw)
		{
			bw.Write((float)base.Value);
		}
	}
}
