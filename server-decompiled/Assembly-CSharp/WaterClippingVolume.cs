using System;
using UnityEngine;

// Token: 0x02001518 RID: 5400
public class WaterClippingVolume
{
	// Token: 0x0600A99E RID: 43422 RVA: 0x003F1DB0 File Offset: 0x003EFFB0
	public void Prepare(Plane waterClipPlane)
	{
		this.waterClipPlane = waterClipPlane;
		this.isSliced = WaterClippingUtils.GetCubePlaneIntersectionEdgeLoop(waterClipPlane, ref this.intersectionPoints, out this.count);
	}

	// Token: 0x0600A99F RID: 43423 RVA: 0x003F1DD4 File Offset: 0x003EFFD4
	public void ApplyClipping(ref Vector3 vertLocalPos)
	{
		if (!this.isSliced)
		{
			return;
		}
		if (this.waterClipPlane.GetDistanceToPoint(vertLocalPos) > 0f)
		{
			vertLocalPos = this.waterClipPlane.ClosestPointOnPlane(vertLocalPos);
			if (!WaterClippingUtils.CubeBounds.Contains(vertLocalPos))
			{
				vertLocalPos = GeometryUtils.NearestPointOnEdgeLoop(vertLocalPos, this.intersectionPoints, this.count);
			}
			return;
		}
	}

	// Token: 0x04007D89 RID: 32137
	[PublicizedFrom(EAccessModifier.Private)]
	public Plane waterClipPlane;

	// Token: 0x04007D8A RID: 32138
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3[] intersectionPoints = new Vector3[6];

	// Token: 0x04007D8B RID: 32139
	[PublicizedFrom(EAccessModifier.Private)]
	public int count = -1;

	// Token: 0x04007D8C RID: 32140
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSliced;
}
