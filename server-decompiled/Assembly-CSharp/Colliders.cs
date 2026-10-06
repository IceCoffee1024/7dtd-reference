using System;
using UnityEngine;

// Token: 0x020013DD RID: 5085
public class Colliders
{
	// Token: 0x06009FA8 RID: 40872 RVA: 0x003C3BEC File Offset: 0x003C1DEC
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

	// Token: 0x06009FA9 RID: 40873 RVA: 0x003C3C78 File Offset: 0x003C1E78
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
