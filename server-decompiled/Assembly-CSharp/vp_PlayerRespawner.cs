using System;
using UnityEngine;

// Token: 0x0200163E RID: 5694
public class vp_PlayerRespawner : vp_Respawner
{
	// Token: 0x17001506 RID: 5382
	// (get) Token: 0x0600B242 RID: 45634 RVA: 0x00429871 File Offset: 0x00427A71
	public vp_PlayerEventHandler Player
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Player == null)
			{
				this.m_Player = base.transform.GetComponent<vp_PlayerEventHandler>();
			}
			return this.m_Player;
		}
	}

	// Token: 0x0600B243 RID: 45635 RVA: 0x00429898 File Offset: 0x00427A98
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x0600B244 RID: 45636 RVA: 0x004298A0 File Offset: 0x00427AA0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
		base.OnEnable();
	}

	// Token: 0x0600B245 RID: 45637 RVA: 0x004298C2 File Offset: 0x00427AC2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B246 RID: 45638 RVA: 0x004298E0 File Offset: 0x00427AE0
	public override void Reset()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.Player == null)
		{
			return;
		}
		this.Player.Position.Set(this.Placement.Position);
		this.Player.Rotation.Set(this.Placement.Rotation.eulerAngles);
		this.Player.Stop.Send();
	}

	// Token: 0x040085E0 RID: 34272
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;
}
