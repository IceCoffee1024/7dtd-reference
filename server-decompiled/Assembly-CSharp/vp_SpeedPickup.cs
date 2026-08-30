using System;

// Token: 0x020015EA RID: 5610
public class vp_SpeedPickup : vp_Pickup
{
	// Token: 0x0600AF12 RID: 44818 RVA: 0x004144A0 File Offset: 0x004126A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		this.UpdateMotion();
		if (this.m_Depleted && !this.m_Audio.isPlaying)
		{
			this.Remove();
		}
	}

	// Token: 0x0600AF13 RID: 44819 RVA: 0x004144C4 File Offset: 0x004126C4
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

	// Token: 0x040082DD RID: 33501
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Timer = new vp_Timer.Handle();
}
