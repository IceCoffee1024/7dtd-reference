using System;
using UnityEngine;

// Token: 0x020014BF RID: 5311
public abstract class ScriptBase : MonoBehaviour
{
	// Token: 0x170013C4 RID: 5060
	// (get) Token: 0x0600A648 RID: 42568 RVA: 0x003DEAD4 File Offset: 0x003DCCD4
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

	// Token: 0x170013C5 RID: 5061
	// (get) Token: 0x0600A649 RID: 42569 RVA: 0x003DEB08 File Offset: 0x003DCD08
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

	// Token: 0x0600A64A RID: 42570 RVA: 0x003DEB39 File Offset: 0x003DCD39
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.className = base.GetType().Name;
		this.sbAwake();
	}

	// Token: 0x0600A64B RID: 42571 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void sbAwake()
	{
	}

	// Token: 0x0600A64C RID: 42572 RVA: 0x003DEB52 File Offset: 0x003DCD52
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		this.sbUpdate();
	}

	// Token: 0x0600A64D RID: 42573 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void sbUpdate()
	{
	}

	// Token: 0x0600A64E RID: 42574 RVA: 0x003DEB5A File Offset: 0x003DCD5A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void FixedUpdate()
	{
		this.sbFixedUpdate();
	}

	// Token: 0x0600A64F RID: 42575 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void sbFixedUpdate()
	{
	}

	// Token: 0x0600A650 RID: 42576 RVA: 0x003DEB62 File Offset: 0x003DCD62
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void LateUpdate()
	{
		this.sbLateUpdate();
	}

	// Token: 0x0600A651 RID: 42577 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void sbLateUpdate()
	{
	}

	// Token: 0x0600A652 RID: 42578 RVA: 0x00002C47 File Offset: 0x00000E47
	[PublicizedFrom(EAccessModifier.Protected)]
	public ScriptBase()
	{
	}

	// Token: 0x04007BEA RID: 31722
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform myTransform;

	// Token: 0x04007BEB RID: 31723
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject myGameObject;

	// Token: 0x04007BEC RID: 31724
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string className;
}
