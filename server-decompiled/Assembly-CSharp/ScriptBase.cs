using System;
using UnityEngine;

// Token: 0x020014D1 RID: 5329
public abstract class ScriptBase : MonoBehaviour
{
	// Token: 0x170013D0 RID: 5072
	// (get) Token: 0x0600A6A0 RID: 42656 RVA: 0x003E0ED8 File Offset: 0x003DF0D8
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

	// Token: 0x170013D1 RID: 5073
	// (get) Token: 0x0600A6A1 RID: 42657 RVA: 0x003E0F0C File Offset: 0x003DF10C
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

	// Token: 0x0600A6A2 RID: 42658 RVA: 0x003E0F3D File Offset: 0x003DF13D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.className = base.GetType().Name;
		this.sbAwake();
	}

	// Token: 0x0600A6A3 RID: 42659 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void sbAwake()
	{
	}

	// Token: 0x0600A6A4 RID: 42660 RVA: 0x003E0F56 File Offset: 0x003DF156
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		this.sbUpdate();
	}

	// Token: 0x0600A6A5 RID: 42661 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void sbUpdate()
	{
	}

	// Token: 0x0600A6A6 RID: 42662 RVA: 0x003E0F5E File Offset: 0x003DF15E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void FixedUpdate()
	{
		this.sbFixedUpdate();
	}

	// Token: 0x0600A6A7 RID: 42663 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void sbFixedUpdate()
	{
	}

	// Token: 0x0600A6A8 RID: 42664 RVA: 0x003E0F66 File Offset: 0x003DF166
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void LateUpdate()
	{
		this.sbLateUpdate();
	}

	// Token: 0x0600A6A9 RID: 42665 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void sbLateUpdate()
	{
	}

	// Token: 0x0600A6AA RID: 42666 RVA: 0x00002C47 File Offset: 0x00000E47
	[PublicizedFrom(EAccessModifier.Protected)]
	public ScriptBase()
	{
	}

	// Token: 0x04007C2B RID: 31787
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform myTransform;

	// Token: 0x04007C2C RID: 31788
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject myGameObject;

	// Token: 0x04007C2D RID: 31789
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string className;
}
