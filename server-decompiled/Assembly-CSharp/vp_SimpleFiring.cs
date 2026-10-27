using System;
using UnityEngine;

// Token: 0x02001640 RID: 5696
public class vp_SimpleFiring : MonoBehaviour
{
	// Token: 0x0600B25D RID: 45661 RVA: 0x0042A01D File Offset: 0x0042821D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = (vp_PlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_PlayerEventHandler));
	}

	// Token: 0x0600B25E RID: 45662 RVA: 0x0042A044 File Offset: 0x00428244
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B25F RID: 45663 RVA: 0x0042A060 File Offset: 0x00428260
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B260 RID: 45664 RVA: 0x0042A07C File Offset: 0x0042827C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (this.m_Player.Attack.Active)
		{
			this.m_Player.Fire.Try();
		}
	}

	// Token: 0x040085F4 RID: 34292
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;
}
