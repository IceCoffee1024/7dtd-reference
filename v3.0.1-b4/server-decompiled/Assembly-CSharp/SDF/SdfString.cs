using System;
using System.IO;

namespace SDF
{
	// Token: 0x02001670 RID: 5744
	public class SdfString : SdfTag
	{
		// Token: 0x0600B3E8 RID: 46056 RVA: 0x00432AD2 File Offset: 0x00430CD2
		public SdfString(string _name, string _value)
		{
			base.TagType = SdfTagType.String;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B3E9 RID: 46057 RVA: 0x00432AF0 File Offset: 0x00430CF0
		public override void WritePayload(BinaryWriter bw)
		{
			if (base.Value == null)
			{
				Log.Error("Null value: " + base.Name);
			}
			bw.Write((short)Utils.ToBase64(base.Value.ToString()).Length);
			bw.Write(Utils.ToBase64(base.Value.ToString()));
		}
	}
}
