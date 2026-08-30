using System;
using UnityEngine;

// Token: 0x020014DB RID: 5339
public class WaterClippingVolume
{
	// Token: 0x0600A7EE RID: 42990 RVA: 0x003E7C1C File Offset: 0x003E5E1C
	public void Prepare(Plane waterClipPlane)
	{
		this.waterClipPlane = waterClipPlane;
		this.isSliced = WaterClippingUtils.GetCubePlaneIntersectionEdgeLoop(waterClipPlane, ref this.intersectionPoints, out this.count);
	}

	// Token: 0x0600A7EF RID: 42991 RVA: 0x003E7C40 File Offset: 0x003E5E40
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

	// Token: 0x04007C2D RID: 31789
	[PublicizedFrom(EAccessModifier.Private)]
	public Plane waterClipPlane;

	// Token: 0x04007C2E RID: 31790
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3[] intersectionPoints = new Vector3[6];

	// Token: 0x04007C2F RID: 31791
	[PublicizedFrom(EAccessModifier.Private)]
	public int count = -1;

	// Token: 0x04007C30 RID: 31792
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSliced;
}
