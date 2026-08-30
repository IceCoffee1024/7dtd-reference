using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000620 RID: 1568
[Preserve]
public class LootEntryRequirementRandomRoll : BaseOperationLootEntryRequirement
{
	// Token: 0x060032CB RID: 13003 RVA: 0x0014A4F6 File Offset: 0x001486F6
	public override void Init(XElement e)
	{
		base.Init(e);
		e.ParseAttribute("min_max", ref this.minMax);
		e.ParseAttribute("value", ref this.valueText);
	}

	// Token: 0x060032CC RID: 13004 RVA: 0x0014A530 File Offset: 0x00148730
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float LeftSide(EntityPlayer player)
	{
		float randomFloat = GameEventManager.Current.Random.RandomFloat;
		return Mathf.Lerp(this.minMax.x, this.minMax.y, randomFloat);
	}

	// Token: 0x060032CD RID: 13005 RVA: 0x0014A569 File Offset: 0x00148769
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float RightSide(EntityPlayer player)
	{
		return GameEventManager.GetFloatValue(player, this.valueText, 0f);
	}

	// Token: 0x0400286B RID: 10347
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector2 minMax;

	// Token: 0x0400286C RID: 10348
	[PublicizedFrom(EAccessModifier.Protected)]
	public GameRandom rand;

	// Token: 0x0400286D RID: 10349
	[PublicizedFrom(EAccessModifier.Protected)]
	public string valueText;
}
