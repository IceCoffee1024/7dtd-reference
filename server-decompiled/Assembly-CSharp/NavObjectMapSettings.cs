using System;
using System.Globalization;
using UnityEngine;

// Token: 0x02000756 RID: 1878
public class NavObjectMapSettings : NavObjectSettings
{
	// Token: 0x06003831 RID: 14385 RVA: 0x00170388 File Offset: 0x0016E588
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

	// Token: 0x04002DF5 RID: 11765
	public int Layer;

	// Token: 0x04002DF6 RID: 11766
	public float IconScale = 1f;

	// Token: 0x04002DF7 RID: 11767
	public Vector3 IconScaleVector = Vector3.one;

	// Token: 0x04002DF8 RID: 11768
	public bool UseRotation;

	// Token: 0x04002DF9 RID: 11769
	public bool AdjustCenter;
}
