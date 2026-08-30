using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000601 RID: 1537
[Preserve]
public class ItemClassWaterContainer : ItemClass
{
	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x06003228 RID: 12840 RVA: 0x00145DD0 File Offset: 0x00143FD0
	// (set) Token: 0x06003229 RID: 12841 RVA: 0x00145DD8 File Offset: 0x00143FD8
	public int MaxMass
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600322A RID: 12842 RVA: 0x00145DE4 File Offset: 0x00143FE4
	public override void Init()
	{
		base.Init();
		float num = 0f;
		this.Properties.ParseFloat("WaterCapacity", ref num);
		this.MaxMass = Mathf.Clamp((int)(num * 19500f), 0, 65535);
		this.Properties.ParseFloat("InitialFillRatio", ref this.initialFillRatio);
		this.initialFillRatio = Mathf.Clamp(this.initialFillRatio, 0f, 1f);
	}

	// Token: 0x0600322B RID: 12843 RVA: 0x00145E59 File Offset: 0x00144059
	public override int GetInitialMetadata(ItemValue _itemValue)
	{
		return (int)((float)this.MaxMass * this.initialFillRatio);
	}

	// Token: 0x040027A6 RID: 10150
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropWaterCapacity = "WaterCapacity";

	// Token: 0x040027A7 RID: 10151
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropInitFillRatio = "InitialFillRatio";

	// Token: 0x040027A9 RID: 10153
	[PublicizedFrom(EAccessModifier.Private)]
	public float initialFillRatio;
}
