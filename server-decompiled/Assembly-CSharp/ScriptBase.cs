using System;
using UnityEngine;

// Token: 0x02001484 RID: 5252
public abstract class ScriptBase : MonoBehaviour
{
	// Token: 0x17001346 RID: 4934
	// (get) Token: 0x0600A49D RID: 42141 RVA: 0x003D4B04 File Offset: 0x003D2D04
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

	// Token: 0x17001347 RID: 4935
	// (get) Token: 0x0600A49E RID: 42142 RVA: 0x003D4B38 File Offset: 0x003D2D38
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

	// Token: 0x0600A49F RID: 42143 RVA: 0x003D4B69 File Offset: 0x003D2D69
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.className = base.GetType().Name;
		this.sbAwake();
	}

	// Token: 0x0600A4A0 RID: 42144 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void sbAwake()
	{
	}

	// Token: 0x0600A4A1 RID: 42145 RVA: 0x003D4B82 File Offset: 0x003D2D82
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		this.sbUpdate();
	}

	// Token: 0x0600A4A2 RID: 42146 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void sbUpdate()
	{
	}

	// Token: 0x0600A4A3 RID: 42147 RVA: 0x003D4B8A File Offset: 0x003D2D8A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void FixedUpdate()
	{
		this.sbFixedUpdate();
	}

	// Token: 0x0600A4A4 RID: 42148 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void sbFixedUpdate()
	{
	}

	// Token: 0x0600A4A5 RID: 42149 RVA: 0x003D4B92 File Offset: 0x003D2D92
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void LateUpdate()
	{
		this.sbLateUpdate();
	}

	// Token: 0x0600A4A6 RID: 42150 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void sbLateUpdate()
	{
	}

	// Token: 0x0600A4A7 RID: 42151 RVA: 0x00002C47 File Offset: 0x00000E47
	[PublicizedFrom(EAccessModifier.Protected)]
	public ScriptBase()
	{
	}

	// Token: 0x04007A96 RID: 31382
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform myTransform;

	// Token: 0x04007A97 RID: 31383
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject myGameObject;

	// Token: 0x04007A98 RID: 31384
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string className;
}
