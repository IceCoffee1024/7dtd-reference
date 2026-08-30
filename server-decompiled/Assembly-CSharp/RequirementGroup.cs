using System;
using System.Collections.Generic;

// Token: 0x02000664 RID: 1636
public sealed class RequirementGroup
{
	// Token: 0x060034CD RID: 13517 RVA: 0x0015E9CE File Offset: 0x0015CBCE
	public RequirementGroup(RequirementGroup.Op _op, List<IRequirement> _reqs, List<RequirementGroup> _groups)
	{
		this.op = _op;
		this.reqs = _reqs;
		this.groups = _groups;
	}

	// Token: 0x060034CE RID: 13518 RVA: 0x0015E9EC File Offset: 0x0015CBEC
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

	// Token: 0x060034CF RID: 13519 RVA: 0x0015EA28 File Offset: 0x0015CC28
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

	// Token: 0x060034D0 RID: 13520 RVA: 0x0015EABC File Offset: 0x0015CCBC
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

	// Token: 0x060034D1 RID: 13521 RVA: 0x0015EB60 File Offset: 0x0015CD60
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

	// Token: 0x04002B0F RID: 11023
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly RequirementGroup.Op op;

	// Token: 0x04002B10 RID: 11024
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<IRequirement> reqs;

	// Token: 0x04002B11 RID: 11025
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<RequirementGroup> groups;

	// Token: 0x02000665 RID: 1637
	public enum Op
	{
		// Token: 0x04002B13 RID: 11027
		And,
		// Token: 0x04002B14 RID: 11028
		Or
	}
}
