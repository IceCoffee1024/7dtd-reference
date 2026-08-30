using System;
using System.Globalization;
using UnityEngine;

// Token: 0x02000754 RID: 1876
public class NavObjectMapSettings : NavObjectSettings
{
	// Token: 0x060037EE RID: 14318 RVA: 0x0016FA24 File Offset: 0x0016DC24
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

	// Token: 0x04002DFC RID: 11772
	public int Layer;

	// Token: 0x04002DFD RID: 11773
	public float IconScale = 1f;

	// Token: 0x04002DFE RID: 11774
	public Vector3 IconScaleVector = Vector3.one;

	// Token: 0x04002DFF RID: 11775
	public bool UseRotation;

	// Token: 0x04002E00 RID: 11776
	public bool AdjustCenter;
}
