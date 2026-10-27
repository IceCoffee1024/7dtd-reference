using System;
using UnityEngine;

// Token: 0x02001604 RID: 5636
public class vp_Placement
{
	// Token: 0x0600AF88 RID: 44936 RVA: 0x00417728 File Offset: 0x00415928
	public static bool AdjustPosition(vp_Placement p, float physicsRadius, int attempts = 1000)
	{
		attempts--;
		if (attempts > 0)
		{
			if (p.IsObstructed(physicsRadius))
			{
				Vector3 insideUnitSphere = global::UnityEngine.Random.insideUnitSphere;
				p.Position.x = p.Position.x + insideUnitSphere.x;
				p.Position.z = p.Position.z + insideUnitSphere.z;
				vp_Placement.AdjustPosition(p, physicsRadius, attempts);
			}
			return true;
		}
		Debug.LogWarning("(vp_Placement.AdjustPosition) Failed to find valid placement.");
		return false;
	}

	// Token: 0x0600AF89 RID: 44937 RVA: 0x0041778F File Offset: 0x0041598F
	public virtual bool IsObstructed(float physicsRadius = 1f)
	{
		return Physics.CheckSphere(this.Position, physicsRadius, 2261000);
	}

	// Token: 0x0600AF8A RID: 44938 RVA: 0x004177A8 File Offset: 0x004159A8
	public static void SnapToGround(vp_Placement p, float radius, float snapDistance)
	{
		if (snapDistance == 0f)
		{
			return;
		}
		RaycastHit raycastHit;
		Physics.SphereCast(new Ray(p.Position + Vector3.up * snapDistance, Vector3.down), radius, out raycastHit, snapDistance * 2f, 1084850184);
		if (raycastHit.collider != null)
		{
			p.Position.y = raycastHit.point.y + 0.05f;
		}
	}

	// Token: 0x040082D6 RID: 33494
	public Vector3 Position = Vector3.zero;

	// Token: 0x040082D7 RID: 33495
	public Quaternion Rotation = Quaternion.identity;
}
