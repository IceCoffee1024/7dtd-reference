using System;

// Token: 0x020015CB RID: 5579
public class vp_AmmoPickup : vp_Pickup
{
	// Token: 0x0600AE04 RID: 44548 RVA: 0x0040E810 File Offset: 0x0040CA10
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

	// Token: 0x0600AE05 RID: 44549 RVA: 0x0040E864 File Offset: 0x0040CA64
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryReloadIfEmpty(vp_FPPlayerEventHandler player)
	{
		return player.CurrentWeaponAmmoCount.Get() <= 0 && !(player.CurrentWeaponClipType.Get() != this.InventoryName) && player.Reload.TryStart(true);
	}

	// Token: 0x040081E2 RID: 33250
	public int GiveAmount = 1;
}
