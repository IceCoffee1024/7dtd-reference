using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000632 RID: 1586
[Preserve]
public class LootEntryRequirementRandomRoll : BaseOperationLootEntryRequirement
{
	// Token: 0x0600336F RID: 13167 RVA: 0x0014DB76 File Offset: 0x0014BD76
	public override void Init(XElement e)
	{
		base.Init(e);
		e.ParseAttribute("min_max", ref this.minMax);
		e.ParseAttribute("value", ref this.valueText);
	}

	// Token: 0x06003370 RID: 13168 RVA: 0x0014DBB0 File Offset: 0x0014BDB0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float LeftSide(EntityPlayer player)
	{
		float randomFloat = GameEventManager.Current.Random.RandomFloat;
		return Mathf.Lerp(this.minMax.x, this.minMax.y, randomFloat);
	}

	// Token: 0x06003371 RID: 13169 RVA: 0x0014DBE9 File Offset: 0x0014BDE9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float RightSide(EntityPlayer player)
	{
		return GameEventManager.GetFloatValue(player, this.valueText, 0f);
	}

	// Token: 0x040028F2 RID: 10482
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector2 minMax;

	// Token: 0x040028F3 RID: 10483
	[PublicizedFrom(EAccessModifier.Protected)]
	public GameRandom rand;

	// Token: 0x040028F4 RID: 10484
	[PublicizedFrom(EAccessModifier.Protected)]
	public string valueText;
}
