using System;
using UnityEngine;

// Token: 0x0200137F RID: 4991
public class ApplyExplosionForce : MonoBehaviour
{
	// Token: 0x06009D89 RID: 40329 RVA: 0x003B8B78 File Offset: 0x003B6D78
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

	// Token: 0x0400779C RID: 30620
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cUpwards = 3f;

	// Token: 0x0400779D RID: 30621
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cMaxColliders = 1024;

	// Token: 0x0400779E RID: 30622
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Collider[] colliderList = new Collider[1024];
}
