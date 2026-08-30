using System;
using UnityEngine;

// Token: 0x0200160D RID: 5645
public class vp_PlayerRespawner : vp_Respawner
{
	// Token: 0x170014B7 RID: 5303
	// (get) Token: 0x0600B158 RID: 45400 RVA: 0x00423791 File Offset: 0x00421991
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

	// Token: 0x0600B159 RID: 45401 RVA: 0x004237B8 File Offset: 0x004219B8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x0600B15A RID: 45402 RVA: 0x004237C0 File Offset: 0x004219C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
		base.OnEnable();
	}

	// Token: 0x0600B15B RID: 45403 RVA: 0x004237E2 File Offset: 0x004219E2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B15C RID: 45404 RVA: 0x00423800 File Offset: 0x00421A00
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

	// Token: 0x04008549 RID: 34121
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;
}
