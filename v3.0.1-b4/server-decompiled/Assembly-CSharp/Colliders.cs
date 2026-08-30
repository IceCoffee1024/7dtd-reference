using System;
using UnityEngine;

// Token: 0x020013A2 RID: 5026
public class Colliders
{
	// Token: 0x06009E00 RID: 40448 RVA: 0x003B9D0C File Offset: 0x003B7F0C
	public static bool Trace(Rigidbody body, Vector3 dir, float distance, int layerMask, out RaycastHit hitInfo)
	{
		hitInfo = default(RaycastHit);
		if (layerMask == 0)
		{
			return false;
		}
		RaycastHit[] array = body.SweepTestAll(dir, distance);
		if (array.Length < 1)
		{
			return false;
		}
		float num = float.MaxValue;
		foreach (RaycastHit raycastHit in array)
		{
			int layer = raycastHit.collider.gameObject.layer;
			if (((1 << layer) & layerMask) != 0 && raycastHit.distance < num)
			{
				hitInfo = raycastHit;
				num = hitInfo.distance;
			}
		}
		return hitInfo.collider != null;
	}

	// Token: 0x06009E01 RID: 40449 RVA: 0x003B9D98 File Offset: 0x003B7F98
	public static RaycastHit[] TraceAll(Rigidbody body, Vector3 dir, float distance, int layerMask)
	{
		if (layerMask == 0)
		{
			return null;
		}
		RaycastHit[] array = body.SweepTestAll(dir, distance);
		if (array.Length < 1)
		{
			return null;
		}
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			int layer = array[i].collider.gameObject.layer;
			if (((1 << layer) & layerMask) != 0)
			{
				num++;
			}
		}
		if (num < 1)
		{
			return null;
		}
		RaycastHit[] array2 = new RaycastHit[num];
		num = 0;
		foreach (RaycastHit raycastHit in array)
		{
			int layer2 = raycastHit.collider.gameObject.layer;
			if (((1 << layer2) & layerMask) != 0)
			{
				array2[num++] = raycastHit;
			}
		}
		return array2;
	}
}
