using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200069C RID: 1692
[Preserve]
public class IsBloodMoon : TargetedCompareRequirementBase
{
	// Token: 0x06003567 RID: 13671 RVA: 0x00161013 File Offset: 0x0015F213
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params) && (SkyManager.IsBloodMoonVisible() ^ this.invert);
	}

	// Token: 0x06003568 RID: 13672 RVA: 0x0016102C File Offset: 0x0015F22C
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} blood moon", this.invert ? "NOT " : ""));
	}
}
