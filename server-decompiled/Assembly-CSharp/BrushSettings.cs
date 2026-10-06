using System;
using UnityEngine;

// Token: 0x02001359 RID: 4953
[Serializable]
public class BrushSettings
{
	// Token: 0x170012B5 RID: 4789
	// (get) Token: 0x06009C14 RID: 39956 RVA: 0x003AF039 File Offset: 0x003AD239
	// (set) Token: 0x06009C15 RID: 39957 RVA: 0x003AF041 File Offset: 0x003AD241
	public string MainTexName { get; set; } = "_MainTex";

	// Token: 0x170012B6 RID: 4790
	// (get) Token: 0x06009C16 RID: 39958 RVA: 0x003AF04A File Offset: 0x003AD24A
	// (set) Token: 0x06009C17 RID: 39959 RVA: 0x003AF052 File Offset: 0x003AD252
	public string DirectionMapName { get; set; } = "_DirectionMap";

	// Token: 0x170012B7 RID: 4791
	// (get) Token: 0x06009C18 RID: 39960 RVA: 0x003AF05B File Offset: 0x003AD25B
	// (set) Token: 0x06009C19 RID: 39961 RVA: 0x003AF063 File Offset: 0x003AD263
	public string RMOLMapName { get; set; } = "_RMOL";

	// Token: 0x170012B8 RID: 4792
	// (get) Token: 0x06009C1A RID: 39962 RVA: 0x003AF06C File Offset: 0x003AD26C
	// (set) Token: 0x06009C1B RID: 39963 RVA: 0x003AF074 File Offset: 0x003AD274
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

	// Token: 0x170012B9 RID: 4793
	// (get) Token: 0x06009C1C RID: 39964 RVA: 0x003AF08D File Offset: 0x003AD28D
	// (set) Token: 0x06009C1D RID: 39965 RVA: 0x003AF095 File Offset: 0x003AD295
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

	// Token: 0x170012BA RID: 4794
	// (get) Token: 0x06009C1E RID: 39966 RVA: 0x003AF0AA File Offset: 0x003AD2AA
	// (set) Token: 0x06009C1F RID: 39967 RVA: 0x003AF0B2 File Offset: 0x003AD2B2
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

	// Token: 0x170012BB RID: 4795
	// (get) Token: 0x06009C20 RID: 39968 RVA: 0x003AF0C7 File Offset: 0x003AD2C7
	// (set) Token: 0x06009C21 RID: 39969 RVA: 0x003AF0CF File Offset: 0x003AD2CF
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

	// Token: 0x170012BC RID: 4796
	// (get) Token: 0x06009C22 RID: 39970 RVA: 0x003AF0DD File Offset: 0x003AD2DD
	// (set) Token: 0x06009C23 RID: 39971 RVA: 0x003AF0E5 File Offset: 0x003AD2E5
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

	// Token: 0x170012BD RID: 4797
	// (get) Token: 0x06009C24 RID: 39972 RVA: 0x003AF0F3 File Offset: 0x003AD2F3
	// (set) Token: 0x06009C25 RID: 39973 RVA: 0x003AF0FB File Offset: 0x003AD2FB
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

	// Token: 0x170012BE RID: 4798
	// (get) Token: 0x06009C26 RID: 39974 RVA: 0x003AF109 File Offset: 0x003AD309
	// (set) Token: 0x06009C27 RID: 39975 RVA: 0x003AF111 File Offset: 0x003AD311
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

	// Token: 0x170012BF RID: 4799
	// (get) Token: 0x06009C28 RID: 39976 RVA: 0x003AF11F File Offset: 0x003AD31F
	// (set) Token: 0x06009C29 RID: 39977 RVA: 0x003AF127 File Offset: 0x003AD327
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

	// Token: 0x170012C0 RID: 4800
	// (get) Token: 0x06009C2A RID: 39978 RVA: 0x003AF135 File Offset: 0x003AD335
	// (set) Token: 0x06009C2B RID: 39979 RVA: 0x003AF13D File Offset: 0x003AD33D
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

	// Token: 0x170012C1 RID: 4801
	// (get) Token: 0x06009C2C RID: 39980 RVA: 0x003AF146 File Offset: 0x003AD346
	// (set) Token: 0x06009C2D RID: 39981 RVA: 0x003AF154 File Offset: 0x003AD354
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

	// Token: 0x06009C2E RID: 39982 RVA: 0x003AF160 File Offset: 0x003AD360
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

	// Token: 0x040075A3 RID: 30115
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D brushPreview;

	// Token: 0x040075A4 RID: 30116
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float size = 100f;

	// Token: 0x040075A5 RID: 30117
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float strength = 0.5f;

	// Token: 0x040075A6 RID: 30118
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float falloff = 0.5f;

	// Token: 0x040075A7 RID: 30119
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float matting = 1f;

	// Token: 0x040075A8 RID: 30120
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float length = 1f;

	// Token: 0x040075A9 RID: 30121
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float roughness = 0.5f;

	// Token: 0x040075AA RID: 30122
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float metallic = 0.5f;

	// Token: 0x040075AB RID: 30123
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float occlusion = 1f;

	// Token: 0x040075AC RID: 30124
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color color = Color.white;

	// Token: 0x040075AD RID: 30125
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isDirty = true;
}
