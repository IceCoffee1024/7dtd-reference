using System;
using UnityEngine;

// Token: 0x02001496 RID: 5270
public class RootTransformRefParent : RootTransformRef
{
	// Token: 0x0600A55B RID: 42331 RVA: 0x003D875C File Offset: 0x003D695C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		if (this.RootTransform)
		{
			return;
		}
		this.RootTransform = this.FindTopTransform(base.transform);
	}

	// Token: 0x0600A55C RID: 42332 RVA: 0x003D8780 File Offset: 0x003D6980
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

	// Token: 0x0600A55D RID: 42333 RVA: 0x003D87A4 File Offset: 0x003D69A4
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
