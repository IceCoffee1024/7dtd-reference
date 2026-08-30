using System;

// Token: 0x020015E6 RID: 5606
public class vp_FPWeaponHandler : vp_WeaponHandler
{
	// Token: 0x0600AFB3 RID: 44979 RVA: 0x00419E23 File Offset: 0x00418023
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_AutoReload()
	{
		return this.ReloadAutomatically && this.m_Player.Reload.TryStart(true);
	}
}
