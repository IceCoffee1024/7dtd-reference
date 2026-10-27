using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200060A RID: 1546
[Preserve]
public class ItemClassWaterContainer : ItemClass
{
	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x06003279 RID: 12921 RVA: 0x00147350 File Offset: 0x00145550
	// (set) Token: 0x0600327A RID: 12922 RVA: 0x00147358 File Offset: 0x00145558
	public int MaxMass
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600327B RID: 12923 RVA: 0x00147364 File Offset: 0x00145564
	public override void Init()
	{
		base.Init();
		float num = 0f;
		this.Properties.ParseFloat("WaterCapacity", ref num);
		this.MaxMass = Mathf.Clamp((int)(num * 19500f), 0, 65535);
		this.Properties.ParseFloat("InitialFillRatio", ref this.initialFillRatio);
		this.initialFillRatio = Mathf.Clamp(this.initialFillRatio, 0f, 1f);
	}

	// Token: 0x0600327C RID: 12924 RVA: 0x001473D9 File Offset: 0x001455D9
	public override int GetInitialMetadata(ItemValue _itemValue)
	{
		return (int)((float)this.MaxMass * this.initialFillRatio);
	}

	// Token: 0x040027B8 RID: 10168
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropWaterCapacity = "WaterCapacity";

	// Token: 0x040027B9 RID: 10169
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropInitFillRatio = "InitialFillRatio";

	// Token: 0x040027BB RID: 10171
	[PublicizedFrom(EAccessModifier.Private)]
	public float initialFillRatio;
}
