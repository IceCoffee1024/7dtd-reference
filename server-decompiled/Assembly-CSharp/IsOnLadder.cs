using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006AD RID: 1709
[Preserve]
public class IsOnLadder : TargetedCompareRequirementBase
{
	// Token: 0x06003599 RID: 13721 RVA: 0x00161ACF File Offset: 0x0015FCCF
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (!this.invert)
		{
			return this.target.IsInElevator();
		}
		return !this.target.IsInElevator();
	}

	// Token: 0x0600359A RID: 13722 RVA: 0x00161AFE File Offset: 0x0015FCFE
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} On Ladder", this.invert ? "NOT " : ""));
	}
}
