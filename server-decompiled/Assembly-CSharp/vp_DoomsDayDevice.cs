using System;
using UnityEngine;

// Token: 0x02001599 RID: 5529
public class vp_DoomsDayDevice : MonoBehaviour
{
	// Token: 0x0600ACF8 RID: 44280 RVA: 0x00405880 File Offset: 0x00403A80
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

	// Token: 0x0600ACF9 RID: 44281 RVA: 0x00405944 File Offset: 0x00403B44
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

	// Token: 0x0600ACFA RID: 44282 RVA: 0x004059F7 File Offset: 0x00403BF7
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600ACFB RID: 44283 RVA: 0x00405A14 File Offset: 0x00403C14
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

	// Token: 0x0600ACFC RID: 44284 RVA: 0x00405AD0 File Offset: 0x00403CD0
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

	// Token: 0x04007FE9 RID: 32745
	public AudioClip EarthQuakeSound;

	// Token: 0x04007FEA RID: 32746
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x04007FEB RID: 32747
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool Initiated;

	// Token: 0x04007FEC RID: 32748
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_Button;

	// Token: 0x04007FED RID: 32749
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PulsingLight m_PulsingLight;

	// Token: 0x04007FEE RID: 32750
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_PlayerAudioSource;

	// Token: 0x04007FEF RID: 32751
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_DeviceAudioSource;

	// Token: 0x04007FF0 RID: 32752
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_OriginalButtonPos;

	// Token: 0x04007FF1 RID: 32753
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_OriginalButtonColor;

	// Token: 0x04007FF2 RID: 32754
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_OriginalPulsingLightMaxIntensity;
}
