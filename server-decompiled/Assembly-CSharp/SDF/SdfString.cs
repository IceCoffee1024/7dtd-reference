using System;
using System.IO;

namespace SDF
{
	// Token: 0x020016E1 RID: 5857
	public class SdfString : SdfTag
	{
		// Token: 0x0600B796 RID: 46998 RVA: 0x0043F80E File Offset: 0x0043DA0E
		public SdfString(string _name, string _value)
		{
			base.TagType = SdfTagType.String;
			base.Name = _name;
			base.Value = _value;
		}

		// Token: 0x0600B797 RID: 46999 RVA: 0x0043F82C File Offset: 0x0043DA2C
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
