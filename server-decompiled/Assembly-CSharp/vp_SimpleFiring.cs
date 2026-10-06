using System;
using UnityEngine;

// Token: 0x0200162E RID: 5678
public class vp_SimpleFiring : MonoBehaviour
{
	// Token: 0x0600B205 RID: 45573 RVA: 0x00427C19 File Offset: 0x00425E19
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = (vp_PlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_PlayerEventHandler));
	}

	// Token: 0x0600B206 RID: 45574 RVA: 0x00427C40 File Offset: 0x00425E40
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B207 RID: 45575 RVA: 0x00427C5C File Offset: 0x00425E5C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B208 RID: 45576 RVA: 0x00427C78 File Offset: 0x00425E78
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (this.m_Player.Attack.Active)
		{
			this.m_Player.Fire.Try();
		}
	}

	// Token: 0x040085B3 RID: 34227
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;
}
