using System;
using UnityEngine;

// Token: 0x020015CC RID: 5580
public class vp_HealthPickup : vp_Pickup
{
	// Token: 0x0600AE07 RID: 44551 RVA: 0x0040E8C8 File Offset: 0x0040CAC8
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

	// Token: 0x040081E3 RID: 33251
	public float Health = 1f;
}
