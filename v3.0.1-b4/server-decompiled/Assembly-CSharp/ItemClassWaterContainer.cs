using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005FA RID: 1530
[Preserve]
public class ItemClassWaterContainer : ItemClass
{
	// Token: 0x17000518 RID: 1304
	// (get) Token: 0x060031D9 RID: 12761 RVA: 0x00143F8C File Offset: 0x0014218C
	// (set) Token: 0x060031DA RID: 12762 RVA: 0x00143F94 File Offset: 0x00142194
	public int MaxMass
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060031DB RID: 12763 RVA: 0x00143FA0 File Offset: 0x001421A0
	public override void Init()
	{
		base.Init();
		float num = 0f;
		this.Properties.ParseFloat("WaterCapacity", ref num);
		this.MaxMass = Mathf.Clamp((int)(num * 19500f), 0, 65535);
		this.Properties.ParseFloat("InitialFillRatio", ref this.initialFillRatio);
		this.initialFillRatio = Mathf.Clamp(this.initialFillRatio, 0f, 1f);
	}

	// Token: 0x060031DC RID: 12764 RVA: 0x00144015 File Offset: 0x00142215
	public override int GetInitialMetadata(ItemValue _itemValue)
	{
		return (int)((float)this.MaxMass * this.initialFillRatio);
	}

	// Token: 0x04002743 RID: 10051
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropWaterCapacity = "WaterCapacity";

	// Token: 0x04002744 RID: 10052
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropInitFillRatio = "InitialFillRatio";

	// Token: 0x04002746 RID: 10054
	[PublicizedFrom(EAccessModifier.Private)]
	public float initialFillRatio;
}
