using System;
using UnityEngine;

// Token: 0x02000514 RID: 1300
public class RootTransformRefEntity : RootTransformRef
{
	// Token: 0x06002A66 RID: 10854 RVA: 0x0010C581 File Offset: 0x0010A781
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		if (this.RootTransform == null)
		{
			this.RootTransform = RootTransformRefEntity.FindEntityUpwards(base.transform);
		}
	}

	// Token: 0x06002A67 RID: 10855 RVA: 0x0010C5A4 File Offset: 0x0010A7A4
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

	// Token: 0x06002A68 RID: 10856 RVA: 0x0010C5D4 File Offset: 0x0010A7D4
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

	// Token: 0x06002A69 RID: 10857 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunOpen()
	{
	}

	// Token: 0x06002A6A RID: 10858 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunClose()
	{
	}

	// Token: 0x06002A6B RID: 10859 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunRemoveRound()
	{
	}

	// Token: 0x06002A6C RID: 10860 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunLoadRound()
	{
	}

	// Token: 0x06002A6D RID: 10861 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunCockBack()
	{
	}

	// Token: 0x06002A6E RID: 10862 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunCockForward()
	{
	}
}
