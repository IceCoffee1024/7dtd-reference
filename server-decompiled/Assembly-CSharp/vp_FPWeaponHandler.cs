using System;

// Token: 0x02001601 RID: 5633
public class vp_FPWeaponHandler : vp_WeaponHandler
{
	// Token: 0x0600B0B0 RID: 45232 RVA: 0x0041F163 File Offset: 0x0041D363
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_AutoReload()
	{
		return this.ReloadAutomatically && this.m_Player.Reload.TryStart(true);
	}
}
