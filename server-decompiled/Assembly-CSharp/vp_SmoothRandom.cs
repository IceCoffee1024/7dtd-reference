using System;
using UnityEngine;

// Token: 0x02001576 RID: 5494
public class vp_SmoothRandom
{
	// Token: 0x0600ABF2 RID: 44018 RVA: 0x004007DC File Offset: 0x003FE9DC
	public static Vector3 GetVector3(float speed)
	{
		float num = Time.time * 0.01f * speed;
		return new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 0.2f, 0.58f));
	}

	// Token: 0x0600ABF3 RID: 44019 RVA: 0x0040083C File Offset: 0x003FEA3C
	public static Vector3 GetVector3Centered(float speed)
	{
		float num = Time.time * 0.01f * speed;
		float num2 = (Time.time - 1f) * 0.01f * speed;
		Vector3 vector = new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 0.2f, 0.58f));
		Vector3 vector2 = new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num2, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num2, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num2, 0.2f, 0.58f));
		return vector - vector2;
	}

	// Token: 0x0600ABF4 RID: 44020 RVA: 0x004008FC File Offset: 0x003FEAFC
	public static Vector3 GetVector3Centered(float time, float speed)
	{
		float num = time * 0.01f * speed;
		float num2 = (time - 1f) * 0.01f * speed;
		Vector3 vector = new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 0.2f, 0.58f));
		Vector3 vector2 = new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num2, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num2, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num2, 0.2f, 0.58f));
		return vector - vector2;
	}

	// Token: 0x0600ABF5 RID: 44021 RVA: 0x004009B4 File Offset: 0x003FEBB4
	public static float Get(float speed)
	{
		float num = Time.time * 0.01f * speed;
		return vp_SmoothRandom.Get().HybridMultifractal(num * 0.01f, 15.7f, 0.65f);
	}

	// Token: 0x0600ABF6 RID: 44022 RVA: 0x004009EA File Offset: 0x003FEBEA
	[PublicizedFrom(EAccessModifier.Private)]
	public static vp_FractalNoise Get()
	{
		if (vp_SmoothRandom.s_Noise == null)
		{
			vp_SmoothRandom.s_Noise = new vp_FractalNoise(1.27f, 2.04f, 8.36f);
		}
		return vp_SmoothRandom.s_Noise;
	}

	// Token: 0x04007F3B RID: 32571
	[PublicizedFrom(EAccessModifier.Private)]
	public static vp_FractalNoise s_Noise;
}
