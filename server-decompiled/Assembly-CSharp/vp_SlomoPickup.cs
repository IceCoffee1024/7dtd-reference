using System;
using UnityEngine;

// Token: 0x020015E9 RID: 5609
public class vp_SlomoPickup : vp_Pickup
{
	// Token: 0x0600AF0F RID: 44815 RVA: 0x004143D0 File Offset: 0x004125D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		this.UpdateMotion();
		if (this.m_Depleted)
		{
			if (this.m_Player != null && this.m_Player.Dead.Active && !this.m_RespawnTimer.Active)
			{
				this.Respawn();
				return;
			}
			if (Time.timeScale > 0.2f && !vp_TimeUtility.Paused)
			{
				vp_TimeUtility.FadeTimeScale(0.2f, 0.1f);
				return;
			}
			if (!this.m_Audio.isPlaying)
			{
				this.Remove();
				return;
			}
		}
		else if (Time.timeScale < 1f && !vp_TimeUtility.Paused)
		{
			vp_TimeUtility.FadeTimeScale(1f, 0.05f);
		}
	}

	// Token: 0x0600AF10 RID: 44816 RVA: 0x00414478 File Offset: 0x00412678
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool TryGive(vp_FPPlayerEventHandler player)
	{
		this.m_Player = player;
		return !this.m_Depleted && Time.timeScale == 1f;
	}

	// Token: 0x040082DC RID: 33500
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;
}
