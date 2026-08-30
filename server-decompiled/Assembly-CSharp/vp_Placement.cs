using System;
using UnityEngine;

// Token: 0x020015D0 RID: 5584
public class vp_Placement
{
	// Token: 0x0600AE7D RID: 44669 RVA: 0x004104D0 File Offset: 0x0040E6D0
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

	// Token: 0x0600AE7E RID: 44670 RVA: 0x00410537 File Offset: 0x0040E737
	public virtual bool IsObstructed(float physicsRadius = 1f)
	{
		return Physics.CheckSphere(this.Position, physicsRadius, 2261000);
	}

	// Token: 0x0600AE7F RID: 44671 RVA: 0x00410550 File Offset: 0x0040E750
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

	// Token: 0x04008212 RID: 33298
	public Vector3 Position = Vector3.zero;

	// Token: 0x04008213 RID: 33299
	public Quaternion Rotation = Quaternion.identity;
}
