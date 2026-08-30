using System;
using UnityEngine;

// Token: 0x0200149F RID: 5279
public abstract class ScriptBase : MonoBehaviour
{
	// Token: 0x1700137F RID: 4991
	// (get) Token: 0x0600A59A RID: 42394 RVA: 0x003D9E48 File Offset: 0x003D8048
	public new Transform transform
	{
		get
		{
			if (this.myTransform != null)
			{
				return this.myTransform;
			}
			return this.myTransform = base.transform;
		}
	}

	// Token: 0x17001380 RID: 4992
	// (get) Token: 0x0600A59B RID: 42395 RVA: 0x003D9E7C File Offset: 0x003D807C
	public new GameObject gameObject
	{
		get
		{
			if (this.myGameObject != null)
			{
				return this.myGameObject;
			}
			return this.myGameObject = base.gameObject;
		}
	}

	// Token: 0x0600A59C RID: 42396 RVA: 0x003D9EAD File Offset: 0x003D80AD
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.className = base.GetType().Name;
		this.sbAwake();
	}

	// Token: 0x0600A59D RID: 42397 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void sbAwake()
	{
	}

	// Token: 0x0600A59E RID: 42398 RVA: 0x003D9EC6 File Offset: 0x003D80C6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		this.sbUpdate();
	}

	// Token: 0x0600A59F RID: 42399 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void sbUpdate()
	{
	}

	// Token: 0x0600A5A0 RID: 42400 RVA: 0x003D9ECE File Offset: 0x003D80CE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void FixedUpdate()
	{
		this.sbFixedUpdate();
	}

	// Token: 0x0600A5A1 RID: 42401 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void sbFixedUpdate()
	{
	}

	// Token: 0x0600A5A2 RID: 42402 RVA: 0x003D9ED6 File Offset: 0x003D80D6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void LateUpdate()
	{
		this.sbLateUpdate();
	}

	// Token: 0x0600A5A3 RID: 42403 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void sbLateUpdate()
	{
	}

	// Token: 0x0600A5A4 RID: 42404 RVA: 0x00002C47 File Offset: 0x00000E47
	[PublicizedFrom(EAccessModifier.Protected)]
	public ScriptBase()
	{
	}

	// Token: 0x04007B6F RID: 31599
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform myTransform;

	// Token: 0x04007B70 RID: 31600
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject myGameObject;

	// Token: 0x04007B71 RID: 31601
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string className;
}
