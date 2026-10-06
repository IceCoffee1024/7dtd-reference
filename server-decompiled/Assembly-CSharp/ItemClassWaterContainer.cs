using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000603 RID: 1539
[Preserve]
public class ItemClassWaterContainer : ItemClass
{
	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x0600326B RID: 12907 RVA: 0x0014675C File Offset: 0x0014495C
	// (set) Token: 0x0600326C RID: 12908 RVA: 0x00146764 File Offset: 0x00144964
	public int MaxMass
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600326D RID: 12909 RVA: 0x00146770 File Offset: 0x00144970
	public override void Init()
	{
		base.Init();
		float num = 0f;
		this.Properties.ParseFloat("WaterCapacity", ref num);
		this.MaxMass = Mathf.Clamp((int)(num * 19500f), 0, 65535);
		this.Properties.ParseFloat("InitialFillRatio", ref this.initialFillRatio);
		this.initialFillRatio = Mathf.Clamp(this.initialFillRatio, 0f, 1f);
	}

	// Token: 0x0600326E RID: 12910 RVA: 0x001467E5 File Offset: 0x001449E5
	public override int GetInitialMetadata(ItemValue _itemValue)
	{
		return (int)((float)this.MaxMass * this.initialFillRatio);
	}

	// Token: 0x0400279D RID: 10141
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropWaterCapacity = "WaterCapacity";

	// Token: 0x0400279E RID: 10142
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropInitFillRatio = "InitialFillRatio";

	// Token: 0x040027A0 RID: 10144
	[PublicizedFrom(EAccessModifier.Private)]
	public float initialFillRatio;
}
