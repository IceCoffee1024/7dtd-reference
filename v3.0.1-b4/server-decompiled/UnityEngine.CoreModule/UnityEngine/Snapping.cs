using System;

namespace UnityEngine
{
	// Token: 0x02000283 RID: 643
	public static class Snapping
	{
		// Token: 0x06001AD4 RID: 6868 RVA: 0x0002D3B8 File Offset: 0x0002B5B8
		internal static bool IsCardinalDirection(Vector3 direction)
		{
			return (Mathf.Abs(direction.x) > 0f && Mathf.Approximately(direction.y, 0f) && Mathf.Approximately(direction.z, 0f)) || (Mathf.Abs(direction.y) > 0f && Mathf.Approximately(direction.x, 0f) && Mathf.Approximately(direction.z, 0f)) || (Mathf.Abs(direction.z) > 0f && Mathf.Approximately(direction.x, 0f) && Mathf.Approximately(direction.y, 0f));
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x0002D470 File Offset: 0x0002B670
		public static float Snap(float val, float snap)
		{
			bool flag = snap == 0f;
			float num;
			if (flag)
			{
				num = val;
			}
			else
			{
				num = snap * Mathf.Round(val / snap);
			}
			return num;
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x0002D49C File Offset: 0x0002B69C
		public static Vector2 Snap(Vector2 val, Vector2 snap)
		{
			return new Vector3((Mathf.Abs(snap.x) < Mathf.Epsilon) ? val.x : (snap.x * Mathf.Round(val.x / snap.x)), (Mathf.Abs(snap.y) < Mathf.Epsilon) ? val.y : (snap.y * Mathf.Round(val.y / snap.y)));
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x0002D520 File Offset: 0x0002B720
		public static Vector3 Snap(Vector3 val, Vector3 snap, SnapAxis axis = SnapAxis.All)
		{
			return new Vector3(((axis & SnapAxis.X) == SnapAxis.X) ? Snapping.Snap(val.x, snap.x) : val.x, ((axis & SnapAxis.Y) == SnapAxis.Y) ? Snapping.Snap(val.y, snap.y) : val.y, ((axis & SnapAxis.Z) == SnapAxis.Z) ? Snapping.Snap(val.z, snap.z) : val.z);
		}
	}
}
