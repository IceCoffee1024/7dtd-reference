using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000668 RID: 1640
[Preserve]
public class RandomRoll : TargetedCompareRequirementBase
{
	// Token: 0x06003515 RID: 13589 RVA: 0x0015F564 File Offset: 0x0015D764
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

	// Token: 0x06003516 RID: 13590 RVA: 0x0015F650 File Offset: 0x0015D850
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

	// Token: 0x06003517 RID: 13591 RVA: 0x0015F6B8 File Offset: 0x0015D8B8
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

	// Token: 0x04002B0E RID: 11022
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 minMax;

	// Token: 0x04002B0F RID: 11023
	[PublicizedFrom(EAccessModifier.Private)]
	public RandomRoll.SeedType seedType;

	// Token: 0x04002B10 RID: 11024
	[PublicizedFrom(EAccessModifier.Private)]
	public int seedAdditive;

	// Token: 0x04002B11 RID: 11025
	[PublicizedFrom(EAccessModifier.Private)]
	public GameRandom rand;

	// Token: 0x02000669 RID: 1641
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SeedType
	{
		// Token: 0x04002B13 RID: 11027
		Item,
		// Token: 0x04002B14 RID: 11028
		Player,
		// Token: 0x04002B15 RID: 11029
		Random
	}
}
