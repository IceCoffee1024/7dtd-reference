using System;
using UnityEngine;

// Token: 0x0200160F RID: 5647
public class vp_SimpleFiring : MonoBehaviour
{
	// Token: 0x0600B173 RID: 45427 RVA: 0x00423F3D File Offset: 0x0042213D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = (vp_PlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_PlayerEventHandler));
	}

	// Token: 0x0600B174 RID: 45428 RVA: 0x00423F64 File Offset: 0x00422164
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B175 RID: 45429 RVA: 0x00423F80 File Offset: 0x00422180
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B176 RID: 45430 RVA: 0x00423F9C File Offset: 0x0042219C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (this.m_Player.Attack.Active)
		{
			this.m_Player.Fire.Try();
		}
	}

	// Token: 0x0400855D RID: 34141
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;
}
