using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200062B RID: 1579
[Preserve]
public class LootEntryRequirementRandomRoll : BaseOperationLootEntryRequirement
{
	// Token: 0x06003361 RID: 13153 RVA: 0x0014CF82 File Offset: 0x0014B182
	public override void Init(XElement e)
	{
		base.Init(e);
		e.ParseAttribute("min_max", ref this.minMax);
		e.ParseAttribute("value", ref this.valueText);
	}

	// Token: 0x06003362 RID: 13154 RVA: 0x0014CFBC File Offset: 0x0014B1BC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float LeftSide(EntityPlayer player)
	{
		float randomFloat = GameEventManager.Current.Random.RandomFloat;
		return Mathf.Lerp(this.minMax.x, this.minMax.y, randomFloat);
	}

	// Token: 0x06003363 RID: 13155 RVA: 0x0014CFF5 File Offset: 0x0014B1F5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float RightSide(EntityPlayer player)
	{
		return GameEventManager.GetFloatValue(player, this.valueText, 0f);
	}

	// Token: 0x040028D7 RID: 10455
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector2 minMax;

	// Token: 0x040028D8 RID: 10456
	[PublicizedFrom(EAccessModifier.Protected)]
	public GameRandom rand;

	// Token: 0x040028D9 RID: 10457
	[PublicizedFrom(EAccessModifier.Protected)]
	public string valueText;
}
