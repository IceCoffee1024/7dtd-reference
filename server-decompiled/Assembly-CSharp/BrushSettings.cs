using System;
using UnityEngine;

// Token: 0x0200136B RID: 4971
[Serializable]
public class BrushSettings
{
	// Token: 0x170012C1 RID: 4801
	// (get) Token: 0x06009C6C RID: 40044 RVA: 0x003B143D File Offset: 0x003AF63D
	// (set) Token: 0x06009C6D RID: 40045 RVA: 0x003B1445 File Offset: 0x003AF645
	public string MainTexName { get; set; } = "_MainTex";

	// Token: 0x170012C2 RID: 4802
	// (get) Token: 0x06009C6E RID: 40046 RVA: 0x003B144E File Offset: 0x003AF64E
	// (set) Token: 0x06009C6F RID: 40047 RVA: 0x003B1456 File Offset: 0x003AF656
	public string DirectionMapName { get; set; } = "_DirectionMap";

	// Token: 0x170012C3 RID: 4803
	// (get) Token: 0x06009C70 RID: 40048 RVA: 0x003B145F File Offset: 0x003AF65F
	// (set) Token: 0x06009C71 RID: 40049 RVA: 0x003B1467 File Offset: 0x003AF667
	public string RMOLMapName { get; set; } = "_RMOL";

	// Token: 0x170012C4 RID: 4804
	// (get) Token: 0x06009C72 RID: 40050 RVA: 0x003B1470 File Offset: 0x003AF670
	// (set) Token: 0x06009C73 RID: 40051 RVA: 0x003B1478 File Offset: 0x003AF678
	public float Size
	{
		get
		{
			return this.size;
		}
		set
		{
			this.UpdateBrushPreview();
			this.size = Mathf.Max(1f, value);
		}
	}

	// Token: 0x170012C5 RID: 4805
	// (get) Token: 0x06009C74 RID: 40052 RVA: 0x003B1491 File Offset: 0x003AF691
	// (set) Token: 0x06009C75 RID: 40053 RVA: 0x003B1499 File Offset: 0x003AF699
	public float Strength
	{
		get
		{
			return this.strength;
		}
		set
		{
			this.isDirty = true;
			this.strength = Mathf.Clamp01(value);
		}
	}

	// Token: 0x170012C6 RID: 4806
	// (get) Token: 0x06009C76 RID: 40054 RVA: 0x003B14AE File Offset: 0x003AF6AE
	// (set) Token: 0x06009C77 RID: 40055 RVA: 0x003B14B6 File Offset: 0x003AF6B6
	public float Falloff
	{
		get
		{
			return this.falloff;
		}
		set
		{
			this.isDirty = true;
			this.falloff = Mathf.Clamp01(value);
		}
	}

	// Token: 0x170012C7 RID: 4807
	// (get) Token: 0x06009C78 RID: 40056 RVA: 0x003B14CB File Offset: 0x003AF6CB
	// (set) Token: 0x06009C79 RID: 40057 RVA: 0x003B14D3 File Offset: 0x003AF6D3
	public float Matting
	{
		get
		{
			return this.matting;
		}
		set
		{
			this.matting = Mathf.Clamp01(value);
		}
	}

	// Token: 0x170012C8 RID: 4808
	// (get) Token: 0x06009C7A RID: 40058 RVA: 0x003B14E1 File Offset: 0x003AF6E1
	// (set) Token: 0x06009C7B RID: 40059 RVA: 0x003B14E9 File Offset: 0x003AF6E9
	public float Length
	{
		get
		{
			return this.length;
		}
		set
		{
			this.length = Mathf.Clamp01(value);
		}
	}

	// Token: 0x170012C9 RID: 4809
	// (get) Token: 0x06009C7C RID: 40060 RVA: 0x003B14F7 File Offset: 0x003AF6F7
	// (set) Token: 0x06009C7D RID: 40061 RVA: 0x003B14FF File Offset: 0x003AF6FF
	public float Roughness
	{
		get
		{
			return this.roughness;
		}
		set
		{
			this.roughness = Mathf.Clamp01(value);
		}
	}

