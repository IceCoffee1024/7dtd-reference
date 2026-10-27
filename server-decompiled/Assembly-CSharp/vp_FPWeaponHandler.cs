using System;

// Token: 0x02001632 RID: 5682
public class vp_FPWeaponHandler : vp_WeaponHandler
{
	// Token: 0x0600B19A RID: 45466 RVA: 0x00425243 File Offset: 0x00423443
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_AutoReload()
	{
		return this.ReloadAutomatically && this.m_Player.Reload.TryStart(true);
	}
}
