using System;

// Token: 0x02001620 RID: 5664
public class vp_FPWeaponHandler : vp_WeaponHandler
{
	// Token: 0x0600B142 RID: 45378 RVA: 0x00422E3F File Offset: 0x0042103F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_AutoReload()
	{
		return this.ReloadAutomatically && this.m_Player.Reload.TryStart(true);
	}
}
