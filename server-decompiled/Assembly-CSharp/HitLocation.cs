using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006AC RID: 1708
[Preserve]
public class HitLocation : TargetedCompareRequirementBase
{
	// Token: 0x060035D2 RID: 13778 RVA: 0x00162256 File Offset: 0x00160456
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (!this.invert)
		{
			return (this.bodyParts & _params.DamageResponse.HitBodyPart) > EnumBodyPartHit.None;
		}
		return (this.bodyParts & _params.DamageResponse.HitBodyPart) == EnumBodyPartHit.None;
	}

	// Token: 0x060035D3 RID: 13779 RVA: 0x00162296 File Offset: 0x00160496
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("{0} hit location: ", this.invert ? "NOT " : "", this.bodyPartNames));
	}

	// Token: 0x060035D4 RID: 13780 RVA: 0x001622C4 File Offset: 0x001604C4
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "body_parts")
		{
			this.bodyPartNames = _attribute.Value;
			string[] array = this.bodyPartNames.Split(',', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				this.bodyParts |= EnumUtils.Parse<EnumBodyPartHit>(array[i], true);
			}
			return true;
		}
		return flag;
	}

	// Token: 0x04002B55 RID: 11093
	[PublicizedFrom(EAccessModifier.Private)]
	public string bodyPartNames = "";

	// Token: 0x04002B56 RID: 11094
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumBodyPartHit bodyParts;
}
