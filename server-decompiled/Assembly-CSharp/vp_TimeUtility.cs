using System;
using UnityEngine;

// Token: 0x02001592 RID: 5522
public static class vp_TimeUtility
{
	// Token: 0x17001401 RID: 5121
	// (get) Token: 0x0600ACB8 RID: 44216 RVA: 0x00403FD4 File Offset: 0x004021D4
	// (set) Token: 0x0600ACB9 RID: 44217 RVA: 0x00403FDB File Offset: 0x004021DB
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

	// Token: 0x17001402 RID: 5122
	// (get) Token: 0x0600ACBA RID: 44218 RVA: 0x00403FFB File Offset: 0x004021FB
	public static float AdjustedTimeScale
	{
		get
		{
			return 1f / (Time.timeScale * (0.02f / Time.fixedDeltaTime));
		}
	}

	// Token: 0x0600ACBB RID: 44219 RVA: 0x00404014 File Offset: 0x00402214
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

	// Token: 0x0600ACBC RID: 44220 RVA: 0x00404068 File Offset: 0x00402268
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

	// Token: 0x17001403 RID: 5123
	// (get) Token: 0x0600ACBD RID: 44221 RVA: 0x004040DA File Offset: 0x004022DA
	// (set) Token: 0x0600ACBE RID: 44222 RVA: 0x004040E4 File Offset: 0x004022E4
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

	// Token: 0x04007FBD RID: 32701
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_MinTimeScale = 0.1f;

	// Token: 0x04007FBE RID: 32702
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_MaxTimeScale = 1f;

	// Token: 0x04007FBF RID: 32703
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool m_Paused = false;

	// Token: 0x04007FC0 RID: 32704
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_TimeScaleOnPause = 1f;

	// Token: 0x04007FC1 RID: 32705
	public static float InitialFixedTimeStep = Time.fixedDeltaTime;
}
