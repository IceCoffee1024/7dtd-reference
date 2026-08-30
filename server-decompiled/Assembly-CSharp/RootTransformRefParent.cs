using System;
using UnityEngine;

// Token: 0x0200147B RID: 5243
public class RootTransformRefParent : RootTransformRef
{
	// Token: 0x0600A45E RID: 42078 RVA: 0x003D3418 File Offset: 0x003D1618
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		if (this.RootTransform)
		{
			return;
		}
		this.RootTransform = this.FindTopTransform(base.transform);
	}

	// Token: 0x0600A45F RID: 42079 RVA: 0x003D343C File Offset: 0x003D163C
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

	// Token: 0x0600A460 RID: 42080 RVA: 0x003D3460 File Offset: 0x003D1660
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
