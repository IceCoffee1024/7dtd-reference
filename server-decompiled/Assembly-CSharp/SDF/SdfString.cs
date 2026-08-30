using System;
using System.IO;

namespace SDF
{
	// Token: 0x02001696 RID: 5782
	public class SdfString : SdfTag
	{
		// Token: 0x0600B56B RID: 46443 RVA: 0x0043884A File Offset: 0x00436A4A
		public SdfString(string _name, string _value)
		{
			base.TagType = SdfTagType.String;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B56C RID: 46444 RVA: 0x00438868 File Offset: 0x00436A68
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
