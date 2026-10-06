using System;
using UnityEngine;

// Token: 0x02001608 RID: 5640
public class vp_SlomoPickup : vp_Pickup
{
	// Token: 0x0600AFA1 RID: 44961 RVA: 0x004180AC File Offset: 0x004162AC
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

	// Token: 0x0600AFA2 RID: 44962 RVA: 0x00418154 File Offset: 0x00416354
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool TryGive(vp_FPPlayerEventHandler player)
	{
		this.m_Player = player;
		return !this.m_Depleted && Time.timeScale == 1f;
	}

	// Token: 0x04008332 RID: 33586
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;
}
