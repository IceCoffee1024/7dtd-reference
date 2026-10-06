using System;

// Token: 0x0200160B RID: 5643
public class vp_WeaponPickup : vp_Pickup
{
	// Token: 0x0600AFA9 RID: 44969 RVA: 0x00418224 File Offset: 0x00416424
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

	// Token: 0x04008335 RID: 33589
	public int AmmoIncluded;
}
