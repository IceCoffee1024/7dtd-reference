using System;

// Token: 0x02001605 RID: 5637
public class vp_AmmoPickup : vp_Pickup
{
	// Token: 0x0600AF93 RID: 44947 RVA: 0x0041782C File Offset: 0x00415A2C
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

	// Token: 0x0600AF94 RID: 44948 RVA: 0x00417880 File Offset: 0x00415A80
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryReloadIfEmpty(vp_FPPlayerEventHandler player)
	{
		return player.CurrentWeaponAmmoCount.Get() <= 0 && !(player.CurrentWeaponClipType.Get() != this.InventoryName) && player.Reload.TryStart(true);
	}

	// Token: 0x04008311 RID: 33553
	public int GiveAmount = 1;
}
