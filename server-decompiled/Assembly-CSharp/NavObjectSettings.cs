using System;
using UnityEngine;

// Token: 0x02000755 RID: 1877
public class NavObjectSettings
{
	// Token: 0x0600382F RID: 14383 RVA: 0x001702CC File Offset: 0x0016E4CC
	public virtual void Init()
	{
		this.Properties.ParseString("sprite_name", ref this.SpriteName);
		this.Properties.ParseFloat("min_distance", ref this.MinDistance);
		this.Properties.ParseFloat("max_distance", ref this.MaxDistance);
		this.Properties.ParseVec("offset", ref this.Offset);
		this.Properties.ParseColor("color", ref this.Color);
		this.Properties.ParseBool("has_pulse", ref this.HasPulse);
	}

	// Token: 0x04002DEE RID: 11758
	public DynamicProperties Properties;

	// Token: 0x04002DEF RID: 11759
	public string SpriteName = "";

	// Token: 0x04002DF0 RID: 11760
	public float MinDistance;

	// Token: 0x04002DF1 RID: 11761
	public float MaxDistance = -1f;

	// Token: 0x04002DF2 RID: 11762
	public Vector3 Offset;

	// Token: 0x04002DF3 RID: 11763
	public Color Color = Color.white;

	// Token: 0x04002DF4 RID: 11764
	public bool HasPulse;
}
