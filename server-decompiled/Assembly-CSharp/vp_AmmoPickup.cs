using System;

// Token: 0x020015E6 RID: 5606
public class vp_AmmoPickup : vp_Pickup
{
	// Token: 0x0600AF01 RID: 44801 RVA: 0x00413B50 File Offset: 0x00411D50
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

	// Token: 0x0600AF02 RID: 44802 RVA: 0x00413BA4 File Offset: 0x00411DA4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryReloadIfEmpty(vp_FPPlayerEventHandler player)
	{
		return player.CurrentWeaponAmmoCount.Get() <= 0 && !(player.CurrentWeaponClipType.Get() != this.InventoryName) && player.Reload.TryStart(true);
	}

	// Token: 0x040082BB RID: 33467
	public int GiveAmount = 1;
}
