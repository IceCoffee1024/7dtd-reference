using System;

// Token: 0x0200161D RID: 5661
public class vp_WeaponPickup : vp_Pickup
{
	// Token: 0x0600B001 RID: 45057 RVA: 0x0041A628 File Offset: 0x00418828
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

	// Token: 0x04008376 RID: 33654
	public int AmmoIncluded;
}
