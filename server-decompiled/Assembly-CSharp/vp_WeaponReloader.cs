using System;
using UnityEngine;

// Token: 0x02001646 RID: 5702
public class vp_WeaponReloader : MonoBehaviour
{
	// Token: 0x0600B2A6 RID: 45734 RVA: 0x0042B25C File Offset: 0x0042945C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Audio = base.GetComponent<AudioSource>();
		this.m_Player = (vp_PlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_PlayerEventHandler));
	}

	// Token: 0x0600B2A7 RID: 45735 RVA: 0x0042B28F File Offset: 0x0042948F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.m_Weapon = base.transform.GetComponent<vp_Weapon>();
	}

	// Token: 0x0600B2A8 RID: 45736 RVA: 0x0042B2A2 File Offset: 0x004294A2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B2A9 RID: 45737 RVA: 0x0042B2BE File Offset: 0x004294BE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B2AA RID: 45738 RVA: 0x0042B2DC File Offset: 0x004294DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Reload()
	{
		return this.m_Player.CurrentWeaponWielded.Get() && (this.m_Player.CurrentWeaponMaxAmmoCount.Get() == 0 || this.m_Player.CurrentWeaponAmmoCount.Get() != this.m_Player.CurrentWeaponMaxAmmoCount.Get()) && this.m_Player.CurrentWeaponClipCount.Get() >= 1;
	}

	// Token: 0x0600B2AB RID: 45739 RVA: 0x0042B364 File Offset: 0x00429564
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

	// Token: 0x0600B2AC RID: 45740 RVA: 0x0042B3C5 File Offset: 0x004295C5
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Reload()
	{
		this.m_Player.RefillCurrentWeapon.Try();
	}

	// Token: 0x1700151E RID: 5406
	// (get) Token: 0x0600B2AD RID: 45741 RVA: 0x0042B3DD File Offset: 0x004295DD
	public virtual float OnValue_CurrentWeaponReloadDuration
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.ReloadDuration;
		}
	}

	// Token: 0x04008621 RID: 34337
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Weapon m_Weapon;

	// Token: 0x04008622 RID: 34338
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x04008623 RID: 34339
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04008624 RID: 34340
	public AudioClip SoundReload;

	// Token: 0x04008625 RID: 34341
	public float ReloadDuration = 1f;
}
