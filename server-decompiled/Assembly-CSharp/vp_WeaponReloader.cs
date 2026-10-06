using System;
using UnityEngine;

// Token: 0x02001634 RID: 5684
public class vp_WeaponReloader : MonoBehaviour
{
	// Token: 0x0600B24E RID: 45646 RVA: 0x00428E58 File Offset: 0x00427058
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Audio = base.GetComponent<AudioSource>();
		this.m_Player = (vp_PlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_PlayerEventHandler));
	}

	// Token: 0x0600B24F RID: 45647 RVA: 0x00428E8B File Offset: 0x0042708B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.m_Weapon = base.transform.GetComponent<vp_Weapon>();
	}

	// Token: 0x0600B250 RID: 45648 RVA: 0x00428E9E File Offset: 0x0042709E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B251 RID: 45649 RVA: 0x00428EBA File Offset: 0x004270BA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B252 RID: 45650 RVA: 0x00428ED8 File Offset: 0x004270D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Reload()
	{
		return this.m_Player.CurrentWeaponWielded.Get() && (this.m_Player.CurrentWeaponMaxAmmoCount.Get() == 0 || this.m_Player.CurrentWeaponAmmoCount.Get() != this.m_Player.CurrentWeaponMaxAmmoCount.Get()) && this.m_Player.CurrentWeaponClipCount.Get() >= 1;
	}

	// Token: 0x0600B253 RID: 45651 RVA: 0x00428F60 File Offset: 0x00427160
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Reload()
	{
		this.m_Player.Reload.AutoDuration = this.m_Player.CurrentWeaponReloadDuration.Get();
		if (this.m_Audio != null)
		{
			this.m_Audio.pitch = Time.timeScale;
			this.m_Audio.PlayOneShot(this.SoundReload);
		}
	}

	// Token: 0x0600B254 RID: 45652 RVA: 0x00428FC1 File Offset: 0x004271C1
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Reload()
	{
		this.m_Player.RefillCurrentWeapon.Try();
	}

	// Token: 0x17001512 RID: 5394
	// (get) Token: 0x0600B255 RID: 45653 RVA: 0x00428FD9 File Offset: 0x004271D9
	public virtual float OnValue_CurrentWeaponReloadDuration
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.ReloadDuration;
		}
	}

	// Token: 0x040085E0 RID: 34272
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Weapon m_Weapon;

	// Token: 0x040085E1 RID: 34273
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x040085E2 RID: 34274
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040085E3 RID: 34275
	public AudioClip SoundReload;

	// Token: 0x040085E4 RID: 34276
	public float ReloadDuration = 1f;
}
