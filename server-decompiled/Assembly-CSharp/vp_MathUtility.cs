using System;
using UnityEngine;

// Token: 0x0200156B RID: 5483
public static class vp_MathUtility
{
	// Token: 0x0600AB60 RID: 43872 RVA: 0x003FD92D File Offset: 0x003FBB2D
	public static float NaNSafeFloat(float value, float prevValue = 0f)
	{
		value = (double.IsNaN((double)value) ? prevValue : value);
		return value;
	}

	// Token: 0x0600AB61 RID: 43873 RVA: 0x003FD940 File Offset: 0x003FBB40
	public static Vector2 NaNSafeVector2(Vector2 vector, Vector2 prevVector = default(Vector2))
	{
		vector.x = (double.IsNaN((double)vector.x) ? prevVector.x : vector.x);
		vector.y = (double.IsNaN((double)vector.y) ? prevVector.y : vector.y);
		return vector;
	}

	// Token: 0x0600AB62 RID: 43874 RVA: 0x003FD994 File Offset: 0x003FBB94
	public static Vector3 NaNSafeVector3(Vector3 vector, Vector3 prevVector = default(Vector3))
	{
		vector.x = (double.IsNaN((double)vector.x) ? prevVector.x : vector.x);
		vector.y = (double.IsNaN((double)vector.y) ? prevVector.y : vector.y);
		vector.z = (double.IsNaN((double)vector.z) ? prevVector.z : vector.z);
		return vector;
	}

	// Token: 0x0600AB63 RID: 43875 RVA: 0x003FDA0C File Offset: 0x003FBC0C
	public static Quaternion NaNSafeQuaternion(Quaternion quaternion, Quaternion prevQuaternion = default(Quaternion))
	{
		quaternion.x = (double.IsNaN((double)quaternion.x) ? prevQuaternion.x : quaternion.x);
		quaternion.y = (double.IsNaN((double)quaternion.y) ? prevQuaternion.y : quaternion.y);
		quaternion.z = (double.IsNaN((double)quaternion.z) ? prevQuaternion.z : quaternion.z);
		quaternion.w = (double.IsNaN((double)quaternion.w) ? prevQuaternion.w : quaternion.w);
		return quaternion;
	}

	// Token: 0x0600AB64 RID: 43876 RVA: 0x003FDAA8 File Offset: 0x003FBCA8
	public static Vector3 SnapToZero(Vector3 value, float epsilon = 0.0001f)
	{
		value.x = ((Mathf.Abs(value.x) < epsilon) ? 0f : value.x);
		value.y = ((Mathf.Abs(value.y) < epsilon) ? 0f : value.y);
		value.z = ((Mathf.Abs(value.z) < epsilon) ? 0f : value.z);
		return value;
	}

	// Token: 0x0600AB65 RID: 43877 RVA: 0x003FDB1C File Offset: 0x003FBD1C
	public static float SnapToZero(float value, float epsilon = 0.0001f)
	{
		value = ((Mathf.Abs(value) < epsilon) ? 0f : value);
		return value;
	}

	// Token: 0x0600AB66 RID: 43878 RVA: 0x003FDB32 File Offset: 0x003FBD32
	public static float ReduceDecimals(float value, float factor = 1000f)
	{
		return Mathf.Round(value * factor) / factor;
	}

	// Token: 0x0600AB67 RID: 43879 RVA: 0x003FDB3E File Offset: 0x003FBD3E
	public static bool IsOdd(int val)
	{
		return val % 2 != 0;
	}

	// Token: 0x0600AB68 RID: 43880 RVA: 0x003FDB46 File Offset: 0x003FBD46
	public static float Sinus(float rate, float amp, float offset = 0f)
	{
		return Mathf.Cos((Time.time + offset) * rate) * amp;
	}
}
