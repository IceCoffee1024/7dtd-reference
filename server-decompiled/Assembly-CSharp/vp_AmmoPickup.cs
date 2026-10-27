using System;

// Token: 0x02001617 RID: 5655
public class vp_AmmoPickup : vp_Pickup
{
	// Token: 0x0600AFEB RID: 45035 RVA: 0x00419C30 File Offset: 0x00417E30
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool TryGive(vp_FPPlayerEventHandler player)
	{
		if (player.Dead.Active)
		{
			return false;
		}
		int i = 0;
		while (i < this.GiveAmount)
		{
			if (!base.TryGive(player))
			{
				if (this.TryReloadIfEmpty(player))
				{
					base.TryGive(player);
					return true;
				}
				return false;
			}
			else
			{
				i++;
			}
		}
		this.TryReloadIfEmpty(player);
		return true;
	}

	// Token: 0x0600AFEC RID: 45036 RVA: 0x00419C84 File Offset: 0x00417E84
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryReloadIfEmpty(vp_FPPlayerEventHandler player)
	{
		return player.CurrentWeaponAmmoCount.Get() <= 0 && !(player.CurrentWeaponClipType.Get() != this.InventoryName) && player.Reload.TryStart(true);
	}

	// Token: 0x04008352 RID: 33618
	public int GiveAmount = 1;
}
