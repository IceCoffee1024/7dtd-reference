using System;
using UnityEngine;

// Token: 0x0200131F RID: 4895
[Serializable]
public class BrushSettings
{
	// Token: 0x17001237 RID: 4663
	// (get) Token: 0x06009A6D RID: 39533 RVA: 0x003A5225 File Offset: 0x003A3425
	// (set) Token: 0x06009A6E RID: 39534 RVA: 0x003A522D File Offset: 0x003A342D
	public string MainTexName { get; set; } = "_MainTex";

	// Token: 0x17001238 RID: 4664
	// (get) Token: 0x06009A6F RID: 39535 RVA: 0x003A5236 File Offset: 0x003A3436
	// (set) Token: 0x06009A70 RID: 39536 RVA: 0x003A523E File Offset: 0x003A343E
	public string DirectionMapName { get; set; } = "_DirectionMap";

	// Token: 0x17001239 RID: 4665
	// (get) Token: 0x06009A71 RID: 39537 RVA: 0x003A5247 File Offset: 0x003A3447
	// (set) Token: 0x06009A72 RID: 39538 RVA: 0x003A524F File Offset: 0x003A344F
	public string RMOLMapName { get; set; } = "_RMOL";

	// Token: 0x1700123A RID: 4666
	// (get) Token: 0x06009A73 RID: 39539 RVA: 0x003A5258 File Offset: 0x003A3458
	// (set) Token: 0x06009A74 RID: 39540 RVA: 0x003A5260 File Offset: 0x003A3460
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

	// Token: 0x1700123B RID: 4667
	// (get) Token: 0x06009A75 RID: 39541 RVA: 0x003A5279 File Offset: 0x003A3479
	// (set) Token: 0x06009A76 RID: 39542 RVA: 0x003A5281 File Offset: 0x003A3481
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

	// Token: 0x1700123C RID: 4668
	// (get) Token: 0x06009A77 RID: 39543 RVA: 0x003A5296 File Offset: 0x003A3496
	// (set) Token: 0x06009A78 RID: 39544 RVA: 0x003A529E File Offset: 0x003A349E
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

	// Token: 0x1700123D RID: 4669
	// (get) Token: 0x06009A79 RID: 39545 RVA: 0x003A52B3 File Offset: 0x003A34B3
	// (set) Token: 0x06009A7A RID: 39546 RVA: 0x003A52BB File Offset: 0x003A34BB
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

	// Token: 0x1700123E RID: 4670
	// (get) Token: 0x06009A7B RID: 39547 RVA: 0x003A52C9 File Offset: 0x003A34C9
	// (set) Token: 0x06009A7C RID: 39548 RVA: 0x003A52D1 File Offset: 0x003A34D1
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

	// Token: 0x1700123F RID: 4671
	// (get) Token: 0x06009A7D RID: 39549 RVA: 0x003A52DF File Offset: 0x003A34DF
	// (set) Token: 0x06009A7E RID: 39550 RVA: 0x003A52E7 File Offset: 0x003A34E7
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

	// Token: 0x17001240 RID: 4672
	// (get) Token: 0x06009A7F RID: 39551 RVA: 0x003A52F5 File Offset: 0x003A34F5
	// (set) Token: 0x06009A80 RID: 39552 RVA: 0x003A52FD File Offset: 0x003A34FD
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

	// Token: 0x17001241 RID: 4673
	// (get) Token: 0x06009A81 RID: 39553 RVA: 0x003A530B File Offset: 0x003A350B
	// (set) Token: 0x06009A82 RID: 39554 RVA: 0x003A5313 File Offset: 0x003A3513
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

	// Token: 0x17001242 RID: 4674
	// (get) Token: 0x06009A83 RID: 39555 RVA: 0x003A5321 File Offset: 0x003A3521
	// (set) Token: 0x06009A84 RID: 39556 RVA: 0x003A5329 File Offset: 0x003A3529
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

	// Token: 0x17001243 RID: 4675
	// (get) Token: 0x06009A85 RID: 39557 RVA: 0x003A5332 File Offset: 0x003A3532
	// (set) Token: 0x06009A86 RID: 39558 RVA: 0x003A5340 File Offset: 0x003A3540
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

	// Token: 0x06009A87 RID: 39559 RVA: 0x003A534C File Offset: 0x003A354C
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

	// Token: 0x04007450 RID: 29776
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D brushPreview;

	// Token: 0x04007451 RID: 29777
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float size = 100f;

	// Token: 0x04007452 RID: 29778
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float strength = 0.5f;

	// Token: 0x04007453 RID: 29779
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float falloff = 0.5f;

	// Token: 0x04007454 RID: 29780
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float matting = 1f;

	// Token: 0x04007455 RID: 29781
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float length = 1f;

	// Token: 0x04007456 RID: 29782
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float roughness = 0.5f;

	// Token: 0x04007457 RID: 29783
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float metallic = 0.5f;

	// Token: 0x04007458 RID: 29784
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float occlusion = 1f;

	// Token: 0x04007459 RID: 29785
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color color = Color.white;

	// Token: 0x0400745A RID: 29786
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isDirty = true;
}
