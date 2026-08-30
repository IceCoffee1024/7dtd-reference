using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200069F RID: 1695
[Preserve]
public class InBiome : TargetedCompareRequirementBase
{
	// Token: 0x0600356F RID: 13679 RVA: 0x00161188 File Offset: 0x0015F388
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (_params.Biome == null)
		{
			return false;
		}
		if (!this.invert)
		{
			return this.biomeID == (int)_params.Biome.m_Id;
		}
		return this.biomeID != (int)_params.Biome.m_Id;
	}

	// Token: 0x06003570 RID: 13680 RVA: 0x001611DC File Offset: 0x0015F3DC
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("{0}in biome {1}", this.invert ? "NOT " : "", this.biomeID));
	}

	// Token: 0x06003571 RID: 13681 RVA: 0x00161210 File Offset: 0x0015F410
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "biome")
		{
			this.biomeID = StringParsers.ParseSInt32(_attribute.Value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return flag;
	}

	// Token: 0x04002B59 RID: 11097
	[PublicizedFrom(EAccessModifier.Private)]
	public int biomeID;
}
