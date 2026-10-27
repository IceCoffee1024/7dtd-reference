using System;
using UnityEngine;

// Token: 0x020015BA RID: 5562
public static class vp_MathUtility
{
	// Token: 0x0600AD68 RID: 44392 RVA: 0x00409EC5 File Offset: 0x004080C5
	public static float NaNSafeFloat(float value, float prevValue = 0f)
	{
		value = (double.IsNaN((double)value) ? prevValue : value);
		return value;
	}

	// Token: 0x0600AD69 RID: 44393 RVA: 0x00409ED8 File Offset: 0x004080D8
	public static Vector2 NaNSafeVector2(Vector2 vector, Vector2 prevVector = default(Vector2))
	{
		vector.x = (double.IsNaN((double)vector.x) ? prevVector.x : vector.x);
		vector.y = (double.IsNaN((double)vector.y) ? prevVector.y : vector.y);
		return vector;
	}

	// Token: 0x0600AD6A RID: 44394 RVA: 0x00409F2C File Offset: 0x0040812C
	public static Vector3 NaNSafeVector3(Vector3 vector, Vector3 prevVector = default(Vector3))
	{
		vector.x = (double.IsNaN((double)vector.x) ? prevVector.x : vector.x);
		vector.y = (double.IsNaN((double)vector.y) ? prevVector.y : vector.y);
		vector.z = (double.IsNaN((double)vector.z) ? prevVector.z : vector.z);
		return vector;
	}

	// Token: 0x0600AD6B RID: 44395 RVA: 0x00409FA4 File Offset: 0x004081A4
	public static Quaternion NaNSafeQuaternion(Quaternion quaternion, Quaternion prevQuaternion = default(Quaternion))
	{
		quaternion.x = (double.IsNaN((double)quaternion.x) ? prevQuaternion.x : quaternion.x);
		quaternion.y = (double.IsNaN((double)quaternion.y) ? prevQuaternion.y : quaternion.y);
		quaternion.z = (double.IsNaN((double)quaternion.z) ? prevQuaternion.z : quaternion.z);
		quaternion.w = (double.IsNaN((double)quaternion.w) ? prevQuaternion.w : quaternion.w);
		return quaternion;
	}

	// Token: 0x0600AD6C RID: 44396 RVA: 0x0040A040 File Offset: 0x00408240
	public static Vector3 SnapToZero(Vector3 value, float epsilon = 0.0001f)
	{
		value.x = ((Mathf.Abs(value.x) < epsilon) ? 0f : value.x);
		value.y = ((Mathf.Abs(value.y) < epsilon) ? 0f : value.y);
		value.z = ((Mathf.Abs(value.z) < epsilon) ? 0f : value.z);
		return value;
	}

	// Token: 0x0600AD6D RID: 44397 RVA: 0x0040A0B4 File Offset: 0x004082B4
	public static float SnapToZero(float value, float epsilon = 0.0001f)
	{
		value = ((Mathf.Abs(value) < epsilon) ? 0f : value);
		return value;
	}

	// Token: 0x0600AD6E RID: 44398 RVA: 0x0040A0CA File Offset: 0x004082CA
	public static float ReduceDecimals(float value, float factor = 1000f)
	{
		return Mathf.Round(value * factor) / factor;
	}

	// Token: 0x0600AD6F RID: 44399 RVA: 0x0040A0D6 File Offset: 0x004082D6
	public static bool IsOdd(int val)
	{
		return val % 2 != 0;
	}

	// Token: 0x0600AD70 RID: 44400 RVA: 0x0040A0DE File Offset: 0x004082DE
	public static float Sinus(float rate, float amp, float offset = 0f)
	{
		return Mathf.Cos((Time.time + offset) * rate) * amp;
	}
}
