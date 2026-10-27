using System;
using UnityEngine;

// Token: 0x0200075D RID: 1885
public class NavObjectSettings
{
	// Token: 0x06003846 RID: 14406 RVA: 0x00171264 File Offset: 0x0016F464
	public virtual void Init()
	{
		this.Properties.ParseString("sprite_name", ref this.SpriteName);
		this.Properties.ParseFloat("min_distance", ref this.MinDistance);
		this.Properties.ParseFloat("max_distance", ref this.MaxDistance);
		this.Properties.ParseVec("offset", ref this.Offset);
		this.Properties.ParseColor("color", ref this.Color);
		this.Properties.ParseBool("has_pulse", ref this.HasPulse);
	}

	// Token: 0x04002E0A RID: 11786
	public DynamicProperties Properties;

	// Token: 0x04002E0B RID: 11787
	public string SpriteName = "";

	// Token: 0x04002E0C RID: 11788
	public float MinDistance;

	// Token: 0x04002E0D RID: 11789
	public float MaxDistance = -1f;

	// Token: 0x04002E0E RID: 11790
	public Vector3 Offset;

	// Token: 0x04002E0F RID: 11791
	public Color Color = Color.white;

	// Token: 0x04002E10 RID: 11792
	public bool HasPulse;
}
