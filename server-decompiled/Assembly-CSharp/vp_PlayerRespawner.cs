using System;
using UnityEngine;

// Token: 0x0200162C RID: 5676
public class vp_PlayerRespawner : vp_Respawner
{
	// Token: 0x170014FA RID: 5370
	// (get) Token: 0x0600B1EA RID: 45546 RVA: 0x0042746D File Offset: 0x0042566D
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

	// Token: 0x0600B1EB RID: 45547 RVA: 0x00427494 File Offset: 0x00425694
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x0600B1EC RID: 45548 RVA: 0x0042749C File Offset: 0x0042569C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
		base.OnEnable();
	}

	// Token: 0x0600B1ED RID: 45549 RVA: 0x004274BE File Offset: 0x004256BE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B1EE RID: 45550 RVA: 0x004274DC File Offset: 0x004256DC
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

	// Token: 0x0400859F RID: 34207
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;
}
