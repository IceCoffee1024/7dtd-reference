using System;

// Token: 0x020015EC RID: 5612
public class vp_WeaponPickup : vp_Pickup
{
	// Token: 0x0600AF17 RID: 44823 RVA: 0x00414548 File Offset: 0x00412748
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

	// Token: 0x040082DF RID: 33503
	public int AmmoIncluded;
}
