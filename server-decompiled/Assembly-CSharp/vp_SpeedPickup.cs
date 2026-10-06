using System;

// Token: 0x02001609 RID: 5641
public class vp_SpeedPickup : vp_Pickup
{
	// Token: 0x0600AFA4 RID: 44964 RVA: 0x0041817C File Offset: 0x0041637C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		this.UpdateMotion();
		if (this.m_Depleted && !this.m_Audio.isPlaying)
		{
			this.Remove();
		}
	}

	// Token: 0x0600AFA5 RID: 44965 RVA: 0x004181A0 File Offset: 0x004163A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool TryGive(vp_FPPlayerEventHandler player)
	{
		if (this.m_Timer.Active)
		{
			return false;
		}
		player.SetState("MegaSpeed", true, true, false);
		vp_Timer.In(this.RespawnDuration, delegate
		{
			player.SetState("MegaSpeed", false, true, false);
		}, this.m_Timer);
		return true;
	}

	// Token: 0x04008333 RID: 33587
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer = new vp_Timer.Handle();
}
