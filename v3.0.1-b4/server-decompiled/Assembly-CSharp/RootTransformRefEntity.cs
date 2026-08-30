using System;
using UnityEngine;

// Token: 0x02000509 RID: 1289
public class RootTransformRefEntity : RootTransformRef
{
	// Token: 0x06002A47 RID: 10823 RVA: 0x0010AC0D File Offset: 0x00108E0D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		if (this.RootTransform == null)
		{
			this.RootTransform = RootTransformRefEntity.FindEntityUpwards(base.transform);
		}
	}

	// Token: 0x06002A48 RID: 10824 RVA: 0x0010AC30 File Offset: 0x00108E30
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

	// Token: 0x06002A49 RID: 10825 RVA: 0x0010AC60 File Offset: 0x00108E60
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

	// Token: 0x06002A4A RID: 10826 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunOpen()
	{
	}

	// Token: 0x06002A4B RID: 10827 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunClose()
	{
	}

	// Token: 0x06002A4C RID: 10828 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunRemoveRound()
	{
	}

	// Token: 0x06002A4D RID: 10829 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunLoadRound()
	{
	}

	// Token: 0x06002A4E RID: 10830 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunCockBack()
	{
	}

	// Token: 0x06002A4F RID: 10831 RVA: 0x000027FC File Offset: 0x000009FC
	public void GunCockForward()
	{
	}
}
