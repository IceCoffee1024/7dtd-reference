using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006A5 RID: 1701
[Preserve]
public class IsBloodMoon : TargetedCompareRequirementBase
{
	// Token: 0x060035B8 RID: 13752 RVA: 0x00162587 File Offset: 0x00160787
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params) && (SkyManager.IsBloodMoonVisible() ^ this.invert);
	}

	// Token: 0x060035B9 RID: 13753 RVA: 0x001625A0 File Offset: 0x001607A0
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} blood moon", this.invert ? "NOT " : ""));
	}
}
