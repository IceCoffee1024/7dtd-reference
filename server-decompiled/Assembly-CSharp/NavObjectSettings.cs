using System;
using UnityEngine;

// Token: 0x02000749 RID: 1865
public class NavObjectSettings
{
	// Token: 0x06003796 RID: 14230 RVA: 0x0016D74C File Offset: 0x0016B94C
	public virtual void Init()
	{
		this.Properties.ParseString("sprite_name", ref this.SpriteName);
		this.Properties.ParseFloat("min_distance", ref this.MinDistance);
		this.Properties.ParseFloat("max_distance", ref this.MaxDistance);
		this.Properties.ParseVec("offset", ref this.Offset);
		this.Properties.ParseColor("color", ref this.Color);
		this.Properties.ParseBool("has_pulse", ref this.HasPulse);
	}

	// Token: 0x04002D7D RID: 11645
	public DynamicProperties Properties;

	// Token: 0x04002D7E RID: 11646
	public string SpriteName = "";

	// Token: 0x04002D7F RID: 11647
	public float MinDistance;

	// Token: 0x04002D80 RID: 11648
	public float MaxDistance = -1f;

	// Token: 0x04002D81 RID: 11649
	public Vector3 Offset;

	// Token: 0x04002D82 RID: 11650
	public Color Color = Color.white;

	// Token: 0x04002D83 RID: 11651
	public bool HasPulse;
}
