using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000033 RID: 51
public class MicroSplatProceduralTextureConfig : ScriptableObject
{
	// Token: 0x06000147 RID: 327 RVA: 0x0000DAC4 File Offset: 0x0000BCC4
	public void ResetToDefault()
	{
		this.layers = new List<MicroSplatProceduralTextureConfig.Layer>(3);
		this.layers.Add(new MicroSplatProceduralTextureConfig.Layer());
		this.layers.Add(new MicroSplatProceduralTextureConfig.Layer());
		this.layers.Add(new MicroSplatProceduralTextureConfig.Layer());
		this.layers[1].textureIndex = 1;
		this.layers[1].slopeActive = true;
		this.layers[1].slopeCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0.03f, 0f),
			new Keyframe(0.06f, 1f),
			new Keyframe(0.16f, 1f),
			new Keyframe(0.2f, 0f)
		});
		this.layers[0].slopeActive = true;
		this.layers[0].textureIndex = 2;
		this.layers[0].slopeCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0.13f, 0f),
			new Keyframe(0.25f, 1f)
		});
	}

	// Token: 0x06000148 RID: 328 RVA: 0x0000DC14 File Offset: 0x0000BE14
	public Texture2D GetHeightGradientTexture()
	{
		int num = 32;
		int num2 = 128;
		if (this.heightGradientTex == null)
		{
			this.heightGradientTex = new Texture2D(num2, num, TextureFormat.RGBA32, false);
			this.heightGradientTex.hideFlags = HideFlags.HideAndDontSave;
		}
		Color grey = Color.grey;
		for (int i = 0; i < this.heightGradients.Count; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				float num3 = (float)j / (float)num2;
				Color color = this.heightGradients[i].Evaluate(num3);
				this.heightGradientTex.SetPixel(j, i, color);
			}
		}
		for (int k = this.heightGradients.Count; k < 32; k++)
		{
			for (int l = 0; l < num2; l++)
			{
				this.heightGradientTex.SetPixel(l, k, grey);
			}
		}
		this.heightGradientTex.Apply(false, false);
		return this.heightGradientTex;
	}

	// Token: 0x06000149 RID: 329 RVA: 0x0000DD00 File Offset: 0x0000BF00
	public Texture2D GetHeightHSVTexture()
	{
		int num = 32;
		int num2 = 128;
		if (this.heightHSVTex == null)
		{
			this.heightHSVTex = new Texture2D(num2, num, TextureFormat.RGBA32, false);
			this.heightHSVTex.hideFlags = HideFlags.HideAndDontSave;
		}
		Color grey = Color.grey;
		for (int i = 0; i < this.heightHSV.Count; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				Color color = grey;
				float num3 = (float)j / (float)num2;
				color.r = this.heightHSV[i].H.Evaluate(num3) * 0.5f + 0.5f;
				color.g = this.heightHSV[i].S.Evaluate(num3) * 0.5f + 0.5f;
				color.b = this.heightHSV[i].V.Evaluate(num3) * 0.5f + 0.5f;
				this.heightHSVTex.SetPixel(j, i, color);
			}
		}
		for (int k = this.heightHSV.Count; k < 32; k++)
		{
			for (int l = 0; l < num2; l++)
			{
				this.heightHSVTex.SetPixel(l, k, grey);
			}
		}
		this.heightHSVTex.Apply(false, false);
		return this.heightHSVTex;
	}

	// Token: 0x0600014A RID: 330 RVA: 0x0000DE64 File Offset: 0x0000C064
	public Texture2D GetSlopeGradientTexture()
	{
		int num = 32;
		int num2 = 128;
		if (this.slopeGradientTex == null)
		{
			this.slopeGradientTex = new Texture2D(num2, num, TextureFormat.RGBA32, false);
			this.slopeGradientTex.hideFlags = HideFlags.HideAndDontSave;
		}
		Color grey = Color.grey;
		for (int i = 0; i < this.slopeGradients.Count; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				float num3 = (float)j / (float)num2;
				Color color = this.slopeGradients[i].Evaluate(num3);
				this.slopeGradientTex.SetPixel(j, i, color);
			}
		}
		for (int k = this.slopeGradients.Count; k < 32; k++)
		{
			for (int l = 0; l < num2; l++)
			{
				this.slopeGradientTex.SetPixel(l, k, grey);
			}
		}
		this.slopeGradientTex.Apply(false, false);
		return this.slopeGradientTex;
	}

	// Token: 0x0600014B RID: 331 RVA: 0x0000DF50 File Offset: 0x0000C150
	public Texture2D GetSlopeHSVTexture()
	{
		int num = 32;
		int num2 = 128;
		if (this.slopeHSVTex == null)
		{
			this.slopeHSVTex = new Texture2D(num2, num, TextureFormat.RGBA32, false);
			this.slopeHSVTex.hideFlags = HideFlags.HideAndDontSave;
		}
		Color grey = Color.grey;
		for (int i = 0; i < this.slopeHSV.Count; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				Color color = grey;
				float num3 = (float)j / (float)num2;
				color.r = this.slopeHSV[i].H.Evaluate(num3) * 0.5f + 0.5f;
				color.g = this.slopeHSV[i].S.Evaluate(num3) * 0.5f + 0.5f;
				color.b = this.slopeHSV[i].V.Evaluate(num3) * 0.5f + 0.5f;
				this.slopeHSVTex.SetPixel(j, i, color);
			}
		}
		for (int k = this.slopeHSV.Count; k < 32; k++)
		{
			for (int l = 0; l < num2; l++)
			{
				this.slopeHSVTex.SetPixel(l, k, grey);
			}
		}
		this.slopeHSVTex.Apply(false, false);
		return this.slopeHSVTex;
	}

	// Token: 0x0600014C RID: 332 RVA: 0x0000E0B4 File Offset: 0x0000C2B4
	[PublicizedFrom(EAccessModifier.Private)]
	public float CompFilter(MicroSplatProceduralTextureConfig.Layer.Filter f, MicroSplatProceduralTextureConfig.Layer.CurveMode mode, float v)
	{
		float num = Mathf.Abs(v - f.center) * (1f / Mathf.Max(f.width, 0.0001f));
		num = Mathf.Clamp01(Mathf.Pow(num, f.contrast));
		switch (mode)
		{
		case MicroSplatProceduralTextureConfig.Layer.CurveMode.BoostFilter:
			return 1f - num;
		case MicroSplatProceduralTextureConfig.Layer.CurveMode.HighPass:
			if (v >= f.center)
			{
				return 1f;
			}
			return 1f - num;
		case MicroSplatProceduralTextureConfig.Layer.CurveMode.LowPass:
			if (v <= f.center)
			{
				return 1f;
			}
			return 1f - num;
		case MicroSplatProceduralTextureConfig.Layer.CurveMode.CutFilter:
			return num;
		default:
			Debug.LogError("Unhandled case in ProceduralTextureConfig::CompFilter");
			return 0f;
		}
	}

	// Token: 0x0600014D RID: 333 RVA: 0x0000E15C File Offset: 0x0000C35C
	public Texture2D GetCurveTexture()
	{
		int num = 32;
		int num2 = (int)this.proceduralCurveTextureSize;
		if (this.curveTex != null && this.curveTex.width != num2)
		{
			global::UnityEngine.Object.DestroyImmediate(this.curveTex);
			this.curveTex = null;
		}
		if (this.curveTex == null)
		{
			this.curveTex = new Texture2D(num2, num, TextureFormat.RGBA32, false, true);
			this.curveTex.hideFlags = HideFlags.HideAndDontSave;
		}
		Color white = Color.white;
		for (int i = 0; i < this.layers.Count; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				Color color = white;
				float num3 = (float)j / (float)num2;
				if (this.layers[i].heightActive)
				{
					if (this.layers[i].heightCurveMode == MicroSplatProceduralTextureConfig.Layer.CurveMode.Curve)
					{
						color.r = this.layers[i].heightCurve.Evaluate(num3);
					}
					else
					{
						color.r = this.CompFilter(this.layers[i].heightFilter, this.layers[i].heightCurveMode, num3);
					}
				}
				if (this.layers[i].slopeActive)
				{
					if (this.layers[i].slopeCurveMode == MicroSplatProceduralTextureConfig.Layer.CurveMode.Curve)
					{
						color.g = this.layers[i].slopeCurve.Evaluate(num3);
					}
					else
					{
						color.g = this.CompFilter(this.layers[i].slopeFilter, this.layers[i].slopeCurveMode, num3);
					}
				}
				if (this.layers[i].cavityMapActive)
				{
					if (this.layers[i].cavityCurveMode == MicroSplatProceduralTextureConfig.Layer.CurveMode.Curve)
					{
						color.b = this.layers[i].cavityMapCurve.Evaluate(num3);
					}
					else
					{
						color.b = this.CompFilter(this.layers[i].cavityMapFilter, this.layers[i].cavityCurveMode, num3);
					}
				}
				if (this.layers[i].erosionMapActive)
				{
					if (this.layers[i].erosionCurveMode == MicroSplatProceduralTextureConfig.Layer.CurveMode.Curve)
					{
						color.a = this.layers[i].erosionMapCurve.Evaluate(num3);
					}
					else
					{
						color.a = this.CompFilter(this.layers[i].erosionFilter, this.layers[i].erosionCurveMode, num3);
					}
				}
				this.curveTex.SetPixel(j, i, color);
			}
		}
		this.curveTex.Apply(false, false);
		return this.curveTex;
	}

	// Token: 0x0600014E RID: 334 RVA: 0x0000E414 File Offset: 0x0000C614
	public Texture2D GetParamTexture()
	{
		int num = 32;
		int num2 = 4;
		if (this.paramTex == null || this.paramTex.format != TextureFormat.RGBAHalf || this.paramTex.width != num2)
		{
			this.paramTex = new Texture2D(num2, num, TextureFormat.RGBAHalf, false, true);
			this.paramTex.hideFlags = HideFlags.HideAndDontSave;
		}
		Color color = new Color(0f, 0f, 0f, 0f);
		for (int i = 0; i < this.layers.Count; i++)
		{
			Color color2 = color;
			Color color3 = color;
			if (this.layers[i].noiseActive)
			{
				color2.r = this.layers[i].noiseFrequency;
				color2.g = this.layers[i].noiseRange.x;
				color2.b = this.layers[i].noiseRange.y;
				color2.a = this.layers[i].noiseOffset;
			}
			color3.r = this.layers[i].weight;
			color3.g = (float)this.layers[i].textureIndex;
			this.paramTex.SetPixel(0, i, color2);
			this.paramTex.SetPixel(1, i, color3);
			Vector4 biomeWeights = this.layers[i].biomeWeights;
			this.paramTex.SetPixel(2, i, new Color(biomeWeights.x, biomeWeights.y, biomeWeights.z, biomeWeights.w));
			Vector4 biomeWeights2 = this.layers[i].biomeWeights2;
			this.paramTex.SetPixel(3, i, new Color(biomeWeights2.x, biomeWeights2.y, biomeWeights2.z, biomeWeights2.w));
		}
		this.paramTex.Apply(false, false);
		return this.paramTex;
	}

	// Token: 0x040001B0 RID: 432
	public MicroSplatProceduralTextureConfig.TableSize proceduralCurveTextureSize = MicroSplatProceduralTextureConfig.TableSize.k256;

	// Token: 0x040001B1 RID: 433
	public List<Gradient> heightGradients = new List<Gradient>();

	// Token: 0x040001B2 RID: 434
	public List<MicroSplatProceduralTextureConfig.HSVCurve> heightHSV = new List<MicroSplatProceduralTextureConfig.HSVCurve>();

	// Token: 0x040001B3 RID: 435
	public List<Gradient> slopeGradients = new List<Gradient>();

	// Token: 0x040001B4 RID: 436
	public List<MicroSplatProceduralTextureConfig.HSVCurve> slopeHSV = new List<MicroSplatProceduralTextureConfig.HSVCurve>();

	// Token: 0x040001B5 RID: 437
	[HideInInspector]
	public List<MicroSplatProceduralTextureConfig.Layer> layers = new List<MicroSplatProceduralTextureConfig.Layer>();

	// Token: 0x040001B6 RID: 438
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D curveTex;

	// Token: 0x040001B7 RID: 439
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D paramTex;

	// Token: 0x040001B8 RID: 440
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D heightGradientTex;

	// Token: 0x040001B9 RID: 441
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D heightHSVTex;

	// Token: 0x040001BA RID: 442
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D slopeGradientTex;

	// Token: 0x040001BB RID: 443
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Texture2D slopeHSVTex;

	// Token: 0x02000034 RID: 52
	public enum TableSize
	{
		// Token: 0x040001BD RID: 445
		k64 = 64,
		// Token: 0x040001BE RID: 446
		k128 = 128,
		// Token: 0x040001BF RID: 447
		k256 = 256,
		// Token: 0x040001C0 RID: 448
		k512 = 512,
		// Token: 0x040001C1 RID: 449
		k1024 = 1024,
		// Token: 0x040001C2 RID: 450
		k2048 = 2048,
		// Token: 0x040001C3 RID: 451
		k4096 = 4096
	}

	// Token: 0x02000035 RID: 53
	[Serializable]
	public class Layer
	{
		// Token: 0x06000150 RID: 336 RVA: 0x0000E668 File Offset: 0x0000C868
		public MicroSplatProceduralTextureConfig.Layer Copy()
		{
			return new MicroSplatProceduralTextureConfig.Layer
			{
				weight = this.weight,
				textureIndex = this.textureIndex,
				noiseActive = this.noiseActive,
				noiseFrequency = this.noiseFrequency,
				noiseOffset = this.noiseOffset,
				noiseRange = this.noiseRange,
				biomeWeights = this.biomeWeights,
				biomeWeights2 = this.biomeWeights2,
				heightActive = this.heightActive,
				slopeActive = this.slopeActive,
				erosionMapActive = this.erosionMapActive,
				cavityMapActive = this.cavityMapActive,
				heightCurve = new AnimationCurve(this.heightCurve.keys),
				slopeCurve = new AnimationCurve(this.slopeCurve.keys),
				erosionMapCurve = new AnimationCurve(this.erosionMapCurve.keys),
				cavityMapCurve = new AnimationCurve(this.cavityMapCurve.keys),
				cavityMapFilter = this.cavityMapFilter,
				heightFilter = this.heightFilter,
				slopeFilter = this.slopeFilter,
				erosionFilter = this.erosionFilter,
				heightCurveMode = this.heightCurveMode,
				slopeCurveMode = this.slopeCurveMode,
				erosionCurveMode = this.erosionCurveMode,
				cavityCurveMode = this.cavityCurveMode
			};
		}

		// Token: 0x040001C4 RID: 452
		public float weight = 1f;

		// Token: 0x040001C5 RID: 453
		public int textureIndex;

		// Token: 0x040001C6 RID: 454
		public bool noiseActive;

		// Token: 0x040001C7 RID: 455
		public float noiseFrequency = 1f;

		// Token: 0x040001C8 RID: 456
		public float noiseOffset;

		// Token: 0x040001C9 RID: 457
		public Vector2 noiseRange = new Vector2(0f, 1f);

		// Token: 0x040001CA RID: 458
		public Vector4 biomeWeights = new Vector4(1f, 1f, 1f, 1f);

		// Token: 0x040001CB RID: 459
		public Vector4 biomeWeights2 = new Vector4(1f, 1f, 1f, 1f);

		// Token: 0x040001CC RID: 460
		public bool heightActive;

		// Token: 0x040001CD RID: 461
		public AnimationCurve heightCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x040001CE RID: 462
		public MicroSplatProceduralTextureConfig.Layer.Filter heightFilter = new MicroSplatProceduralTextureConfig.Layer.Filter();

		// Token: 0x040001CF RID: 463
		public bool slopeActive;

		// Token: 0x040001D0 RID: 464
		public AnimationCurve slopeCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x040001D1 RID: 465
		public MicroSplatProceduralTextureConfig.Layer.Filter slopeFilter = new MicroSplatProceduralTextureConfig.Layer.Filter();

		// Token: 0x040001D2 RID: 466
		public bool erosionMapActive;

		// Token: 0x040001D3 RID: 467
		public AnimationCurve erosionMapCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x040001D4 RID: 468
		public MicroSplatProceduralTextureConfig.Layer.Filter erosionFilter = new MicroSplatProceduralTextureConfig.Layer.Filter();

		// Token: 0x040001D5 RID: 469
		public bool cavityMapActive;

		// Token: 0x040001D6 RID: 470
		public AnimationCurve cavityMapCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x040001D7 RID: 471
		public MicroSplatProceduralTextureConfig.Layer.Filter cavityMapFilter = new MicroSplatProceduralTextureConfig.Layer.Filter();

		// Token: 0x040001D8 RID: 472
		public MicroSplatProceduralTextureConfig.Layer.CurveMode heightCurveMode;

		// Token: 0x040001D9 RID: 473
		public MicroSplatProceduralTextureConfig.Layer.CurveMode slopeCurveMode;

		// Token: 0x040001DA RID: 474
		public MicroSplatProceduralTextureConfig.Layer.CurveMode erosionCurveMode;

		// Token: 0x040001DB RID: 475
		public MicroSplatProceduralTextureConfig.Layer.CurveMode cavityCurveMode;

		// Token: 0x02000036 RID: 54
		[Serializable]
		public class Filter
		{
			// Token: 0x040001DC RID: 476
			public float center = 0.5f;

			// Token: 0x040001DD RID: 477
			public float width = 0.1f;

			// Token: 0x040001DE RID: 478
			public float contrast = 1f;
		}

		// Token: 0x02000037 RID: 55
		public enum CurveMode
		{
			// Token: 0x040001E0 RID: 480
			Curve,
			// Token: 0x040001E1 RID: 481
			BoostFilter,
			// Token: 0x040001E2 RID: 482
			HighPass,
			// Token: 0x040001E3 RID: 483
			LowPass,
			// Token: 0x040001E4 RID: 484
			CutFilter
		}
	}

	// Token: 0x02000038 RID: 56
	[Serializable]
	public class HSVCurve
	{
		// Token: 0x040001E5 RID: 485
		public AnimationCurve H = AnimationCurve.Linear(0f, 0.5f, 1f, 0.5f);

		// Token: 0x040001E6 RID: 486
		public AnimationCurve S = AnimationCurve.Linear(0f, 0f, 1f, 0f);

		// Token: 0x040001E7 RID: 487
		public AnimationCurve V = AnimationCurve.Linear(0f, 0f, 1f, 0f);
	}
}
