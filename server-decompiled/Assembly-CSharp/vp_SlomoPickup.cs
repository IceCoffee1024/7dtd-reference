using System;
using UnityEngine;

// Token: 0x0200161A RID: 5658
public class vp_SlomoPickup : vp_Pickup
{
	// Token: 0x0600AFF9 RID: 45049 RVA: 0x0041A4B0 File Offset: 0x004186B0
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

	// Token: 0x0600AFFA RID: 45050 RVA: 0x0041A558 File Offset: 0x00418758
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool TryGive(vp_FPPlayerEventHandler player)
	{
		this.m_Player = player;
		return !this.m_Depleted && Time.timeScale == 1f;
	}

	// Token: 0x04008373 RID: 33651
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;
}
