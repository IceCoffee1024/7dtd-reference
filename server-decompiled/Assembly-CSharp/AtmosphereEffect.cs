using System;

// Token: 0x02000B0A RID: 2826
public class AtmosphereEffect
{
	// Token: 0x060053C8 RID: 21448 RVA: 0x00203874 File Offset: 0x00201A74
	public static AtmosphereEffect Load(string _folder, AtmosphereEffect _default)
	{
		AtmosphereEffect atmosphereEffect = new AtmosphereEffect();
		string text = "@:Textures/Environment/Spectrums/" + ((_folder != null) ? (_folder + "/") : "");
		atmosphereEffect.spectrums[0] = ColorSpectrum.FromTexture(text + "sky.tga");
		atmosphereEffect.spectrums[1] = ColorSpectrum.FromTexture(text + "ambient.tga");
		atmosphereEffect.spectrums[2] = ColorSpectrum.FromTexture(text + "sun.tga");
		atmosphereEffect.spectrums[3] = ColorSpectrum.FromTexture(text + "moon.tga");
		atmosphereEffect.spectrums[4] = ColorSpectrum.FromTexture(text + "fog.tga");
		atmosphereEffect.spectrums[5] = ColorSpectrum.FromTexture(text + "fogfade.tga");
		if (_default != null)
		{
			for (int i = 0; i < atmosphereEffect.spectrums.Length; i++)
			{
				if (atmosphereEffect.spectrums[i] == null)
				{
					atmosphereEffect.spectrums[i] = _default.spectrums[i];
				}
			}
		}
		return atmosphereEffect;
	}

	// Token: 0x040041AA RID: 16810
	public ColorSpectrum[] spectrums = new ColorSpectrum[6];

	// Token: 0x02000B0B RID: 2827
	public enum ESpecIdx
	{
		// Token: 0x040041AC RID: 16812
		Sky,
		// Token: 0x040041AD RID: 16813
		Ambient,
		// Token: 0x040041AE RID: 16814
		Sun,
		// Token: 0x040041AF RID: 16815
		Moon,
		// Token: 0x040041B0 RID: 16816
		Fog,
		// Token: 0x040041B1 RID: 16817
		FogFade,
		// Token: 0x040041B2 RID: 16818
		Count
	}
}
