using System;
using UnityEngine;

// Token: 0x020014B6 RID: 5302
public class RootTransformRefParent : RootTransformRef
{
	// Token: 0x0600A609 RID: 42505 RVA: 0x003DD3E8 File Offset: 0x003DB5E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		if (this.RootTransform)
		{
			return;
		}
		this.RootTransform = this.FindTopTransform(base.transform);
	}

	// Token: 0x0600A60A RID: 42506 RVA: 0x003DD40C File Offset: 0x003DB60C
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform FindTopTransform(Transform _t)
	{
		Transform parent;
		while ((parent = _t.parent) != null)
		{
			_t = parent;
		}
		return _t;
	}

	// Token: 0x0600A60B RID: 42507 RVA: 0x003DD430 File Offset: 0x003DB630
	public static Transform FindRoot(Transform _t)
	{
		Transform transform = _t;
		RootTransformRefParent rootTransformRefParent;
		while (!transform.TryGetComponent<RootTransformRefParent>(out rootTransformRefParent))
		{
			transform = transform.parent;
			if (!transform)
			{
				return _t;
			}
		}
		return rootTransformRefParent.RootTransform;
	}
}
