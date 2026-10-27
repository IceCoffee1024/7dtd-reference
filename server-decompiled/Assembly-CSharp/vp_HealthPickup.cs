using System;
using UnityEngine;

// Token: 0x02001618 RID: 5656
public class vp_HealthPickup : vp_Pickup
{
	// Token: 0x0600AFEE RID: 45038 RVA: 0x00419CE8 File Offset: 0x00417EE8
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

	// Token: 0x04008353 RID: 33619
	public float Health = 1f;
}
