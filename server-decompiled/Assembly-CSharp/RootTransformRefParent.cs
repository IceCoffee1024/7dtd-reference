using System;
using UnityEngine;

// Token: 0x020014C8 RID: 5320
public class RootTransformRefParent : RootTransformRef
{
	// Token: 0x0600A661 RID: 42593 RVA: 0x003DF7EC File Offset: 0x003DD9EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		if (this.RootTransform)
		{
			return;
		}
		this.RootTransform = this.FindTopTransform(base.transform);
	}

	// Token: 0x0600A662 RID: 42594 RVA: 0x003DF810 File Offset: 0x003DDA10
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

	// Token: 0x0600A663 RID: 42595 RVA: 0x003DF834 File Offset: 0x003DDA34
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
