using System;
using UnityEngine;

// Token: 0x020015C6 RID: 5574
public static class vp_TimeUtility
{
	// Token: 0x17001452 RID: 5202
	// (get) Token: 0x0600ADC3 RID: 44483 RVA: 0x0040B22C File Offset: 0x0040942C
	// (set) Token: 0x0600ADC4 RID: 44484 RVA: 0x0040B233 File Offset: 0x00409433
	public static float TimeScale
	{
		get
		{
			return Time.timeScale;
		}
		set
		{
			value = vp_TimeUtility.ClampTimeScale(value);
			Time.timeScale = value;
			Time.fixedDeltaTime = vp_TimeUtility.InitialFixedTimeStep * Time.timeScale;
		}
	}

	// Token: 0x17001453 RID: 5203
	// (get) Token: 0x0600ADC5 RID: 44485 RVA: 0x0040B253 File Offset: 0x00409453
	public static float AdjustedTimeScale
	{
		get
		{
			return 1f / (Time.timeScale * (0.02f / Time.fixedDeltaTime));
		}
	}

	// Token: 0x0600ADC6 RID: 44486 RVA: 0x0040B26C File Offset: 0x0040946C
	public static void FadeTimeScale(float targetTimeScale, float fadeSpeed)
	{
		if (vp_TimeUtility.TimeScale == targetTimeScale)
		{
			return;
		}
		targetTimeScale = vp_TimeUtility.ClampTimeScale(targetTimeScale);
		vp_TimeUtility.TimeScale = Mathf.Lerp(vp_TimeUtility.TimeScale, targetTimeScale, Time.deltaTime * 60f * fadeSpeed);
		if (Mathf.Abs(vp_TimeUtility.TimeScale - targetTimeScale) < 0.01f)
		{
			vp_TimeUtility.TimeScale = targetTimeScale;
		}
	}

	// Token: 0x0600ADC7 RID: 44487 RVA: 0x0040B2C0 File Offset: 0x004094C0
	[PublicizedFrom(EAccessModifier.Private)]
	public static float ClampTimeScale(float t)
	{
		if (t < vp_TimeUtility.m_MinTimeScale || t > vp_TimeUtility.m_MaxTimeScale)
		{
			t = Mathf.Clamp(t, vp_TimeUtility.m_MinTimeScale, vp_TimeUtility.m_MaxTimeScale);
			Debug.LogWarning(string.Concat(new string[]
			{
				"Warning: (vp_TimeUtility) TimeScale was clamped to within the supported range (",
				vp_TimeUtility.m_MinTimeScale.ToCultureInvariantString(),
				" - ",
				vp_TimeUtility.m_MaxTimeScale.ToCultureInvariantString(),
				")."
			}));
		}
		return t;
	}

	// Token: 0x17001454 RID: 5204
	// (get) Token: 0x0600ADC8 RID: 44488 RVA: 0x0040B332 File Offset: 0x00409532
	// (set) Token: 0x0600ADC9 RID: 44489 RVA: 0x0040B33C File Offset: 0x0040953C
	public static bool Paused
	{
		get
		{
			return vp_TimeUtility.m_Paused;
		}
		set
		{
			if (value)
			{
				if (vp_TimeUtility.m_Paused)
				{
					return;
				}
				vp_TimeUtility.m_Paused = true;
				vp_TimeUtility.m_TimeScaleOnPause = Time.timeScale;
				Time.timeScale = 0f;
				return;
			}
			else
			{
				if (!vp_TimeUtility.m_Paused)
				{
					return;
				}
				vp_TimeUtility.m_Paused = false;
				Time.timeScale = vp_TimeUtility.m_TimeScaleOnPause;
				vp_TimeUtility.m_TimeScaleOnPause = 1f;
				return;
			}
		}
	}

	// Token: 0x04008081 RID: 32897
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_MinTimeScale = 0.1f;

	// Token: 0x04008082 RID: 32898
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_MaxTimeScale = 1f;

	// Token: 0x04008083 RID: 32899
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool m_Paused = false;

	// Token: 0x04008084 RID: 32900
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_TimeScaleOnPause = 1f;

	// Token: 0x04008085 RID: 32901
	public static float InitialFixedTimeStep = Time.fixedDeltaTime;
}
