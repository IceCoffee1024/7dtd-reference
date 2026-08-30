using System;
using System.Collections.Generic;
using System.Xml.Linq;

// Token: 0x02000664 RID: 1636
public abstract class StatCompareAbs : TargetedCompareRequirementBase
{
	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x0600348D RID: 13453 RVA: 0x0015CF94 File Offset: 0x0015B194
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

	// Token: 0x0600348E RID: 13454 RVA: 0x0015CFD4 File Offset: 0x0015B1D4
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params) && this.Compare(_params);
	}

	// Token: 0x0600348F RID: 13455
	public abstract bool Compare(MinEventParams _params);

	// Token: 0x06003490 RID: 13456 RVA: 0x0015CFE8 File Offset: 0x0015B1E8
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

	// Token: 0x06003491 RID: 13457 RVA: 0x0015D050 File Offset: 0x0015B250
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

	// Token: 0x06003492 RID: 13458 RVA: 0x0015CC6F File Offset: 0x0015AE6F
	[PublicizedFrom(EAccessModifier.Protected)]
	public StatCompareAbs()
	{
	}

	// Token: 0x04002AB7 RID: 10935
	[PublicizedFrom(EAccessModifier.Protected)]
	public StatCompareAbs.StatTypes stat;

	// Token: 0x04002AB8 RID: 10936
	[PublicizedFrom(EAccessModifier.Protected)]
	public StatCompareAbs.StatSample sample;

	// Token: 0x02000665 RID: 1637
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum StatTypes
	{
		// Token: 0x04002ABA RID: 10938
		None,
		// Token: 0x04002ABB RID: 10939
		Health,
		// Token: 0x04002ABC RID: 10940
		Stamina,
		// Token: 0x04002ABD RID: 10941
		Food,
		// Token: 0x04002ABE RID: 10942
		Water,
		// Token: 0x04002ABF RID: 10943
		Armor
	}

	// Token: 0x02000666 RID: 1638
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum StatSample
	{
		// Token: 0x04002AC1 RID: 10945
		Current,
		// Token: 0x04002AC2 RID: 10946
		StartOfFrame
	}
}