	// Token: 0x170012CA RID: 4810
	// (get) Token: 0x06009C7E RID: 40062 RVA: 0x003B150D File Offset: 0x003AF70D
	// (set) Token: 0x06009C7F RID: 40063 RVA: 0x003B1515 File Offset: 0x003AF715
	public float Metallic
	{
		get
		{
			return this.metallic;
		}
		set
		{
			this.metallic = Mathf.Clamp01(value);
		}
	}

	// Token: 0x170012CB RID: 4811
	// (get) Token: 0x06009C80 RID: 40064 RVA: 0x003B1523 File Offset: 0x003AF723
	// (set) Token: 0x06009C81 RID: 40065 RVA: 0x003B152B File Offset: 0x003AF72B
	public float Occlusion
	{
		get
		{
			return this.occlusion;
		}
		set
		{
			this.occlusion = Mathf.Clamp01(value);
		}
	}

	// Token: 0x170012CC RID: 4812
	// (get) Token: 0x06009C82 RID: 40066 RVA: 0x003B1539 File Offset: 0x003AF739
	// (set) Token: 0x06009C83 RID: 40067 RVA: 0x003B1541 File Offset: 0x003AF741
	public Color Color
	{
		get
		{
			return this.color;
		}
		set
		{
			this.color = value;
		}
	}

	// Token: 0x170012CD RID: 4813
	// (get) Token: 0x06009C84 RID: 40068 RVA: 0x003B154A File Offset: 0x003AF74A
	// (set) Token: 0x06009C85 RID: 40069 RVA: 0x003B1558 File Offset: 0x003AF758
	public Texture2D BrushPreview
	{
		get
		{
			this.UpdateBrushPreview();
			return this.brushPreview;
		}
		set
		{
			this.brushPreview = value;
		}
	}

	// Token: 0x06009C86 RID: 40070 RVA: 0x003B1564 File Offset: 0x003AF764
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateBrushPreview()
	{
		if (!this.isDirty)
		{
			return;
		}
		this.isDirty = false;
		if (this.brushPreview == null)
		{
			this.brushPreview = new Texture2D(100, 100);
		}
		Color[] array = new Color[this.brushPreview.width * this.brushPreview.height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Color.clear;
		}
		int num = Mathf.FloorToInt(this.size * 0.5f);
		for (int j = 0; j < 100; j++)
		{
			for (int k = 0; k < 100; k++)
			{
				Vector2 vector = new Vector2((float)j, (float)k) - new Vector2(50f, 50f);
				float num2 = Vector3.Dot(vector, vector);
				if (num2 <= (float)(num * num))
				{
					float num3 = Mathf.Pow(Mathf.Clamp01(1f - MathF.Sqrt(num2) / (float)num), this.falloff * 4f) * this.strength;
					array[k * this.brushPreview.width + j] = new Color(num3, num3, num3, 1f);
				}
				else
				{
					array[k * this.brushPreview.width + j] = Color.black;
				}
			}
		}
		this.brushPreview.SetPixels(array);
		this.brushPreview.Apply();
	}

	// Token: 0x040075E4 RID: 30180
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D brushPreview;

	// Token: 0x040075E5 RID: 30181
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float size = 100f;

	// Token: 0x040075E6 RID: 30182
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float strength = 0.5f;

	// Token: 0x040075E7 RID: 30183
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float falloff = 0.5f;

	// Token: 0x040075E8 RID: 30184
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float matting = 1f;

	// Token: 0x040075E9 RID: 30185
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float length = 1f;

	// Token: 0x040075EA RID: 30186
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float roughness = 0.5f;

	// Token: 0x040075EB RID: 30187
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float metallic = 0.5f;

	// Token: 0x040075EC RID: 30188
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float occlusion = 1f;

	// Token: 0x040075ED RID: 30189
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color color = Color.white;

	// Token: 0x040075EE RID: 30190
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isDirty = true;
}
