using System;
using UnityEngine;

// Token: 0x020015AA RID: 5546
public class vp_SmoothRandom
{
	// Token: 0x0600ACFD RID: 44285 RVA: 0x00407A34 File Offset: 0x00405C34
	public static Vector3 GetVector3(float speed)
	{
		float num = Time.time * 0.01f * speed;
		return new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 0.2f, 0.58f));
	}

	// Token: 0x0600ACFE RID: 44286 RVA: 0x00407A94 File Offset: 0x00405C94
	public static Vector3 GetVector3Centered(float speed)
	{
		float num = Time.time * 0.01f * speed;
		float num2 = (Time.time - 1f) * 0.01f * speed;
		Vector3 vector = new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 0.2f, 0.58f));
		Vector3 vector2 = new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num2, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num2, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num2, 0.2f, 0.58f));
		return vector - vector2;
	}

	// Token: 0x0600ACFF RID: 44287 RVA: 0x00407B54 File Offset: 0x00405D54
	public static Vector3 GetVector3Centered(float time, float speed)
	{
		float num = time * 0.01f * speed;
		float num2 = (time - 1f) * 0.01f * speed;
		Vector3 vector = new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 0.2f, 0.58f));
		Vector3 vector2 = new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num2, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num2, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num2, 0.2f, 0.58f));
		return vector - vector2;
	}

	// Token: 0x0600AD00 RID: 44288 RVA: 0x00407C0C File Offset: 0x00405E0C
	public static float Get(float speed)
	{
		float num = Time.time * 0.01f * speed;
		return vp_SmoothRandom.Get().HybridMultifractal(num * 0.01f, 15.7f, 0.65f);
	}

	// Token: 0x0600AD01 RID: 44289 RVA: 0x00407C42 File Offset: 0x00405E42
	[PublicizedFrom(EAccessModifier.Private)]
	public static vp_FractalNoise Get()
	{
		if (vp_SmoothRandom.s_Noise == null)
		{
			vp_SmoothRandom.s_Noise = new vp_FractalNoise(1.27f, 2.04f, 8.36f);
		}
		return vp_SmoothRandom.s_Noise;
	}

	// Token: 0x04007FFF RID: 32767
	[PublicizedFrom(EAccessModifier.Private)]
	public static vp_FractalNoise s_Noise;
}
