using System;
using UnityEngine;

// Token: 0x02001634 RID: 5684
[RequireComponent(typeof(vp_FPWeapon))]
public class vp_FPWeaponReloader : vp_WeaponReloader
{
	// Token: 0x0600B1AB RID: 45483 RVA: 0x004259F4 File Offset: 0x00423BF4
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

	// Token: 0x0400850E RID: 34062
	public AnimationClip AnimationReload;
}
