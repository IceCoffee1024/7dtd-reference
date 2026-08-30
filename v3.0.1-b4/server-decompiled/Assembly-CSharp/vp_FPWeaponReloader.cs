using System;
using UnityEngine;

// Token: 0x020015E8 RID: 5608
[RequireComponent(typeof(vp_FPWeapon))]
public class vp_FPWeaponReloader : vp_WeaponReloader
{
	// Token: 0x0600AFC4 RID: 44996 RVA: 0x0041A5D4 File Offset: 0x004187D4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnStart_Reload()
	{
		base.OnStart_Reload();
		if (this.AnimationReload == null)
		{
			return;
		}
		if (this.m_Player.Reload.AutoDuration == 0f)
		{
			this.m_Player.Reload.AutoDuration = this.AnimationReload.length;
		}
		((vp_FPWeapon)this.m_Weapon).WeaponModel.GetComponent<Animation>().CrossFade(this.AnimationReload.name);
	}

	// Token: 0x0400839E RID: 33694
	public AnimationClip AnimationReload;
}
