using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006A1 RID: 1697
[Preserve]
public class InBiome : TargetedCompareRequirementBase
{
	// Token: 0x060035B2 RID: 13746 RVA: 0x00161B08 File Offset: 0x0015FD08
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

	// Token: 0x060035B3 RID: 13747 RVA: 0x00161B5C File Offset: 0x0015FD5C
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("{0}in biome {1}", this.invert ? "NOT " : "", this.biomeID));
	}

	// Token: 0x060035B4 RID: 13748 RVA: 0x00161B90 File Offset: 0x0015FD90
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

	// Token: 0x04002B52 RID: 11090
	[PublicizedFrom(EAccessModifier.Private)]
	public int biomeID;
}
