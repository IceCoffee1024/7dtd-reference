using System;
using UnityEngine;

// Token: 0x020015CD RID: 5581
public class vp_DoomsDayDevice : MonoBehaviour
{
	// Token: 0x0600AE03 RID: 44547 RVA: 0x0040CAD8 File Offset: 0x0040ACD8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = global::UnityEngine.Object.FindAnyObjectByType<vp_FPPlayerEventHandler>();
		if (this.m_Player != null)
		{
			this.m_PlayerAudioSource = this.m_Player.GetComponent<AudioSource>();
		}
		this.m_DeviceAudioSource = base.GetComponent<AudioSource>();
		this.m_Button = GameObject.Find("ForbiddenButton");
		if (this.m_Button != null)
		{
			this.m_OriginalButtonPos = this.m_Button.transform.localPosition;
			this.m_OriginalButtonColor = this.m_Button.GetComponent<Renderer>().material.color;
		}
		this.m_PulsingLight = this.m_Button.GetComponentInChildren<vp_PulsingLight>();
		if (this.m_PulsingLight != null)
		{
			this.m_OriginalPulsingLightMaxIntensity = this.m_PulsingLight.m_MaxIntensity;
		}
	}

	// Token: 0x0600AE04 RID: 44548 RVA: 0x0040CB9C File Offset: 0x0040AD9C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
		if (this.m_Button != null)
		{
			this.m_Button.transform.localPosition = this.m_OriginalButtonPos;
			this.m_Button.GetComponent<Renderer>().material.color = this.m_OriginalButtonColor;
		}
		if (this.m_DeviceAudioSource != null)
		{
			this.m_DeviceAudioSource.pitch = 1f;
			this.m_DeviceAudioSource.volume = 1f;
		}
		if (this.m_PulsingLight != null)
		{
			this.m_PulsingLight.m_MaxIntensity = this.m_OriginalPulsingLightMaxIntensity;
		}
	}

	// Token: 0x0600AE05 RID: 44549 RVA: 0x0040CC4F File Offset: 0x0040AE4F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600AE06 RID: 44550 RVA: 0x0040CC6C File Offset: 0x0040AE6C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (this.Initiated)
		{
			if (this.m_Button != null)
			{
				this.m_Button.GetComponent<Renderer>().material.color = Color.Lerp(this.m_Button.GetComponent<Renderer>().material.color, this.m_OriginalButtonColor * 0.2f, Time.deltaTime * 1.5f);
			}
			if (this.m_DeviceAudioSource != null)
			{
				this.m_DeviceAudioSource.pitch -= Time.deltaTime * 0.35f;
			}
			if (this.m_PulsingLight != null)
			{
				this.m_PulsingLight.m_MaxIntensity = 2.5f;
			}
		}
	}

	// Token: 0x0600AE07 RID: 44551 RVA: 0x0040CD28 File Offset: 0x0040AF28
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InitiateDoomsDay()
	{
		if (this.Initiated)
		{
			return;
		}
		this.Initiated = true;
		if (this.m_Button != null)
		{
			this.m_Button.transform.localPosition += Vector3.down * 0.02f;
		}
		if (this.m_PlayerAudioSource != null)
		{
			this.m_PlayerAudioSource.PlayOneShot(this.EarthQuakeSound);
		}
		this.m_Player.CameraEarthQuake.TryStart<Vector3>(new Vector3(0.05f, 0.05f, 10f));
		vp_Timer.In(3f, delegate
		{
			base.SendMessage("Die");
		}, null);
		vp_Timer.In(3f, delegate
		{
			this.Initiated = false;
		}, null);
	}

	// Token: 0x040080AD RID: 32941
	public AudioClip EarthQuakeSound;

	// Token: 0x040080AE RID: 32942
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x040080AF RID: 32943
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool Initiated;

	// Token: 0x040080B0 RID: 32944
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_Button;

	// Token: 0x040080B1 RID: 32945
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PulsingLight m_PulsingLight;

	// Token: 0x040080B2 RID: 32946
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_PlayerAudioSource;

	// Token: 0x040080B3 RID: 32947
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_DeviceAudioSource;

	// Token: 0x040080B4 RID: 32948
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_OriginalButtonPos;

	// Token: 0x040080B5 RID: 32949
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_OriginalButtonColor;

	// Token: 0x040080B6 RID: 32950
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_OriginalPulsingLightMaxIntensity;
}
