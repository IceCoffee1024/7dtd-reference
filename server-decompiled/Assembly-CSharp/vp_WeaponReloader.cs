using System;
using UnityEngine;

// Token: 0x02001615 RID: 5653
public class vp_WeaponReloader : MonoBehaviour
{
	// Token: 0x0600B1BC RID: 45500 RVA: 0x0042517C File Offset: 0x0042337C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Audio = base.GetComponent<AudioSource>();
		this.m_Player = (vp_PlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_PlayerEventHandler));
	}

	// Token: 0x0600B1BD RID: 45501 RVA: 0x004251AF File Offset: 0x004233AF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.m_Weapon = base.transform.GetComponent<vp_Weapon>();
	}

	// Token: 0x0600B1BE RID: 45502 RVA: 0x004251C2 File Offset: 0x004233C2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B1BF RID: 45503 RVA: 0x004251DE File Offset: 0x004233DE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B1C0 RID: 45504 RVA: 0x004251FC File Offset: 0x004233FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Reload()
	{
		return this.m_Player.CurrentWeaponWielded.Get() && (this.m_Player.CurrentWeaponMaxAmmoCount.Get() == 0 || this.m_Player.CurrentWeaponAmmoCount.Get() != this.m_Player.CurrentWeaponMaxAmmoCount.Get()) && this.m_Player.CurrentWeaponClipCount.Get() >= 1;
	}

	// Token: 0x0600B1C1 RID: 45505 RVA: 0x00425284 File Offset: 0x00423484
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

	// Token: 0x0600B1C2 RID: 45506 RVA: 0x004252E5 File Offset: 0x004234E5
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Reload()
	{
		this.m_Player.RefillCurrentWeapon.Try();
	}

	// Token: 0x170014CF RID: 5327
	// (get) Token: 0x0600B1C3 RID: 45507 RVA: 0x004252FD File Offset: 0x004234FD
	public virtual float OnValue_CurrentWeaponReloadDuration
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.ReloadDuration;
		}
	}

	// Token: 0x0400858A RID: 34186
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Weapon m_Weapon;

	// Token: 0x0400858B RID: 34187
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x0400858C RID: 34188
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x0400858D RID: 34189
	public AudioClip SoundReload;

	// Token: 0x0400858E RID: 34190
	public float ReloadDuration = 1f;
}
