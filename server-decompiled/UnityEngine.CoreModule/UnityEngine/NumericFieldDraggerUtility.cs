using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine
{
	// Token: 0x02000204 RID: 516
	[MovedFrom("UnityEditor")]
	internal class NumericFieldDraggerUtility
	{
		// Token: 0x0600175F RID: 5983 RVA: 0x00026FE0 File Offset: 0x000251E0
		internal static float Acceleration(bool shiftPressed, bool altPressed)
		{
			return (float)(shiftPressed ? 4 : 1) * (altPressed ? 0.25f : 1f);
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x0002700C File Offset: 0x0002520C
		internal static float NiceDelta(Vector2 deviceDelta, float acceleration)
		{
			deviceDelta.y = -deviceDelta.y;
			bool flag = Mathf.Abs(Mathf.Abs(deviceDelta.x) - Mathf.Abs(deviceDelta.y)) / Mathf.Max(Mathf.Abs(deviceDelta.x), Mathf.Abs(deviceDelta.y)) > 0.1f;
			if (flag)
			{
				bool flag2 = Mathf.Abs(deviceDelta.x) > Mathf.Abs(deviceDelta.y);
				if (flag2)
				{
					NumericFieldDraggerUtility.s_UseYSign = false;
				}
				else
				{
					NumericFieldDraggerUtility.s_UseYSign = true;
				}
			}
			bool flag3 = NumericFieldDraggerUtility.s_UseYSign;
			float num;
			if (flag3)
			{
				num = Mathf.Sign(deviceDelta.y) * deviceDelta.magnitude * acceleration;
			}
			else
			{
				num = Mathf.Sign(deviceDelta.x) * deviceDelta.magnitude * acceleration;
			}
			return num;
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x000270D4 File Offset: 0x000252D4
		internal static double CalculateFloatDragSensitivity(double value)
		{
			bool flag = double.IsInfinity(value) || double.IsNaN(value);
			double num;
			if (flag)
			{
				num = 0.0;
			}
			else
			{
				num = Math.Max(1.0, Math.Pow(Math.Abs(value), 0.5)) * 0.029999999329447746;
			}
			return num;
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x00027134 File Offset: 0x00025334
		internal static double CalculateFloatDragSensitivity(double value, double minValue, double maxValue)
		{
			bool flag = double.IsInfinity(value) || double.IsNaN(value);
			double num;
			if (flag)
			{
				num = 0.0;
			}
			else
			{
				double num2 = Math.Abs(maxValue - minValue);
				num = num2 / 100.0 * 0.029999999329447746;
			}
			return num;
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x00027188 File Offset: 0x00025388
		internal static long CalculateIntDragSensitivity(long value)
		{
			return (long)NumericFieldDraggerUtility.CalculateIntDragSensitivity((double)value);
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x000271A4 File Offset: 0x000253A4
		internal static ulong CalculateIntDragSensitivity(ulong value)
		{
			return (ulong)NumericFieldDraggerUtility.CalculateIntDragSensitivity(value);
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x000271C0 File Offset: 0x000253C0
		private static double CalculateIntDragSensitivity(double value)
		{
			return Math.Max(1.0, Math.Pow(Math.Abs(value), 0.5) * 0.029999999329447746);
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x00027200 File Offset: 0x00025400
		internal static long CalculateIntDragSensitivity(long value, long minValue, long maxValue)
		{
			long num = Math.Abs(maxValue - minValue);
			return Math.Max(1L, (long)(0.03f * (float)num / 100f));
		}

		// Token: 0x04000869 RID: 2153
		private static bool s_UseYSign;

		// Token: 0x0400086A RID: 2154
		private const float kDragSensitivity = 0.03f;
	}
}
