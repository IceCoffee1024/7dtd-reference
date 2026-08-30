using System;
using UnityEngine;

// Token: 0x020015FA RID: 5626
public class vp_WeaponReloader : MonoBehaviour
{
	// Token: 0x0600B0BF RID: 45247 RVA: 0x0041FE3C File Offset: 0x0041E03C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Audio = base.GetComponent<AudioSource>();
		this.m_Player = (vp_PlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_PlayerEventHandler));
	}

	// Token: 0x0600B0C0 RID: 45248 RVA: 0x0041FE6F File Offset: 0x0041E06F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.m_Weapon = base.transform.GetComponent<vp_Weapon>();
	}

	// Token: 0x0600B0C1 RID: 45249 RVA: 0x0041FE82 File Offset: 0x0041E082
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B0C2 RID: 45250 RVA: 0x0041FE9E File Offset: 0x0041E09E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B0C3 RID: 45251 RVA: 0x0041FEBC File Offset: 0x0041E0BC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Reload()
	{
		return this.m_Player.CurrentWeaponWielded.Get() && (this.m_Player.CurrentWeaponMaxAmmoCount.Get() == 0 || this.m_Player.CurrentWeaponAmmoCount.Get() != this.m_Player.CurrentWeaponMaxAmmoCount.Get()) && this.m_Player.CurrentWeaponClipCount.Get() >= 1;
	}

	// Token: 0x0600B0C4 RID: 45252 RVA: 0x0041FF44 File Offset: 0x0041E144
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

	// Token: 0x0600B0C5 RID: 45253 RVA: 0x0041FFA5 File Offset: 0x0041E1A5
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Reload()
	{
		this.m_Player.RefillCurrentWeapon.Try();
	}

	// Token: 0x17001496 RID: 5270
	// (get) Token: 0x0600B0C6 RID: 45254 RVA: 0x0041FFBD File Offset: 0x0041E1BD
	public virtual float OnValue_CurrentWeaponReloadDuration
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.ReloadDuration;
		}
	}

	// Token: 0x040084B1 RID: 33969
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Weapon m_Weapon;

	// Token: 0x040084B2 RID: 33970
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x040084B3 RID: 33971
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040084B4 RID: 33972
	public AudioClip SoundReload;

	// Token: 0x040084B5 RID: 33973
	public float ReloadDuration = 1f;
}
