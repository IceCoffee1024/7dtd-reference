using System;
using System.IO;

namespace SDF
{
	// Token: 0x020016C2 RID: 5826
	public class SdfString : SdfTag
	{
		// Token: 0x0600B6C2 RID: 46786 RVA: 0x0043CD6A File Offset: 0x0043AF6A
		public SdfString(string _name, string _value)
		{
			base.TagType = SdfTagType.String;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B6C3 RID: 46787 RVA: 0x0043CD88 File Offset: 0x0043AF88
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
