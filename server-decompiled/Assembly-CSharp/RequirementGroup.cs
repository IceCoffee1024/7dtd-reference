using System;
using System.Collections.Generic;

// Token: 0x02000666 RID: 1638
public sealed class RequirementGroup
{
	// Token: 0x06003510 RID: 13584 RVA: 0x0015F332 File Offset: 0x0015D532
	public RequirementGroup(RequirementGroup.Op _op, List<IRequirement> _reqs, List<RequirementGroup> _groups)
	{
		this.op = _op;
		this.reqs = _reqs;
		this.groups = _groups;
	}

	// Token: 0x06003511 RID: 13585 RVA: 0x0015F350 File Offset: 0x0015D550
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

	// Token: 0x06003512 RID: 13586 RVA: 0x0015F38C File Offset: 0x0015D58C
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

	// Token: 0x06003513 RID: 13587 RVA: 0x0015F420 File Offset: 0x0015D620
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

	// Token: 0x06003514 RID: 13588 RVA: 0x0015F4C4 File Offset: 0x0015D6C4
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

	// Token: 0x04002B08 RID: 11016
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly RequirementGroup.Op op;

	// Token: 0x04002B09 RID: 11017
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<IRequirement> reqs;

	// Token: 0x04002B0A RID: 11018
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<RequirementGroup> groups;

	// Token: 0x02000667 RID: 1639
	public enum Op
	{
		// Token: 0x04002B0C RID: 11020
		And,
		// Token: 0x04002B0D RID: 11021
		Or
	}
}
