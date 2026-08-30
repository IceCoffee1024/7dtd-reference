using System;
using UnityEngine;

// Token: 0x020015F2 RID: 5618
public class vp_PlayerRespawner : vp_Respawner
{
	// Token: 0x1700147E RID: 5246
	// (get) Token: 0x0600B05B RID: 45147 RVA: 0x0041E451 File Offset: 0x0041C651
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

	// Token: 0x0600B05C RID: 45148 RVA: 0x0041E478 File Offset: 0x0041C678
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x0600B05D RID: 45149 RVA: 0x0041E480 File Offset: 0x0041C680
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
		base.OnEnable();
	}

	// Token: 0x0600B05E RID: 45150 RVA: 0x0041E4A2 File Offset: 0x0041C6A2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B05F RID: 45151 RVA: 0x0041E4C0 File Offset: 0x0041C6C0
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

	// Token: 0x04008470 RID: 33904
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;
}
