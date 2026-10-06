using System;
using UnityEngine;

// Token: 0x02001622 RID: 5666
[RequireComponent(typeof(vp_FPWeapon))]
public class vp_FPWeaponReloader : vp_WeaponReloader
{
	// Token: 0x0600B153 RID: 45395 RVA: 0x004235F0 File Offset: 0x004217F0
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

	// Token: 0x040084CD RID: 33997
	public AnimationClip AnimationReload;
}
