using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000695 RID: 1685
[Preserve]
public class InBiome : TargetedCompareRequirementBase
{
	// Token: 0x06003519 RID: 13593 RVA: 0x0015EFB0 File Offset: 0x0015D1B0
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

	// Token: 0x0600351A RID: 13594 RVA: 0x0015F004 File Offset: 0x0015D204
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("{0}in biome {1}", this.invert ? "NOT " : "", this.biomeID));
	}

	// Token: 0x0600351B RID: 13595 RVA: 0x0015F038 File Offset: 0x0015D238
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

	// Token: 0x04002AE5 RID: 10981
	[PublicizedFrom(EAccessModifier.Private)]
	public int biomeID;
}
