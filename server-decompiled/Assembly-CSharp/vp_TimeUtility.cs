using System;
using UnityEngine;

// Token: 0x020015B4 RID: 5556
public static class vp_TimeUtility
{
	// Token: 0x17001446 RID: 5190
	// (get) Token: 0x0600AD6B RID: 44395 RVA: 0x00408E28 File Offset: 0x00407028
	// (set) Token: 0x0600AD6C RID: 44396 RVA: 0x00408E2F File Offset: 0x0040702F
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

	// Token: 0x17001447 RID: 5191
	// (get) Token: 0x0600AD6D RID: 44397 RVA: 0x00408E4F File Offset: 0x0040704F
	public static float AdjustedTimeScale
	{
		get
		{
			return 1f / (Time.timeScale * (0.02f / Time.fixedDeltaTime));
		}
	}

	// Token: 0x0600AD6E RID: 44398 RVA: 0x00408E68 File Offset: 0x00407068
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

	// Token: 0x0600AD6F RID: 44399 RVA: 0x00408EBC File Offset: 0x004070BC
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

	// Token: 0x17001448 RID: 5192
	// (get) Token: 0x0600AD70 RID: 44400 RVA: 0x00408F2E File Offset: 0x0040712E
	// (set) Token: 0x0600AD71 RID: 44401 RVA: 0x00408F38 File Offset: 0x00407138
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

	// Token: 0x04008040 RID: 32832
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_MinTimeScale = 0.1f;

	// Token: 0x04008041 RID: 32833
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_MaxTimeScale = 1f;

	// Token: 0x04008042 RID: 32834
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool m_Paused = false;

	// Token: 0x04008043 RID: 32835
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_TimeScaleOnPause = 1f;

	// Token: 0x04008044 RID: 32836
	public static float InitialFixedTimeStep = Time.fixedDeltaTime;
}
