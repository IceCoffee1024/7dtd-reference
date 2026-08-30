using System;
using UnityEngine;

// Token: 0x0200155B RID: 5467
public class vp_SmoothRandom
{
	// Token: 0x0600AAF5 RID: 43765 RVA: 0x003FB49C File Offset: 0x003F969C
	public static Vector3 GetVector3(float speed)
	{
		float num = Time.time * 0.01f * speed;
		return new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 0.2f, 0.58f));
	}

	// Token: 0x0600AAF6 RID: 43766 RVA: 0x003FB4FC File Offset: 0x003F96FC
	public static Vector3 GetVector3Centered(float speed)
	{
		float num = Time.time * 0.01f * speed;
		float num2 = (Time.time - 1f) * 0.01f * speed;
		Vector3 vector = new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 0.2f, 0.58f));
		Vector3 vector2 = new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num2, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num2, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num2, 0.2f, 0.58f));
		return vector - vector2;
	}

	// Token: 0x0600AAF7 RID: 43767 RVA: 0x003FB5BC File Offset: 0x003F97BC
	public static Vector3 GetVector3Centered(float time, float speed)
	{
		float num = time * 0.01f * speed;
		float num2 = (time - 1f) * 0.01f * speed;
		Vector3 vector = new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num, 0.2f, 0.58f));
		Vector3 vector2 = new Vector3(vp_SmoothRandom.Get().HybridMultifractal(num2, 15.73f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num2, 63.94f, 0.58f), vp_SmoothRandom.Get().HybridMultifractal(num2, 0.2f, 0.58f));
		return vector - vector2;
	}

	// Token: 0x0600AAF8 RID: 43768 RVA: 0x003FB674 File Offset: 0x003F9874
	public static float Get(float speed)
	{
		float num = Time.time * 0.01f * speed;
		return vp_SmoothRandom.Get().HybridMultifractal(num * 0.01f, 15.7f, 0.65f);
	}

	// Token: 0x0600AAF9 RID: 43769 RVA: 0x003FB6AA File Offset: 0x003F98AA
	[PublicizedFrom(EAccessModifier.Private)]
	public static vp_FractalNoise Get()
	{
		if (vp_SmoothRandom.s_Noise == null)
		{
			vp_SmoothRandom.s_Noise = new vp_FractalNoise(1.27f, 2.04f, 8.36f);
		}
		return vp_SmoothRandom.s_Noise;
	}

	// Token: 0x04007E62 RID: 32354
	[PublicizedFrom(EAccessModifier.Private)]
	public static vp_FractalNoise s_Noise;
}
