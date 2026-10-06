using System;
using System.Collections.Generic;
using System.Xml.Linq;

// Token: 0x0200066F RID: 1647
public abstract class StatCompareAbs : TargetedCompareRequirementBase
{
	// Token: 0x17000581 RID: 1409
	// (get) Token: 0x06003523 RID: 13603 RVA: 0x0015FA68 File Offset: 0x0015DC68
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

	// Token: 0x06003524 RID: 13604 RVA: 0x0015FAA8 File Offset: 0x0015DCA8
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params) && this.Compare(_params);
	}

	// Token: 0x06003525 RID: 13605
	public abstract bool Compare(MinEventParams _params);

	// Token: 0x06003526 RID: 13606 RVA: 0x0015FABC File Offset: 0x0015DCBC
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

	// Token: 0x06003527 RID: 13607 RVA: 0x0015FB24 File Offset: 0x0015DD24
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

	// Token: 0x06003528 RID: 13608 RVA: 0x0015F743 File Offset: 0x0015D943
	[PublicizedFrom(EAccessModifier.Protected)]
	public StatCompareAbs()
	{
	}

	// Token: 0x04002B24 RID: 11044
	[PublicizedFrom(EAccessModifier.Protected)]
	public StatCompareAbs.StatTypes stat;

	// Token: 0x04002B25 RID: 11045
	[PublicizedFrom(EAccessModifier.Protected)]
	public StatCompareAbs.StatSample sample;

	// Token: 0x02000670 RID: 1648
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum StatTypes
	{
		// Token: 0x04002B27 RID: 11047
		None,
		// Token: 0x04002B28 RID: 11048
		Health,
		// Token: 0x04002B29 RID: 11049
		Stamina,
		// Token: 0x04002B2A RID: 11050
		Food,
		// Token: 0x04002B2B RID: 11051
		Water,
		// Token: 0x04002B2C RID: 11052
		Armor
	}

	// Token: 0x02000671 RID: 1649
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum StatSample
	{
		// Token: 0x04002B2E RID: 11054
		Current,
		// Token: 0x04002B2F RID: 11055
		StartOfFrame
	}
}
