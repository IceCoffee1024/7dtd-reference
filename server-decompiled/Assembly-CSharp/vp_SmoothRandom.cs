using System;
using UnityEngine;

// Token: 0x02001598 RID: 5528
public class vp_SmoothRandom
{
	// Token: 0x0600ACA5 RID: 44197 RVA: 0x00405630 File Offset: 0x00403830
	public static Vector3 GetVector3(float speed)
	{
		float num = Time.time * 0.01f * speed;
		return new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 0.2f, 0.58f));
	}

	// Token: 0x0600ACA6 RID: 44198 RVA: 0x00405690 File Offset: 0x00403890
	public static Vector3 GetVector3Centered(float speed)
	{
		float num = Time.time * 0.01f * speed;
		float num2 = (Time.time - 1f) * 0.01f * speed;
		Vector3 vector = new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 0.2f, 0.58f));
		Vector3 vector2 = new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num2, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num2, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num2, 0.2f, 0.58f));
		return vector - vector2;
	}

	// Token: 0x0600ACA7 RID: 44199 RVA: 0x00405750 File Offset: 0x00403950
	public static Vector3 GetVector3Centered(float time, float speed)
	{
		float num = time * 0.01f * speed;
		float num2 = (time - 1f) * 0.01f * speed;
		Vector3 vector = new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 0.2f, 0.58f));
		Vector3 vector2 = new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num2, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num2, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num2, 0.2f, 0.58f));
		return vector - vector2;
	}

	// Token: 0x0600ACA8 RID: 44200 RVA: 0x00405808 File Offset: 0x00403A08
	public static float Get(float speed)
	{
		float num = Time.time * 0.01f * speed;
		return vp_SmoothRandom.Get().HybridMultifractal(num * 0.01f, 15.7f, 0.65f);
	}

	// Token: 0x0600ACA9 RID: 44201 RVA: 0x0040583E File Offset: 0x00403A3E
	[PublicizedFrom(EAccessModifier.Private)]
	public static vp_FractalNoise Get()
	{
		if (vp_SmoothRandom.s_Noise == null)
		{
			vp_SmoothRandom.s_Noise = new vp_FractalNoise(1.27f, 2.04f, 8.36f);
		}
		return vp_SmoothRandom.s_Noise;
	}

	// Token: 0x04007FBE RID: 32702
	[PublicizedFrom(EAccessModifier.Private)]
	public static vp_FractalNoise s_Noise;
}
