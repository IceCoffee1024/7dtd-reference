using System;
using UnityEngine;

// Token: 0x02000516 RID: 1302
public class RootTransformRefEntity : RootTransformRef
{
	// Token: 0x06002A6D RID: 10861 RVA: 0x0010CAD5 File Offset: 0x0010ACD5
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		if (this.RootTransform == null)
		{
			this.RootTransform = RootTransformRefEntity.FindEntityUpwards(base.transform);
		}
	}

	// Token: 0x06002A6E RID: 10862 RVA: 0x0010CAF8 File Offset: 0x0010ACF8
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

	// Token: 0x06002A6F RID: 10863 RVA: 0x0010CB28 File Offset: 0x0010AD28
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

	// Token: 0x06002A70 RID: 10864 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunOpen()
	{
	}

	// Token: 0x06002A71 RID: 10865 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunClose()
	{
	}

	// Token: 0x06002A72 RID: 10866 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunRemoveRound()
	{
	}

	// Token: 0x06002A73 RID: 10867 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunLoadRound()
	{
	}

	// Token: 0x06002A74 RID: 10868 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunCockBack()
	{
	}

	// Token: 0x06002A75 RID: 10869 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunCockForward()
	{
	}
}
