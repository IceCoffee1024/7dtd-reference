using System;
using UnityEngine;

// Token: 0x02000753 RID: 1875
public class NavObjectSettings
{
	// Token: 0x060037EC RID: 14316 RVA: 0x0016F968 File Offset: 0x0016DB68
	public virtual void Init()
	{
		this.Properties.ParseString("sprite_name", ref this.SpriteName);
		this.Properties.ParseFloat("min_distance", ref this.MinDistance);
		this.Properties.ParseFloat("max_distance", ref this.MaxDistance);
		this.Properties.ParseVec("offset", ref this.Offset);
		this.Properties.ParseColor("color", ref this.Color);
		this.Properties.ParseBool("has_pulse", ref this.HasPulse);
	}

	// Token: 0x04002DF5 RID: 11765
	public DynamicProperties Properties;

	// Token: 0x04002DF6 RID: 11766
	public string SpriteName = "";

	// Token: 0x04002DF7 RID: 11767
	public float MinDistance;

	// Token: 0x04002DF8 RID: 11768
	public float MaxDistance = -1f;

	// Token: 0x04002DF9 RID: 11769
	public Vector3 Offset;

	// Token: 0x04002DFA RID: 11770
	public Color Color = Color.white;

	// Token: 0x04002DFB RID: 11771
	public bool HasPulse;
}
