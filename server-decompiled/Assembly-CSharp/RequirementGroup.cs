using System;
using System.Collections.Generic;

// Token: 0x0200065B RID: 1627
public sealed class RequirementGroup
{
	// Token: 0x0600347A RID: 13434 RVA: 0x0015C85E File Offset: 0x0015AA5E
	public RequirementGroup(RequirementGroup.Op _op, List<IRequirement> _reqs, List<RequirementGroup> _groups)
	{
		this.op = _op;
		this.reqs = _reqs;
		this.groups = _groups;
	}

	// Token: 0x0600347B RID: 13435 RVA: 0x0015C87C File Offset: 0x0015AA7C
	public bool IsValid(MinEventParams _params)
	{
		RequirementGroup.Op op = this.op;
		bool flag;
		if (op != RequirementGroup.Op.And)
		{
			if (op != RequirementGroup.Op.Or)
			{
				flag = this.EvalAnd(_params);
			}
			else
			{
				flag = this.EvalOr(_params);
			}
		}
		else
		{
			flag = this.EvalAnd(_params);
		}
		return flag;
	}

	// Token: 0x0600347C RID: 13436 RVA: 0x0015C8B8 File Offset: 0x0015AAB8
	public void GetInfoStrings(ref List<string> list)
	{
		if (this.reqs != null)
		{
			for (int i = 0; i < this.reqs.Count; i++)
			{
				string description = this.reqs[i].GetDescription();
				if (string.IsNullOrEmpty(description))
				{
					this.reqs[i].GetInfoStrings(ref list);
				}
				else
				{
					list.Add(description);
				}
			}
		}
		if (this.groups != null)
		{
			for (int j = 0; j < this.groups.Count; j++)
			{
				this.groups[j].GetInfoStrings(ref list);
			}
		}
	}

	// Token: 0x0600347D RID: 13437 RVA: 0x0015C94C File Offset: 0x0015AB4C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool EvalAnd(MinEventParams _params)
	{
		if ((this.reqs == null || this.reqs.Count == 0) && (this.groups == null || this.groups.Count == 0))
		{
			return true;
		}
		if (this.reqs != null)
		{
			for (int i = 0; i < this.reqs.Count; i++)
			{
				if (!this.reqs[i].IsValid(_params))
				{
					return false;
				}
			}
		}
		if (this.groups != null)
		{
			for (int j = 0; j < this.groups.Count; j++)
			{
				if (!this.groups[j].IsValid(_params))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600347E RID: 13438 RVA: 0x0015C9F0 File Offset: 0x0015ABF0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool EvalOr(MinEventParams _params)
	{
		bool flag = false;
		if (this.reqs != null)
		{
			flag |= this.reqs.Count > 0;
			for (int i = 0; i < this.reqs.Count; i++)
			{
				if (this.reqs[i].IsValid(_params))
				{
					return true;
				}
			}
		}
		if (this.groups != null)
		{
			flag |= this.groups.Count > 0;
			for (int j = 0; j < this.groups.Count; j++)
			{
				if (this.groups[j].IsValid(_params))
				{
					return true;
				}
			}
		}
		return !flag;
	}

	// Token: 0x04002A9B RID: 10907
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly RequirementGroup.Op op;

	// Token: 0x04002A9C RID: 10908
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<IRequirement> reqs;

	// Token: 0x04002A9D RID: 10909
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<RequirementGroup> groups;

	// Token: 0x0200065C RID: 1628
	public enum Op
	{
		// Token: 0x04002A9F RID: 10911
		And,
		// Token: 0x04002AA0 RID: 10912
		Or
	}
}
