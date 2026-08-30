using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000666 RID: 1638
[Preserve]
public class RandomRoll : TargetedCompareRequirementBase
{
	// Token: 0x060034D2 RID: 13522 RVA: 0x0015EC00 File Offset: 0x0015CE00
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (this.cvarName != null)
		{
			this.value = this.target.Buffs.GetCustomVar(this.cvarName);
		}
		if (this.seedType == RandomRoll.SeedType.Item)
		{
			this.rand = GameRandomManager.Instance.CreateGameRandom(_params.Seed);
		}
		else if (this.seedType == RandomRoll.SeedType.Player)
		{
			this.rand = GameRandomManager.Instance.CreateGameRandom(_params.Self.entityId);
		}
		else
		{
			this.rand = GameRandomManager.Instance.CreateGameRandom(Environment.TickCount);
		}
		float randomFloat = this.rand.RandomFloat;
		GameRandomManager.Instance.FreeGameRandom(this.rand);
		return this.invert != RequirementBase.compareValues(Utils.FastLerp(this.minMax.x, this.minMax.y, randomFloat), this.operation, this.value);
	}

	// Token: 0x060034D3 RID: 13523 RVA: 0x0015ECEC File Offset: 0x0015CEEC
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("roll[{0}-{1}] {2} {3}", new object[]
		{
			this.minMax.x.ToCultureInvariantString(),
			this.minMax.y.ToCultureInvariantString(),
			this.operation.ToStringCached<RequirementBase.OperationTypes>(),
			this.value.ToCultureInvariantString()
		}));
	}

	// Token: 0x060034D4 RID: 13524 RVA: 0x0015ED54 File Offset: 0x0015CF54
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "min_max")
			{
				this.minMax = StringParsers.ParseVector2(_attribute.Value);
				return true;
			}
			if (localName == "seed_type")
			{
				this.seedType = EnumUtils.Parse<RandomRoll.SeedType>(_attribute.Value, true);
				return true;
			}
			if (localName == "seed_additive")
			{
				this.seedAdditive = StringParsers.ParseSInt32(_attribute.Value, 0, -1, NumberStyles.Integer);
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002B15 RID: 11029
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 minMax;

	// Token: 0x04002B16 RID: 11030
	[PublicizedFrom(EAccessModifier.Private)]
	public RandomRoll.SeedType seedType;

	// Token: 0x04002B17 RID: 11031
	[PublicizedFrom(EAccessModifier.Private)]
	public int seedAdditive;

	// Token: 0x04002B18 RID: 11032
	[PublicizedFrom(EAccessModifier.Private)]
	public GameRandom rand;

	// Token: 0x02000667 RID: 1639
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SeedType
	{
		// Token: 0x04002B1A RID: 11034
		Item,
		// Token: 0x04002B1B RID: 11035
		Player,
		// Token: 0x04002B1C RID: 11036
		Random
	}
}
