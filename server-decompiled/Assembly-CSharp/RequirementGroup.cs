using System;
using System.Collections.Generic;

// Token: 0x0200066D RID: 1645
public sealed class RequirementGroup
{
	// Token: 0x0600351E RID: 13598 RVA: 0x0015FF26 File Offset: 0x0015E126
	public RequirementGroup(RequirementGroup.Op _op, List<IRequirement> _reqs, List<RequirementGroup> _groups)
	{
		this.op = _op;
		this.reqs = _reqs;
		this.groups = _groups;
	}

	// Token: 0x0600351F RID: 13599 RVA: 0x0015FF44 File Offset: 0x0015E144
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

	// Token: 0x06003520 RID: 13600 RVA: 0x0015FF80 File Offset: 0x0015E180
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

	// Token: 0x06003521 RID: 13601 RVA: 0x00160014 File Offset: 0x0015E214
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

	// Token: 0x06003522 RID: 13602 RVA: 0x001600B8 File Offset: 0x0015E2B8
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

	// Token: 0x04002B23 RID: 11043
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly RequirementGroup.Op op;

	// Token: 0x04002B24 RID: 11044
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<IRequirement> reqs;

	// Token: 0x04002B25 RID: 11045
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<RequirementGroup> groups;

	// Token: 0x0200066E RID: 1646
	public enum Op
	{
		// Token: 0x04002B27 RID: 11047
		And,
		// Token: 0x04002B28 RID: 11048
		Or
	}
}
