using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001389 RID: 5001
public class ParticleSystemSpawnRateLimiter : MonoBehaviour
{
	// Token: 0x06009D29 RID: 40233 RVA: 0x003B62C4 File Offset: 0x003B44C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		float num = GamePrefs.GetFloat(EnumGamePrefs.OptionsGfxWaterPtlLimiter);
		num = Utils.FastMax(0.15f, num);
		foreach (ParticleSystem particleSystem in base.GetComponentsInChildren<ParticleSystem>())
		{
			ParticleSystem.EmissionModule emission = particleSystem.emission;
			ParticleSystem.MinMaxCurve rateOverTime = emission.rateOverTime;
			ParticleSystemSpawnRateLimiter.EmissionRateData emissionRateData = new ParticleSystemSpawnRateLimiter.EmissionRateData
			{
				mode = rateOverTime.mode,
				curveMultiplier = rateOverTime.curveMultiplier,
				originalBursts = new ParticleSystem.Burst[emission.burstCount]
			};
			emission.GetBursts(emissionRateData.originalBursts);
			switch (rateOverTime.mode)
			{
			case ParticleSystemCurveMode.Constant:
				emissionRateData.constant = rateOverTime.constant;
				break;
			case ParticleSystemCurveMode.Curve:
				emissionRateData.curve = new AnimationCurve(rateOverTime.curve.keys);
				break;
			case ParticleSystemCurveMode.TwoCurves:
				emissionRateData.curveMin = new AnimationCurve(rateOverTime.curveMin.keys);
				emissionRateData.curveMax = new AnimationCurve(rateOverTime.curveMax.keys);
				break;
			case ParticleSystemCurveMode.TwoConstants:
				emissionRateData.constantMin = rateOverTime.constantMin;
				emissionRateData.constantMax = rateOverTime.constantMax;
				break;
			}
			this.originalRates[particleSystem] = emissionRateData;
			this.ScaleEmissionRate(particleSystem, num);
		}
		GameOptionsManager.OnGameOptionsApplied += this.OnGameOptionsApplied;
	}

	// Token: 0x06009D2A RID: 40234 RVA: 0x003B641B File Offset: 0x003B461B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		GameOptionsManager.OnGameOptionsApplied -= this.OnGameOptionsApplied;
	}

	// Token: 0x06009D2B RID: 40235 RVA: 0x003B6430 File Offset: 0x003B4630
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGameOptionsApplied()
	{
		float num = GamePrefs.GetFloat(EnumGamePrefs.OptionsGfxWaterPtlLimiter);
		num = Utils.FastMax(0.15f, num);
		foreach (ParticleSystem particleSystem in base.GetComponentsInChildren<ParticleSystem>())
		{
			if (this.originalRates.ContainsKey(particleSystem))
			{
				this.ScaleEmissionRate(particleSystem, num);
			}
		}
	}

	// Token: 0x06009D2C RID: 40236 RVA: 0x003B6484 File Offset: 0x003B4684
	[PublicizedFrom(EAccessModifier.Private)]
	public void ScaleEmissionRate(ParticleSystem ps, float scale)
	{
		if (!this.originalRates.ContainsKey(ps))
		{
			return;
		}
		ParticleSystem.EmissionModule emission = ps.emission;
		ParticleSystemSpawnRateLimiter.EmissionRateData emissionRateData = this.originalRates[ps];
		ParticleSystem.MinMaxCurve minMaxCurve = default(ParticleSystem.MinMaxCurve);
		switch (emissionRateData.mode)
		{
		case ParticleSystemCurveMode.Constant:
			minMaxCurve = new ParticleSystem.MinMaxCurve(emissionRateData.constant * scale);
			break;
		case ParticleSystemCurveMode.Curve:
			minMaxCurve = new ParticleSystem.MinMaxCurve(emissionRateData.curveMultiplier * scale, emissionRateData.curve);
			break;
		case ParticleSystemCurveMode.TwoCurves:
			minMaxCurve = new ParticleSystem.MinMaxCurve(emissionRateData.curveMultiplier * scale, emissionRateData.curveMin, emissionRateData.curveMax);
			break;
		case ParticleSystemCurveMode.TwoConstants:
			minMaxCurve = new ParticleSystem.MinMaxCurve(emissionRateData.constantMin * scale, emissionRateData.constantMax * scale);
			break;
		}
		emission.rateOverTime = minMaxCurve;
		if (emissionRateData.originalBursts != null && emissionRateData.originalBursts.Length != 0)
		{
			ParticleSystem.Burst[] array = new ParticleSystem.Burst[emissionRateData.originalBursts.Length];
			for (int i = 0; i < emissionRateData.originalBursts.Length; i++)
			{
				ParticleSystem.Burst burst = emissionRateData.originalBursts[i];
				array[i] = new ParticleSystem.Burst(burst.time, ParticleSystemSpawnRateLimiter.ScaleBurstCurve(burst.count, scale), burst.cycleCount, burst.repeatInterval);
			}
			emission.SetBursts(array);
		}
	}

	// Token: 0x06009D2D RID: 40237 RVA: 0x003B65C0 File Offset: 0x003B47C0
	[PublicizedFrom(EAccessModifier.Private)]
	public static ParticleSystem.MinMaxCurve ScaleBurstCurve(ParticleSystem.MinMaxCurve original, float scale)
	{
		ParticleSystem.MinMaxCurve minMaxCurve = default(ParticleSystem.MinMaxCurve);
		minMaxCurve.mode = original.mode;
		switch (original.mode)
		{
		case ParticleSystemCurveMode.Constant:
			minMaxCurve.constant = ParticleSystemSpawnRateLimiter.ScaleBurstConstant(original.constant, scale);
			break;
		case ParticleSystemCurveMode.Curve:
			minMaxCurve.curveMultiplier = original.curveMultiplier * scale;
			minMaxCurve.curve = original.curve;
			break;
		case ParticleSystemCurveMode.TwoCurves:
			minMaxCurve.curveMultiplier = original.curveMultiplier * scale;
			minMaxCurve.curveMin = original.curveMin;
			minMaxCurve.curveMax = original.curveMax;
			break;
		case ParticleSystemCurveMode.TwoConstants:
			minMaxCurve.constantMin = ParticleSystemSpawnRateLimiter.ScaleBurstConstant(original.constantMin, scale);
			minMaxCurve.constantMax = ParticleSystemSpawnRateLimiter.ScaleBurstConstant(original.constantMax, scale);
			break;
		}
		return minMaxCurve;
	}

	// Token: 0x06009D2E RID: 40238 RVA: 0x003B6693 File Offset: 0x003B4893
	[PublicizedFrom(EAccessModifier.Private)]
	public static float ScaleBurstConstant(float constant, float scale)
	{
		if (Utils.FastAbs(constant) <= 1E-45f)
		{
			return 0f;
		}
		return Utils.FastMax(Mathf.Round(constant * scale), 1f) + float.Epsilon;
	}

	// Token: 0x040076A8 RID: 30376
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cScaleMin = 0.15f;

	// Token: 0x040076A9 RID: 30377
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<ParticleSystem, ParticleSystemSpawnRateLimiter.EmissionRateData> originalRates = new Dictionary<ParticleSystem, ParticleSystemSpawnRateLimiter.EmissionRateData>();

	// Token: 0x0200138A RID: 5002
	[PublicizedFrom(EAccessModifier.Private)]
	public class EmissionRateData
	{
		// Token: 0x040076AA RID: 30378
		public ParticleSystemCurveMode mode;

		// Token: 0x040076AB RID: 30379
		public float constant;

		// Token: 0x040076AC RID: 30380
		public AnimationCurve curve;

		// Token: 0x040076AD RID: 30381
		public float curveMultiplier;

		// Token: 0x040076AE RID: 30382
		public float constantMin;

		// Token: 0x040076AF RID: 30383
		public float constantMax;

		// Token: 0x040076B0 RID: 30384
		public AnimationCurve curveMin;

		// Token: 0x040076B1 RID: 30385
		public AnimationCurve curveMax;

		// Token: 0x040076B2 RID: 30386
		public ParticleSystem.Burst[] originalBursts;
	}
}
