using System;
using UnityEngine;

// Token: 0x020015E7 RID: 5607
public class vp_HealthPickup : vp_Pickup
{
	// Token: 0x0600AF04 RID: 44804 RVA: 0x00413C08 File Offset: 0x00411E08
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool TryGive(vp_FPPlayerEventHandler player)
	{
		if (player.Health.Get() < 0f)
		{
			return false;
		}
		if (player.Health.Get() >= player.MaxHealth.Get())
		{
			return false;
		}
		player.Health.Set(Mathf.Min(player.MaxHealth.Get(), player.Health.Get() + this.Health));
		return true;
	}

	// Token: 0x040082BC RID: 33468
	public float Health = 1f;
}
