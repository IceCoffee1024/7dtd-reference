using System;
using UnityEngine;

// Token: 0x02001586 RID: 5510
public static class vp_MathUtility
{
	// Token: 0x0600AC5D RID: 44125 RVA: 0x00402C6D File Offset: 0x00400E6D
	public static float NaNSafeFloat(float value, float prevValue = 0f)
	{
		value = (double.IsNaN((double)value) ? prevValue : value);
		return value;
	}

	// Token: 0x0600AC5E RID: 44126 RVA: 0x00402C80 File Offset: 0x00400E80
	public static Vector2 NaNSafeVector2(Vector2 vector, Vector2 prevVector = default(Vector2))
	{
		vector.x = (double.IsNaN((double)vector.x) ? prevVector.x : vector.x);
		vector.y = (double.IsNaN((double)vector.y) ? prevVector.y : vector.y);
		return vector;
	}

	// Token: 0x0600AC5F RID: 44127 RVA: 0x00402CD4 File Offset: 0x00400ED4
	public static Vector3 NaNSafeVector3(Vector3 vector, Vector3 prevVector = default(Vector3))
	{
		vector.x = (double.IsNaN((double)vector.x) ? prevVector.x : vector.x);
		vector.y = (double.IsNaN((double)vector.y) ? prevVector.y : vector.y);
		vector.z = (double.IsNaN((double)vector.z) ? prevVector.z : vector.z);
		return vector;
	}

	// Token: 0x0600AC60 RID: 44128 RVA: 0x00402D4C File Offset: 0x00400F4C
	public static Quaternion NaNSafeQuaternion(Quaternion quaternion, Quaternion prevQuaternion = default(Quaternion))
	{
		quaternion.x = (double.IsNaN((double)quaternion.x) ? prevQuaternion.x : quaternion.x);
		quaternion.y = (double.IsNaN((double)quaternion.y) ? prevQuaternion.y : quaternion.y);
		quaternion.z = (double.IsNaN((double)quaternion.z) ? prevQuaternion.z : quaternion.z);
		quaternion.w = (double.IsNaN((double)quaternion.w) ? prevQuaternion.w : quaternion.w);
		return quaternion;
	}

	// Token: 0x0600AC61 RID: 44129 RVA: 0x00402DE8 File Offset: 0x00400FE8
	public static Vector3 SnapToZero(Vector3 value, float epsilon = 0.0001f)
	{
		value.x = ((Mathf.Abs(value.x) < epsilon) ? 0f : value.x);
		value.y = ((Mathf.Abs(value.y) < epsilon) ? 0f : value.y);
		value.z = ((Mathf.Abs(value.z) < epsilon) ? 0f : value.z);
		return value;
	}

	// Token: 0x0600AC62 RID: 44130 RVA: 0x00402E5C File Offset: 0x0040105C
	public static float SnapToZero(float value, float epsilon = 0.0001f)
	{
		value = ((Mathf.Abs(value) < epsilon) ? 0f : value);
		return value;
	}

	// Token: 0x0600AC63 RID: 44131 RVA: 0x00402E72 File Offset: 0x00401072
	public static float ReduceDecimals(float value, float factor = 1000f)
	{
		return Mathf.Round(value * factor) / factor;
	}

	// Token: 0x0600AC64 RID: 44132 RVA: 0x00402E7E File Offset: 0x0040107E
	public static bool IsOdd(int val)
	{
		return val % 2 != 0;
	}

	// Token: 0x0600AC65 RID: 44133 RVA: 0x00402E86 File Offset: 0x00401086
	public static float Sinus(float rate, float amp, float offset = 0f)
	{
		return Mathf.Cos((Time.time + offset) * rate) * amp;
	}
}
