using System;
using UnityEngine;

// Token: 0x02001603 RID: 5635
[RequireComponent(typeof(vp_FPWeapon))]
public class vp_FPWeaponReloader : vp_WeaponReloader
{
	// Token: 0x0600B0C1 RID: 45249 RVA: 0x0041F914 File Offset: 0x0041DB14
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

	// Token: 0x04008477 RID: 33911
	public AnimationClip AnimationReload;
}
