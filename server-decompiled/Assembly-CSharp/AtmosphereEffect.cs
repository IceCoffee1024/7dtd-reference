using System;

// Token: 0x02000B15 RID: 2837
public class AtmosphereEffect
{
	// Token: 0x060053ED RID: 21485 RVA: 0x00204DF8 File Offset: 0x00202FF8
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

	// Token: 0x040041CF RID: 16847
	public ColorSpectrum[] spectrums = new ColorSpectrum[6];

	// Token: 0x02000B16 RID: 2838
	public enum ESpecIdx
	{
		// Token: 0x040041D1 RID: 16849
		Sky,
		// Token: 0x040041D2 RID: 16850
		Ambient,
		// Token: 0x040041D3 RID: 16851
		Sun,
		// Token: 0x040041D4 RID: 16852
		Moon,
		// Token: 0x040041D5 RID: 16853
		Fog,
		// Token: 0x040041D6 RID: 16854
		FogFade,
		// Token: 0x040041D7 RID: 16855
		Count
	}
}
