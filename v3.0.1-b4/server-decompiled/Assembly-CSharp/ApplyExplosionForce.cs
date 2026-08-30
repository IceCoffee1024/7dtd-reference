using System;
using UnityEngine;

// Token: 0x02001365 RID: 4965
public class ApplyExplosionForce : MonoBehaviour
{
	// Token: 0x06009C8E RID: 40078 RVA: 0x003B38C4 File Offset: 0x003B1AC4
	public static void Explode(Vector3 explosionPos, float power, float radius)
	{
		explosionPos -= Origin.position;
		power *= 20f;
		radius *= 1.75f;
		int num = Physics.OverlapSphereNonAlloc(explosionPos, radius, ApplyExplosionForce.colliderList);
		if (num > 1024)
		{
			num = 1024;
		}
		for (int i = 0; i < num; i++)
		{
			Rigidbody component = ApplyExplosionForce.colliderList[i].GetComponent<Rigidbody>();
			if (component)
			{
				component.AddExplosionForce(power, explosionPos, radius, 3f);
			}
		}
	}

	// Token: 0x040076C4 RID: 30404
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cUpwards = 3f;

	// Token: 0x040076C5 RID: 30405
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cMaxColliders = 1024;

	// Token: 0x040076C6 RID: 30406
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Collider[] colliderList = new Collider[1024];
}
