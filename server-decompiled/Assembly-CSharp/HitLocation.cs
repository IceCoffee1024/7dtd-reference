using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006A0 RID: 1696
[Preserve]
public class HitLocation : TargetedCompareRequirementBase
{
	// Token: 0x06003539 RID: 13625 RVA: 0x0015F716 File Offset: 0x0015D916
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

	// Token: 0x0600353A RID: 13626 RVA: 0x0015F756 File Offset: 0x0015D956
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("{0} hit location: ", this.invert ? "NOT " : "", this.bodyPartNames));
	}

	// Token: 0x0600353B RID: 13627 RVA: 0x0015F784 File Offset: 0x0015D984
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

	// Token: 0x04002AE8 RID: 10984
	[PublicizedFrom(EAccessModifier.Private)]
	public string bodyPartNames = "";

	// Token: 0x04002AE9 RID: 10985
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumBodyPartHit bodyParts;
}
