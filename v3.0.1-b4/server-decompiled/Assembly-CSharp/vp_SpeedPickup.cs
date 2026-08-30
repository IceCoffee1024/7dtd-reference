using System;

// Token: 0x020015CF RID: 5583
public class vp_SpeedPickup : vp_Pickup
{
	// Token: 0x0600AE15 RID: 44565 RVA: 0x0040F160 File Offset: 0x0040D360
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		this.UpdateMotion();
		if (this.m_Depleted && !this.m_Audio.isPlaying)
		{
			this.Remove();
		}
	}

	// Token: 0x0600AE16 RID: 44566 RVA: 0x0040F184 File Offset: 0x0040D384
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

	// Token: 0x04008204 RID: 33284
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer = new vp_Timer.Handle();
}
