using System;
using UnityEngine;

// Token: 0x02001577 RID: 5495
public static class vp_TimeUtility
{
	// Token: 0x170013C8 RID: 5064
	// (get) Token: 0x0600ABBB RID: 43963 RVA: 0x003FEC94 File Offset: 0x003FCE94
	// (set) Token: 0x0600ABBC RID: 43964 RVA: 0x003FEC9B File Offset: 0x003FCE9B
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

	// Token: 0x170013C9 RID: 5065
	// (get) Token: 0x0600ABBD RID: 43965 RVA: 0x003FECBB File Offset: 0x003FCEBB
	public static float AdjustedTimeScale
	{
		get
		{
			return 1f / (Time.timeScale * (0.02f / Time.fixedDeltaTime));
		}
	}

	// Token: 0x0600ABBE RID: 43966 RVA: 0x003FECD4 File Offset: 0x003FCED4
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

	// Token: 0x0600ABBF RID: 43967 RVA: 0x003FED28 File Offset: 0x003FCF28
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

	// Token: 0x170013CA RID: 5066
	// (get) Token: 0x0600ABC0 RID: 43968 RVA: 0x003FED9A File Offset: 0x003FCF9A
	// (set) Token: 0x0600ABC1 RID: 43969 RVA: 0x003FEDA4 File Offset: 0x003FCFA4
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

	// Token: 0x04007EE4 RID: 32484
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_MinTimeScale = 0.1f;

	// Token: 0x04007EE5 RID: 32485
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_MaxTimeScale = 1f;

	// Token: 0x04007EE6 RID: 32486
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool m_Paused = false;

	// Token: 0x04007EE7 RID: 32487
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_TimeScaleOnPause = 1f;

	// Token: 0x04007EE8 RID: 32488
	public static float InitialFixedTimeStep = Time.fixedDeltaTime;
}
