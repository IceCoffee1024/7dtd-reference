using System;

// Token: 0x020015D1 RID: 5585
public class vp_WeaponPickup : vp_Pickup
{
	// Token: 0x0600AE1A RID: 44570 RVA: 0x0040F208 File Offset: 0x0040D408
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool TryGive(vp_FPPlayerEventHandler player)
	{
		if (player.Dead.Active)
		{
			return false;
		}
		if (!base.TryGive(player))
		{
			return false;
		}
		player.SetWeaponByName.Try(this.InventoryName);
		if (this.AmmoIncluded > 0)
		{
			player.AddAmmo.Try(new object[] { this.InventoryName, this.AmmoIncluded });
		}
		return true;
	}

	// Token: 0x04008206 RID: 33286
	public int AmmoIncluded;
}
