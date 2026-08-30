using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200065D RID: 1629
[Preserve]
public class RandomRoll : TargetedCompareRequirementBase
{
	// Token: 0x0600347F RID: 13439 RVA: 0x0015CA90 File Offset: 0x0015AC90
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

	// Token: 0x06003480 RID: 13440 RVA: 0x0015CB7C File Offset: 0x0015AD7C
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

	// Token: 0x06003481 RID: 13441 RVA: 0x0015CBE4 File Offset: 0x0015ADE4
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

	// Token: 0x04002AA1 RID: 10913
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 minMax;

	// Token: 0x04002AA2 RID: 10914
	[PublicizedFrom(EAccessModifier.Private)]
	public RandomRoll.SeedType seedType;

	// Token: 0x04002AA3 RID: 10915
	[PublicizedFrom(EAccessModifier.Private)]
	public int seedAdditive;

	// Token: 0x04002AA4 RID: 10916
	[PublicizedFrom(EAccessModifier.Private)]
	public GameRandom rand;

	// Token: 0x0200065E RID: 1630
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SeedType
	{
		// Token: 0x04002AA6 RID: 10918
		Item,
		// Token: 0x04002AA7 RID: 10919
		Player,
		// Token: 0x04002AA8 RID: 10920
		Random
	}
}
