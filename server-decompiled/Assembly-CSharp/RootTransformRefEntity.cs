using System;
using UnityEngine;

// Token: 0x0200050E RID: 1294
public class RootTransformRefEntity : RootTransformRef
{
	// Token: 0x06002A71 RID: 10865 RVA: 0x0010BC49 File Offset: 0x00109E49
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		if (this.RootTransform == null)
		{
			this.RootTransform = RootTransformRefEntity.FindEntityUpwards(base.transform);
		}
	}

	// Token: 0x06002A72 RID: 10866 RVA: 0x0010BC6C File Offset: 0x00109E6C
	public static RootTransformRefEntity AddIfEntity(Transform _t)
	{
		Transform transform = RootTransformRefEntity.FindEntityUpwards(_t);
		if (transform)
		{
			RootTransformRefEntity rootTransformRefEntity = _t.gameObject.AddMissingComponent<RootTransformRefEntity>();
			rootTransformRefEntity.RootTransform = transform;
			return rootTransformRefEntity;
		}
		return null;
	}

	// Token: 0x06002A73 RID: 10867 RVA: 0x0010BC9C File Offset: 0x00109E9C
	public static Transform FindEntityUpwards(Transform _t)
	{
		while (!_t.GetComponent<Entity>())
		{
			_t = _t.parent;
			if (!_t)
			{
				return null;
			}
		}
		return _t;
	}

	// Token: 0x06002A74 RID: 10868 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunOpen()
	{
	}

	// Token: 0x06002A75 RID: 10869 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunClose()
	{
	}

	// Token: 0x06002A76 RID: 10870 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunRemoveRound()
	{
	}

	// Token: 0x06002A77 RID: 10871 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunLoadRound()
	{
	}

	// Token: 0x06002A78 RID: 10872 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunCockBack()
	{
	}

	// Token: 0x06002A79 RID: 10873 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunCockForward()
	{
	}
}
