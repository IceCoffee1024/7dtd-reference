using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200133D RID: 4925
public class ParticleSystemSpawnRateLimiter : MonoBehaviour
{
	// Token: 0x06009B2A RID: 39722 RVA: 0x003AA09C File Offset: 0x003A829C
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

	// Token: 0x06009B2B RID: 39723 RVA: 0x003AA1F3 File Offset: 0x003A83F3
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		GameOptionsManager.OnGameOptionsApplied -= this.OnGameOptionsApplied;
	}

	// Token: 0x06009B2C RID: 39724 RVA: 0x003AA208 File Offset: 0x003A8408
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

	// Token: 0x06009B2D RID: 39725 RVA: 0x003AA25C File Offset: 0x003A845C
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

	// Token: 0x06009B2E RID: 39726 RVA: 0x003AA398 File Offset: 0x003A8598
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

	// Token: 0x06009B2F RID: 39727 RVA: 0x003AA46B File Offset: 0x003A866B
	[PublicizedFrom(EAccessModifier.Private)]
	public static float ScaleBurstConstant(float constant, float scale)
	{
		if (Utils.FastAbs(constant) <= 1E-45f)
		{
			return 0f;
		}
		return Utils.FastMax(Mathf.Round(constant * scale), 1f) + float.Epsilon;
	}

	// Token: 0x04007514 RID: 29972
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cScaleMin = 0.15f;

	// Token: 0x04007515 RID: 29973
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<ParticleSystem, ParticleSystemSpawnRateLimiter.EmissionRateData> originalRates = new Dictionary<ParticleSystem, ParticleSystemSpawnRateLimiter.EmissionRateData>();

	// Token: 0x0200133E RID: 4926
	[PublicizedFrom(EAccessModifier.Private)]
	public class EmissionRateData
	{
		// Token: 0x04007516 RID: 29974
		public ParticleSystemCurveMode mode;

		// Token: 0x04007517 RID: 29975
		public float constant;

		// Token: 0x04007518 RID: 29976
		public AnimationCurve curve;

		// Token: 0x04007519 RID: 29977
		public float curveMultiplier;

		// Token: 0x0400751A RID: 29978
		public float constantMin;

		// Token: 0x0400751B RID: 29979
		public float constantMax;

		// Token: 0x0400751C RID: 29980
		public AnimationCurve curveMin;

		// Token: 0x0400751D RID: 29981
		public AnimationCurve curveMax;

		// Token: 0x0400751E RID: 29982
		public ParticleSystem.Burst[] originalBursts;
	}
}
