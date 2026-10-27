using System;

// Token: 0x0200161B RID: 5659
public class vp_SpeedPickup : vp_Pickup
{
	// Token: 0x0600AFFC RID: 45052 RVA: 0x0041A580 File Offset: 0x00418780
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		this.UpdateMotion();
		if (this.m_Depleted && !this.m_Audio.isPlaying)
		{
			this.Remove();
		}
	}

	// Token: 0x0600AFFD RID: 45053 RVA: 0x0041A5A4 File Offset: 0x004187A4
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

	// Token: 0x04008374 RID: 33652
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer = new vp_Timer.Handle();
}
