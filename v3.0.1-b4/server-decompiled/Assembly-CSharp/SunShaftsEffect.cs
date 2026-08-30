using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x0200006D RID: 109
[PostProcess(typeof(SunShaftsEffectRenderer), PostProcessEvent.AfterStack, "Custom/Sun Shafts", false)]
[Serializable]
public sealed class SunShaftsEffect : PostProcessEffectSettings
{
	// Token: 0x06000227 RID: 551 RVA: 0x0001220C File Offset: 0x0001040C
	public override bool IsEnabledAndSupported(PostProcessRenderContext context)
	{
		if (this.enabled.value && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
		{
			Shader value = this.sunShaftsShader.value;
			if (value != null && value.isSupported)
			{
				Shader value2 = this.simpleClearShader.value;
				return value2 != null && value2.isSupported;
			}
		}
		return false;
	}

	// Token: 0x06000228 RID: 552 RVA: 0x00012260 File Offset: 0x00010460
	public SunShaftsEffect.SunSettings GetSunSettings()
	{
		return new SunShaftsEffect.SunSettings
		{
			sunColor = this.sunColor.value,
			sunThreshold = this.sunThreshold.value,
			sunPosition = this.sunPosition.value,
			sunShaftIntensity = this.sunShaftIntensity.value
		};
	}

	// Token: 0x040002D5 RID: 725
	public SunShaftsEffect.SunShaftsResolutionParameter resolution = new SunShaftsEffect.SunShaftsResolutionParameter
	{
		value = SunShaftsEffect.SunShaftsResolution.Normal
	};

	// Token: 0x040002D6 RID: 726
	public SunShaftsEffect.ShaftsScreenBlendModeParameter screenBlendMode = new SunShaftsEffect.ShaftsScreenBlendModeParameter
	{
		value = SunShaftsEffect.ShaftsScreenBlendMode.Screen
	};

	// Token: 0x040002D7 RID: 727
	public BoolParameter autoUpdateSun = new BoolParameter
	{
		value = true
	};

	// Token: 0x040002D8 RID: 728
	public Vector3Parameter sunPosition = new Vector3Parameter();

	// Token: 0x040002D9 RID: 729
	[Range(1f, 4f)]
	public IntParameter radialBlurIterations = new IntParameter
	{
		value = 2
	};

	// Token: 0x040002DA RID: 730
	public ColorParameter sunColor = new ColorParameter
	{
		value = Color.white
	};

	// Token: 0x040002DB RID: 731
	public ColorParameter sunThreshold = new ColorParameter
	{
		value = new Color(0.87f, 0.74f, 0.65f)
	};

	// Token: 0x040002DC RID: 732
	public FloatParameter sunShaftBlurRadius = new FloatParameter
	{
		value = 2.5f
	};

	// Token: 0x040002DD RID: 733
	public FloatParameter sunShaftIntensity = new FloatParameter
	{
		value = 1.15f
	};

	// Token: 0x040002DE RID: 734
	public FloatParameter maxRadius = new FloatParameter
	{
		value = 0.75f
	};

	// Token: 0x040002DF RID: 735
	public SunShaftsEffect.ShaderParameter sunShaftsShader = new SunShaftsEffect.ShaderParameter();

	// Token: 0x040002E0 RID: 736
	public SunShaftsEffect.ShaderParameter simpleClearShader = new SunShaftsEffect.ShaderParameter();

	// Token: 0x0200006E RID: 110
	public struct SunSettings
	{
		// Token: 0x040002E1 RID: 737
		public Color sunColor;

		// Token: 0x040002E2 RID: 738
		public Color sunThreshold;

		// Token: 0x040002E3 RID: 739
		public Vector3 sunPosition;

		// Token: 0x040002E4 RID: 740
		public float sunShaftIntensity;
	}

	// Token: 0x0200006F RID: 111
	public enum SunShaftsResolution
	{
		// Token: 0x040002E6 RID: 742
		Low,
		// Token: 0x040002E7 RID: 743
		Normal,
		// Token: 0x040002E8 RID: 744
		High
	}

	// Token: 0x02000070 RID: 112
	public enum ShaftsScreenBlendMode
	{
		// Token: 0x040002EA RID: 746
		Screen,
		// Token: 0x040002EB RID: 747
		Add
	}

	// Token: 0x02000071 RID: 113
	[Serializable]
	public sealed class SunShaftsResolutionParameter : ParameterOverride<SunShaftsEffect.SunShaftsResolution>
	{
	}

	// Token: 0x02000072 RID: 114
	[Serializable]
	public sealed class ShaftsScreenBlendModeParameter : ParameterOverride<SunShaftsEffect.ShaftsScreenBlendMode>
	{
	}

	// Token: 0x02000073 RID: 115
	[Serializable]
	public sealed class ShaderParameter : ParameterOverride<Shader>
	{
	}
}
