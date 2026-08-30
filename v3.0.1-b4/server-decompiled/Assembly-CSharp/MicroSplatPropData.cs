using System;
using UnityEngine;

// Token: 0x0200002B RID: 43
public class MicroSplatPropData : ScriptableObject
{
	// Token: 0x06000122 RID: 290 RVA: 0x0000CFD4 File Offset: 0x0000B1D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void RevisionData()
	{
		if (this.values.Length == 256)
		{
			Color[] array = new Color[1024];
			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					array[j * 32 + i] = this.values[j * 32 + i];
				}
			}
			this.values = array;
			return;
		}
		if (this.values.Length == 512)
		{
			Color[] array2 = new Color[1024];
			for (int k = 0; k < 32; k++)
			{
				for (int l = 0; l < 16; l++)
				{
					array2[l * 32 + k] = this.values[l * 32 + k];
				}
			}
			this.values = array2;
		}
	}

	// Token: 0x06000123 RID: 291 RVA: 0x0000D09E File Offset: 0x0000B29E
	public Color GetValue(int x, int y)
	{
		this.RevisionData();
		return this.values[y * 32 + x];
	}

	// Token: 0x06000124 RID: 292 RVA: 0x0000D0B7 File Offset: 0x0000B2B7
	public void SetValue(int x, int y, Color c)
	{
		this.RevisionData();
		this.values[y * 32 + x] = c;
	}

	// Token: 0x06000125 RID: 293 RVA: 0x0000D0D4 File Offset: 0x0000B2D4
	public void SetValue(int x, int y, int channel, float value)
	{
		this.RevisionData();
		int num = y * 32 + x;
		Color color = this.values[num];
		color[channel] = value;
		this.values[num] = color;
	}

	// Token: 0x06000126 RID: 294 RVA: 0x0000D114 File Offset: 0x0000B314
	public void SetValue(int x, int y, int channel, Vector2 value)
	{
		this.RevisionData();
		int num = y * 32 + x;
		Color color = this.values[num];
		if (channel == 0)
		{
			color.r = value.x;
			color.g = value.y;
		}
		else
		{
			color.b = value.x;
			color.a = value.y;
		}
		this.values[num] = color;
	}

	// Token: 0x06000127 RID: 295 RVA: 0x0000D188 File Offset: 0x0000B388
	public void SetValue(int textureIndex, MicroSplatPropData.PerTexFloat channel, float value)
	{
		float num = (float)channel / 4f;
		int num2 = (int)num;
		int num3 = Mathf.RoundToInt((num - (float)num2) * 4f);
		this.SetValue(textureIndex, num2, num3, value);
	}

	// Token: 0x06000128 RID: 296 RVA: 0x0000D1BC File Offset: 0x0000B3BC
	public void SetValue(int textureIndex, MicroSplatPropData.PerTexColor channel, Color value)
	{
		int num = (int)((float)channel / 4f);
		this.SetValue(textureIndex, num, value);
	}

	// Token: 0x06000129 RID: 297 RVA: 0x0000D1DC File Offset: 0x0000B3DC
	public void SetValue(int textureIndex, MicroSplatPropData.PerTexVector2 channel, Vector2 value)
	{
		float num = (float)channel / 4f;
		int num2 = (int)num;
		int num3 = Mathf.RoundToInt((num - (float)num2) * 4f);
		this.SetValue(textureIndex, num2, num3, value);
	}

	// Token: 0x0600012A RID: 298 RVA: 0x0000D210 File Offset: 0x0000B410
	public Texture2D GetTexture()
	{
		this.RevisionData();
		if (this.tex == null)
		{
			if (SystemInfo.SupportsTextureFormat(TextureFormat.RGBAFloat))
			{
				this.tex = new Texture2D(32, 32, TextureFormat.RGBAFloat, false, true);
			}
			else if (SystemInfo.SupportsTextureFormat(TextureFormat.RGBAHalf))
			{
				this.tex = new Texture2D(32, 32, TextureFormat.RGBAHalf, false, true);
			}
			else
			{
				Debug.LogError("Could not create RGBAFloat or RGBAHalf format textures, per texture properties will be clamped to 0-1 range, which will break things");
				this.tex = new Texture2D(32, 32, TextureFormat.RGBA32, false, true);
			}
			this.tex.hideFlags = HideFlags.HideAndDontSave;
			this.tex.wrapMode = TextureWrapMode.Clamp;
			this.tex.filterMode = FilterMode.Point;
		}
		this.tex.SetPixels(this.values);
		this.tex.Apply();
		return this.tex;
	}

	// Token: 0x0600012B RID: 299 RVA: 0x0000D2D0 File Offset: 0x0000B4D0
	public Texture2D GetGeoCurve()
	{
		if (this.geoTex == null)
		{
			this.geoTex = new Texture2D(256, 1, TextureFormat.RHalf, false, true);
			this.geoTex.hideFlags = HideFlags.HideAndDontSave;
		}
		for (int i = 0; i < 256; i++)
		{
			float num = this.geoCurve.Evaluate((float)i / 255f);
			this.geoTex.SetPixel(i, 0, new Color(num, num, num, num));
		}
		this.geoTex.Apply();
		return this.geoTex;
	}

	// Token: 0x0600012C RID: 300 RVA: 0x0000D358 File Offset: 0x0000B558
	public Texture2D GetGeoSlopeFilter()
	{
		if (this.geoSlopeTex == null)
		{
			this.geoSlopeTex = new Texture2D(256, 1, TextureFormat.Alpha8, false, true);
			this.geoSlopeTex.hideFlags = HideFlags.HideAndDontSave;
		}
		for (int i = 0; i < 256; i++)
		{
			float num = this.geoSlopeFilter.Evaluate((float)i / 255f);
			this.geoSlopeTex.SetPixel(i, 0, new Color(num, num, num, num));
		}
		this.geoSlopeTex.Apply();
		return this.geoSlopeTex;
	}

	// Token: 0x0600012D RID: 301 RVA: 0x0000D3E0 File Offset: 0x0000B5E0
	public Texture2D GetGlobalSlopeFilter()
	{
		if (this.globalSlopeTex == null)
		{
			this.globalSlopeTex = new Texture2D(256, 1, TextureFormat.Alpha8, false, true);
			this.globalSlopeTex.hideFlags = HideFlags.HideAndDontSave;
		}
		for (int i = 0; i < 256; i++)
		{
			float num = this.globalSlopeFilter.Evaluate((float)i / 255f);
			this.globalSlopeTex.SetPixel(i, 0, new Color(num, num, num, num));
		}
		this.globalSlopeTex.Apply();
		return this.globalSlopeTex;
	}

	// Token: 0x0400015E RID: 350
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int sMaxTextures = 32;

	// Token: 0x0400015F RID: 351
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int sMaxAttributes = 32;

	// Token: 0x04000160 RID: 352
	public Color[] values = new Color[1024];

	// Token: 0x04000161 RID: 353
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D tex;

	// Token: 0x04000162 RID: 354
	[HideInInspector]
	public AnimationCurve geoCurve = AnimationCurve.Linear(0f, 0f, 0f, 0f);

	// Token: 0x04000163 RID: 355
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D geoTex;

	// Token: 0x04000164 RID: 356
	[HideInInspector]
	public AnimationCurve geoSlopeFilter = AnimationCurve.Linear(0f, 0.2f, 0.4f, 1f);

	// Token: 0x04000165 RID: 357
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D geoSlopeTex;

	// Token: 0x04000166 RID: 358
	[HideInInspector]
	public AnimationCurve globalSlopeFilter = AnimationCurve.Linear(0f, 0.2f, 0.4f, 1f);

	// Token: 0x04000167 RID: 359
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D globalSlopeTex;

	// Token: 0x0200002C RID: 44
	public enum PerTexVector2
	{
		// Token: 0x04000169 RID: 361
		SplatUVScale,
		// Token: 0x0400016A RID: 362
		SplatUVOffset = 2
	}

	// Token: 0x0200002D RID: 45
	public enum PerTexColor
	{
		// Token: 0x0400016C RID: 364
		Tint = 4,
		// Token: 0x0400016D RID: 365
		SSSRTint = 72
	}

	// Token: 0x0200002E RID: 46
	public enum PerTexFloat
	{
		// Token: 0x0400016F RID: 367
		InterpolationContrast = 5,
		// Token: 0x04000170 RID: 368
		NormalStrength = 8,
		// Token: 0x04000171 RID: 369
		Smoothness,
		// Token: 0x04000172 RID: 370
		AO,
		// Token: 0x04000173 RID: 371
		Metallic,
		// Token: 0x04000174 RID: 372
		Brightness,
		// Token: 0x04000175 RID: 373
		Contrast,
		// Token: 0x04000176 RID: 374
		Porosity,
		// Token: 0x04000177 RID: 375
		Foam,
		// Token: 0x04000178 RID: 376
		DetailNoiseStrength,
		// Token: 0x04000179 RID: 377
		DistanceNoiseStrength,
		// Token: 0x0400017A RID: 378
		DistanceResample,
		// Token: 0x0400017B RID: 379
		DisplacementMip,
		// Token: 0x0400017C RID: 380
		GeoTexStrength,
		// Token: 0x0400017D RID: 381
		GeoTintStrength,
		// Token: 0x0400017E RID: 382
		GeoNormalStrength,
		// Token: 0x0400017F RID: 383
		GlobalSmoothMetalAOStength,
		// Token: 0x04000180 RID: 384
		DisplacementStength,
		// Token: 0x04000181 RID: 385
		DisplacementBias,
		// Token: 0x04000182 RID: 386
		DisplacementOffset,
		// Token: 0x04000183 RID: 387
		GlobalEmisStength,
		// Token: 0x04000184 RID: 388
		NoiseNormal0Strength,
		// Token: 0x04000185 RID: 389
		NoiseNormal1Strength,
		// Token: 0x04000186 RID: 390
		NoiseNormal2Strength,
		// Token: 0x04000187 RID: 391
		WindParticulateStrength,
		// Token: 0x04000188 RID: 392
		SnowAmount,
		// Token: 0x04000189 RID: 393
		GlitterAmount,
		// Token: 0x0400018A RID: 394
		GeoHeightFilter,
		// Token: 0x0400018B RID: 395
		GeoHeightFilterStrength,
		// Token: 0x0400018C RID: 396
		TriplanarMode,
		// Token: 0x0400018D RID: 397
		TriplanarContrast,
		// Token: 0x0400018E RID: 398
		StochatsicEnabled,
		// Token: 0x0400018F RID: 399
		Saturation,
		// Token: 0x04000190 RID: 400
		TextureClusterContrast,
		// Token: 0x04000191 RID: 401
		TextureClusterBoost,
		// Token: 0x04000192 RID: 402
		HeightOffset,
		// Token: 0x04000193 RID: 403
		HeightContrast,
		// Token: 0x04000194 RID: 404
		AntiTileArrayNormalStrength = 56,
		// Token: 0x04000195 RID: 405
		AntiTileArrayDetailStrength,
		// Token: 0x04000196 RID: 406
		AntiTileArrayDistanceStrength,
		// Token: 0x04000197 RID: 407
		DisplaceShaping,
		// Token: 0x04000198 RID: 408
		UVRotation = 64,
		// Token: 0x04000199 RID: 409
		TriplanarRotationX,
		// Token: 0x0400019A RID: 410
		TriplanarRotationY,
		// Token: 0x0400019B RID: 411
		FuzzyShadingCore = 68,
		// Token: 0x0400019C RID: 412
		FuzzyShadingEdge,
		// Token: 0x0400019D RID: 413
		FuzzyShadingPower,
		// Token: 0x0400019E RID: 414
		SSSThickness = 75
	}
}
