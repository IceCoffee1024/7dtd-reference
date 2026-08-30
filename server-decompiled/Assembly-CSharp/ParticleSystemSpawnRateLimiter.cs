using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001357 RID: 4951
public class ParticleSystemSpawnRateLimiter : MonoBehaviour
{
	// Token: 0x06009C25 RID: 39973 RVA: 0x003AF344 File Offset: 0x003AD544
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

	// Token: 0x06009C26 RID: 39974 RVA: 0x003AF49B File Offset: 0x003AD69B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		GameOptionsManager.OnGameOptionsApplied -= this.OnGameOptionsApplied;
	}

	// Token: 0x06009C27 RID: 39975 RVA: 0x003AF4B0 File Offset: 0x003AD6B0
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

	// Token: 0x06009C28 RID: 39976 RVA: 0x003AF504 File Offset: 0x003AD704
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

	// Token: 0x06009C29 RID: 39977 RVA: 0x003AF640 File Offset: 0x003AD840
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

	// Token: 0x06009C2A RID: 39978 RVA: 0x003AF713 File Offset: 0x003AD913
	[PublicizedFrom(EAccessModifier.Private)]
	public static float ScaleBurstConstant(float constant, float scale)
	{
		if (Utils.FastAbs(constant) <= 1E-45f)
		{
			return 0f;
		}
		return Utils.FastMax(Mathf.Round(constant * scale), 1f) + float.Epsilon;
	}

	// Token: 0x040075EC RID: 30188
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cScaleMin = 0.15f;

	// Token: 0x040075ED RID: 30189
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<ParticleSystem, ParticleSystemSpawnRateLimiter.EmissionRateData> originalRates = new Dictionary<ParticleSystem, ParticleSystemSpawnRateLimiter.EmissionRateData>();

	// Token: 0x02001358 RID: 4952
	[PublicizedFrom(EAccessModifier.Private)]
	public class EmissionRateData
	{
		// Token: 0x040075EE RID: 30190
		public ParticleSystemCurveMode mode;

		// Token: 0x040075EF RID: 30191
		public float constant;

		// Token: 0x040075F0 RID: 30192
		public AnimationCurve curve;

		// Token: 0x040075F1 RID: 30193
		public float curveMultiplier;

		// Token: 0x040075F2 RID: 30194
		public float constantMin;

		// Token: 0x040075F3 RID: 30195
		public float constantMax;

		// Token: 0x040075F4 RID: 30196
		public AnimationCurve curveMin;

		// Token: 0x040075F5 RID: 30197
		public AnimationCurve curveMax;

		// Token: 0x040075F6 RID: 30198
		public ParticleSystem.Burst[] originalBursts;
	}
}
