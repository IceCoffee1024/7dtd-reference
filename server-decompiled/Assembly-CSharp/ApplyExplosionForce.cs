using System;
using UnityEngine;

// Token: 0x0200139F RID: 5023
public class ApplyExplosionForce : MonoBehaviour
{
	// Token: 0x06009E34 RID: 40500 RVA: 0x003BD6F4 File Offset: 0x003BB8F4
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

	// Token: 0x04007817 RID: 30743
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cUpwards = 3f;

	// Token: 0x04007818 RID: 30744
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cMaxColliders = 1024;

	// Token: 0x04007819 RID: 30745
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Collider[] colliderList = new Collider[1024];
}
