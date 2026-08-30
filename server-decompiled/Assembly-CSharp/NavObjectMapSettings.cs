using System;
using System.Globalization;
using UnityEngine;

// Token: 0x0200074A RID: 1866
public class NavObjectMapSettings : NavObjectSettings
{
	// Token: 0x06003798 RID: 14232 RVA: 0x0016D808 File Offset: 0x0016BA08
	public override void Init()
	{
		base.Init();
		this.Properties.ParseInt("layer", ref this.Layer);
		if (this.Properties.Values.ContainsKey("icon_scale"))
		{
			this.IconScale = StringParsers.ParseFloat(this.Properties.Values["icon_scale"], 0, -1, NumberStyles.Any);
			this.IconScaleVector = new Vector3(this.IconScale, this.IconScale, this.IconScale);
		}
		this.Properties.ParseBool("adjust_center", ref this.AdjustCenter);
		this.Properties.ParseBool("use_rotation", ref this.UseRotation);
	}

	// Token: 0x04002D84 RID: 11652
	public int Layer;

	// Token: 0x04002D85 RID: 11653
	public float IconScale = 1f;

	// Token: 0x04002D86 RID: 11654
	public Vector3 IconScaleVector = Vector3.one;

	// Token: 0x04002D87 RID: 11655
	public bool UseRotation;

	// Token: 0x04002D88 RID: 11656
	public bool AdjustCenter;
}
