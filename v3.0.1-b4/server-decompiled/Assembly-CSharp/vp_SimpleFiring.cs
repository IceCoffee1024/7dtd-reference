using System;
using UnityEngine;

// Token: 0x020015F4 RID: 5620
public class vp_SimpleFiring : MonoBehaviour
{
	// Token: 0x0600B076 RID: 45174 RVA: 0x0041EBFD File Offset: 0x0041CDFD
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = (vp_PlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_PlayerEventHandler));
	}

	// Token: 0x0600B077 RID: 45175 RVA: 0x0041EC24 File Offset: 0x0041CE24
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B078 RID: 45176 RVA: 0x0041EC40 File Offset: 0x0041CE40
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B079 RID: 45177 RVA: 0x0041EC5C File Offset: 0x0041CE5C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (this.m_Player.Attack.Active)
		{
			this.m_Player.Fire.Try();
		}
	}

	// Token: 0x04008484 RID: 33924
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;
}
