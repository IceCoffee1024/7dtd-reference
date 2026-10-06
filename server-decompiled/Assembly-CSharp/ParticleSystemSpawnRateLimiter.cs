using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001377 RID: 4983
public class ParticleSystemSpawnRateLimiter : MonoBehaviour
{
	// Token: 0x06009CD1 RID: 40145 RVA: 0x003B3EC0 File Offset: 0x003B20C0
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

	// Token: 0x06009CD2 RID: 40146 RVA: 0x003B4017 File Offset: 0x003B2217
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		GameOptionsManager.OnGameOptionsApplied -= this.OnGameOptionsApplied;
	}

	// Token: 0x06009CD3 RID: 40147 RVA: 0x003B402C File Offset: 0x003B222C
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

	// Token: 0x06009CD4 RID: 40148 RVA: 0x003B4080 File Offset: 0x003B2280
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

	// Token: 0x06009CD5 RID: 40149 RVA: 0x003B41BC File Offset: 0x003B23BC
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

	// Token: 0x06009CD6 RID: 40150 RVA: 0x003B428F File Offset: 0x003B248F
	[PublicizedFrom(EAccessModifier.Private)]
	public static float ScaleBurstConstant(float constant, float scale)
	{
		if (Utils.FastAbs(constant) <= 1E-45f)
		{
			return 0f;
		}
		return Utils.FastMax(Mathf.Round(constant * scale), 1f) + float.Epsilon;
	}

	// Token: 0x04007667 RID: 30311
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cScaleMin = 0.15f;

	// Token: 0x04007668 RID: 30312
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<ParticleSystem, ParticleSystemSpawnRateLimiter.EmissionRateData> originalRates = new Dictionary<ParticleSystem, ParticleSystemSpawnRateLimiter.EmissionRateData>();

	// Token: 0x02001378 RID: 4984
	[PublicizedFrom(EAccessModifier.Private)]
	public class EmissionRateData
	{
		// Token: 0x04007669 RID: 30313
		public ParticleSystemCurveMode mode;

		// Token: 0x0400766A RID: 30314
		public float constant;

		// Token: 0x0400766B RID: 30315
		public AnimationCurve curve;

		// Token: 0x0400766C RID: 30316
		public float curveMultiplier;

		// Token: 0x0400766D RID: 30317
		public float constantMin;

		// Token: 0x0400766E RID: 30318
		public float constantMax;

		// Token: 0x0400766F RID: 30319
		public AnimationCurve curveMin;

		// Token: 0x04007670 RID: 30320
		public AnimationCurve curveMax;

		// Token: 0x04007671 RID: 30321
		public ParticleSystem.Burst[] originalBursts;
	}
}
