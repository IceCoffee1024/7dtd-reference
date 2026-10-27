using System;
using System.Collections.Generic;
using System.Xml.Linq;

// Token: 0x02000676 RID: 1654
public abstract class StatCompareAbs : TargetedCompareRequirementBase
{
	// Token: 0x17000581 RID: 1409
	// (get) Token: 0x06003531 RID: 13617 RVA: 0x0016065C File Offset: 0x0015E85C
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

	// Token: 0x06003532 RID: 13618 RVA: 0x0016069C File Offset: 0x0015E89C
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params) && this.Compare(_params);
	}

	// Token: 0x06003533 RID: 13619
	public abstract bool Compare(MinEventParams _params);

	// Token: 0x06003534 RID: 13620 RVA: 0x001606B0 File Offset: 0x0015E8B0
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

	// Token: 0x06003535 RID: 13621 RVA: 0x00160718 File Offset: 0x0015E918
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

	// Token: 0x06003536 RID: 13622 RVA: 0x00160337 File Offset: 0x0015E537
	[PublicizedFrom(EAccessModifier.Protected)]
	public StatCompareAbs()
	{
	}

	// Token: 0x04002B3F RID: 11071
	[PublicizedFrom(EAccessModifier.Protected)]
	public StatCompareAbs.StatTypes stat;

	// Token: 0x04002B40 RID: 11072
	[PublicizedFrom(EAccessModifier.Protected)]
	public StatCompareAbs.StatSample sample;

	// Token: 0x02000677 RID: 1655
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum StatTypes
	{
		// Token: 0x04002B42 RID: 11074
		None,
		// Token: 0x04002B43 RID: 11075
		Health,
		// Token: 0x04002B44 RID: 11076
		Stamina,
		// Token: 0x04002B45 RID: 11077
		Food,
		// Token: 0x04002B46 RID: 11078
		Water,
		// Token: 0x04002B47 RID: 11079
		Armor
	}

	// Token: 0x02000678 RID: 1656
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum StatSample
	{
		// Token: 0x04002B49 RID: 11081
		Current,
		// Token: 0x04002B4A RID: 11082
		StartOfFrame
	}
}
