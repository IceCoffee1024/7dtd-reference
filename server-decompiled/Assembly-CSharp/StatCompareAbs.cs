using System;
using System.Collections.Generic;
using System.Xml.Linq;

// Token: 0x0200066D RID: 1645
public abstract class StatCompareAbs : TargetedCompareRequirementBase
{
	// Token: 0x1700055B RID: 1371
	// (get) Token: 0x060034E0 RID: 13536 RVA: 0x0015F104 File Offset: 0x0015D304
	public EntityStats Stats
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			StatCompareAbs.StatSample statSample = this.sample;
			if (statSample == StatCompareAbs.StatSample.Current)
			{
				return this.target.Stats;
			}
			if (statSample != StatCompareAbs.StatSample.StartOfFrame)
			{
				return this.target.Stats;
			}
			return this.target.StartOfFrameStats;
		}
	}

	// Token: 0x060034E1 RID: 13537 RVA: 0x0015F144 File Offset: 0x0015D344
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params) && this.Compare(_params);
	}

	// Token: 0x060034E2 RID: 13538
	public abstract bool Compare(MinEventParams _params);

	// Token: 0x060034E3 RID: 13539 RVA: 0x0015F158 File Offset: 0x0015D358
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("stat '{1}' {0} {2} {3}", new object[]
		{
			this.invert ? "NOT" : "",
			this.stat.ToStringCached<StatCompareAbs.StatTypes>(),
			this.operation.ToStringCached<RequirementBase.OperationTypes>(),
			this.value.ToCultureInvariantString()
		}));
	}

	// Token: 0x060034E4 RID: 13540 RVA: 0x0015F1C0 File Offset: 0x0015D3C0
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "stat")
			{
				this.stat = EnumUtils.Parse<StatCompareAbs.StatTypes>(_attribute.Value, true);
				return true;
			}
			if (localName == "sample")
			{
				this.sample = EnumUtils.Parse<StatCompareAbs.StatSample>(_attribute.Value, true);
				return true;
			}
		}
		return flag;
	}

	// Token: 0x060034E5 RID: 13541 RVA: 0x0015EDDF File Offset: 0x0015CFDF
	[PublicizedFrom(EAccessModifier.Protected)]
	public StatCompareAbs()
	{
	}

	// Token: 0x04002B2B RID: 11051
	[PublicizedFrom(EAccessModifier.Protected)]
	public StatCompareAbs.StatTypes stat;

	// Token: 0x04002B2C RID: 11052
	[PublicizedFrom(EAccessModifier.Protected)]
	public StatCompareAbs.StatSample sample;

	// Token: 0x0200066E RID: 1646
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum StatTypes
	{
		// Token: 0x04002B2E RID: 11054
		None,
		// Token: 0x04002B2F RID: 11055
		Health,
		// Token: 0x04002B30 RID: 11056
		Stamina,
		// Token: 0x04002B31 RID: 11057
		Food,
		// Token: 0x04002B32 RID: 11058
		Water,
		// Token: 0x04002B33 RID: 11059
		Armor
	}

	// Token: 0x0200066F RID: 1647
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum StatSample
	{
		// Token: 0x04002B35 RID: 11061
		Current,
		// Token: 0x04002B36 RID: 11062
		StartOfFrame
	}
}
