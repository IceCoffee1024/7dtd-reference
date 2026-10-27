using System;
using System.Globalization;
using UnityEngine;

// Token: 0x0200075E RID: 1886
public class NavObjectMapSettings : NavObjectSettings
{
	// Token: 0x06003848 RID: 14408 RVA: 0x00171320 File Offset: 0x0016F520
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

	// Token: 0x04002E11 RID: 11793
	public int Layer;

	// Token: 0x04002E12 RID: 11794
	public float IconScale = 1f;

	// Token: 0x04002E13 RID: 11795
	public Vector3 IconScaleVector = Vector3.one;

	// Token: 0x04002E14 RID: 11796
	public bool UseRotation;

	// Token: 0x04002E15 RID: 11797
	public bool AdjustCenter;
}
