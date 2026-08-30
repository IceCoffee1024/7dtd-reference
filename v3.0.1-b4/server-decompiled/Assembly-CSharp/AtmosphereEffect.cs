using System;

// Token: 0x02000AE6 RID: 2790
public class AtmosphereEffect
{
	// Token: 0x06005327 RID: 21287 RVA: 0x001FD9A4 File Offset: 0x001FBBA4
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

	// Token: 0x040040D7 RID: 16599
	public ColorSpectrum[] spectrums = new ColorSpectrum[6];

	// Token: 0x02000AE7 RID: 2791
	public enum ESpecIdx
	{
		// Token: 0x040040D9 RID: 16601
		Sky,
		// Token: 0x040040DA RID: 16602
		Ambient,
		// Token: 0x040040DB RID: 16603
		Sun,
		// Token: 0x040040DC RID: 16604
		Moon,
		// Token: 0x040040DD RID: 16605
		Fog,
		// Token: 0x040040DE RID: 16606
		FogFade,
		// Token: 0x040040DF RID: 16607
		Count
	}
}
