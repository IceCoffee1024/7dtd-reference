using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000629 RID: 1577
[Preserve]
public class LootEntryRequirementRandomRoll : BaseOperationLootEntryRequirement
{
	// Token: 0x0600331E RID: 13086 RVA: 0x0014C612 File Offset: 0x0014A812
	public override void Init(XElement e)
	{
		base.Init(e);
		e.ParseAttribute("min_max", ref this.minMax);
		e.ParseAttribute("value", ref this.valueText);
	}

	// Token: 0x0600331F RID: 13087 RVA: 0x0014C64C File Offset: 0x0014A84C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float LeftSide(EntityPlayer player)
	{
		float randomFloat = GameEventManager.Current.Random.RandomFloat;
		return Mathf.Lerp(this.minMax.x, this.minMax.y, randomFloat);
	}

	// Token: 0x06003320 RID: 13088 RVA: 0x0014C685 File Offset: 0x0014A885
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float RightSide(EntityPlayer player)
	{
		return GameEventManager.GetFloatValue(player, this.valueText, 0f);
	}

	// Token: 0x040028DE RID: 10462
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector2 minMax;

	// Token: 0x040028DF RID: 10463
	[PublicizedFrom(EAccessModifier.Protected)]
	public GameRandom rand;

	// Token: 0x040028E0 RID: 10464
	[PublicizedFrom(EAccessModifier.Protected)]
	public string valueText;
}
