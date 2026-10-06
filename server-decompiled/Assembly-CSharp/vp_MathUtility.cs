using System;
using UnityEngine;

// Token: 0x020015A8 RID: 5544
public static class vp_MathUtility
{
	// Token: 0x0600AD10 RID: 44304 RVA: 0x00407AC1 File Offset: 0x00405CC1
	public static float NaNSafeFloat(float value, float prevValue = 0f)
	{
		value = (double.IsNaN((double)value) ? prevValue : value);
		return value;
	}

	// Token: 0x0600AD11 RID: 44305 RVA: 0x00407AD4 File Offset: 0x00405CD4
	public static Vector2 NaNSafeVector2(Vector2 vector, Vector2 prevVector = default(Vector2))
	{
		vector.x = (double.IsNaN((double)vector.x) ? prevVector.x : vector.x);
		vector.y = (double.IsNaN((double)vector.y) ? prevVector.y : vector.y);
		return vector;
	}

	// Token: 0x0600AD12 RID: 44306 RVA: 0x00407B28 File Offset: 0x00405D28
	public static Vector3 NaNSafeVector3(Vector3 vector, Vector3 prevVector = default(Vector3))
	{
		vector.x = (double.IsNaN((double)vector.x) ? prevVector.x : vector.x);
		vector.y = (double.IsNaN((double)vector.y) ? prevVector.y : vector.y);
		vector.z = (double.IsNaN((double)vector.z) ? prevVector.z : vector.z);
		return vector;
	}

	// Token: 0x0600AD13 RID: 44307 RVA: 0x00407BA0 File Offset: 0x00405DA0
	public static Quaternion NaNSafeQuaternion(Quaternion quaternion, Quaternion prevQuaternion = default(Quaternion))
	{
		quaternion.x = (double.IsNaN((double)quaternion.x) ? prevQuaternion.x : quaternion.x);
		quaternion.y = (double.IsNaN((double)quaternion.y) ? prevQuaternion.y : quaternion.y);
		quaternion.z = (double.IsNaN((double)quaternion.z) ? prevQuaternion.z : quaternion.z);
		quaternion.w = (double.IsNaN((double)quaternion.w) ? prevQuaternion.w : quaternion.w);
		return quaternion;
	}

	// Token: 0x0600AD14 RID: 44308 RVA: 0x00407C3C File Offset: 0x00405E3C
	public static Vector3 SnapToZero(Vector3 value, float epsilon = 0.0001f)
	{
		value.x = ((Mathf.Abs(value.x) < epsilon) ? 0f : value.x);
		value.y = ((Mathf.Abs(value.y) < epsilon) ? 0f : value.y);
		value.z = ((Mathf.Abs(value.z) < epsilon) ? 0f : value.z);
		return value;
	}

	// Token: 0x0600AD15 RID: 44309 RVA: 0x00407CB0 File Offset: 0x00405EB0
	public static float SnapToZero(float value, float epsilon = 0.0001f)
	{
		value = ((Mathf.Abs(value) < epsilon) ? 0f : value);
		return value;
	}

	// Token: 0x0600AD16 RID: 44310 RVA: 0x00407CC6 File Offset: 0x00405EC6
	public static float ReduceDecimals(float value, float factor = 1000f)
	{
		return Mathf.Round(value * factor) / factor;
	}

	// Token: 0x0600AD17 RID: 44311 RVA: 0x00407CD2 File Offset: 0x00405ED2
	public static bool IsOdd(int val)
	{
		return val % 2 != 0;
	}

	// Token: 0x0600AD18 RID: 44312 RVA: 0x00407CDA File Offset: 0x00405EDA
	public static float Sinus(float rate, float amp, float offset = 0f)
	{
		return Mathf.Cos((Time.time + offset) * rate) * amp;
	}
}
