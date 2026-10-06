using System;
using UnityEngine;

// Token: 0x02001606 RID: 5638
public class vp_HealthPickup : vp_Pickup
{
	// Token: 0x0600AF96 RID: 44950 RVA: 0x004178E4 File Offset: 0x00415AE4
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

	// Token: 0x04008312 RID: 33554
	public float Health = 1f;
}
