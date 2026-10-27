using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006A8 RID: 1704
[Preserve]
public class InBiome : TargetedCompareRequirementBase
{
	// Token: 0x060035C0 RID: 13760 RVA: 0x001626FC File Offset: 0x001608FC
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

	// Token: 0x060035C1 RID: 13761 RVA: 0x00162750 File Offset: 0x00160950
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("{0}in biome {1}", this.invert ? "NOT " : "", this.biomeID));
	}

	// Token: 0x060035C2 RID: 13762 RVA: 0x00162784 File Offset: 0x00160984
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

	// Token: 0x04002B6D RID: 11117
	[PublicizedFrom(EAccessModifier.Private)]
	public int biomeID;
}
