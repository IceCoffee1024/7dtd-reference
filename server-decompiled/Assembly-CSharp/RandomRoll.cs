using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200066F RID: 1647
[Preserve]
public class RandomRoll : TargetedCompareRequirementBase
{
	// Token: 0x06003523 RID: 13603 RVA: 0x00160158 File Offset: 0x0015E358
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

	// Token: 0x06003524 RID: 13604 RVA: 0x00160244 File Offset: 0x0015E444
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

	// Token: 0x06003525 RID: 13605 RVA: 0x001602AC File Offset: 0x0015E4AC
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

	// Token: 0x04002B29 RID: 11049
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 minMax;

	// Token: 0x04002B2A RID: 11050
	[PublicizedFrom(EAccessModifier.Private)]
	public RandomRoll.SeedType seedType;

	// Token: 0x04002B2B RID: 11051
	[PublicizedFrom(EAccessModifier.Private)]
	public int seedAdditive;

	// Token: 0x04002B2C RID: 11052
	[PublicizedFrom(EAccessModifier.Private)]
	public GameRandom rand;

	// Token: 0x02000670 RID: 1648
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SeedType
	{
		// Token: 0x04002B2E RID: 11054
		Item,
		// Token: 0x04002B2F RID: 11055
		Player,
		// Token: 0x04002B30 RID: 11056
		Random
	}
}
