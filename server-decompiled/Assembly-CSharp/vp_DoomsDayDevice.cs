using System;
using UnityEngine;

// Token: 0x020015BB RID: 5563
public class vp_DoomsDayDevice : MonoBehaviour
{
	// Token: 0x0600ADAB RID: 44459 RVA: 0x0040A6D4 File Offset: 0x004088D4
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

	// Token: 0x0600ADAC RID: 44460 RVA: 0x0040A798 File Offset: 0x00408998
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

	// Token: 0x0600ADAD RID: 44461 RVA: 0x0040A84B File Offset: 0x00408A4B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600ADAE RID: 44462 RVA: 0x0040A868 File Offset: 0x00408A68
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

	// Token: 0x0600ADAF RID: 44463 RVA: 0x0040A924 File Offset: 0x00408B24
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

	// Token: 0x0400806C RID: 32876
	public AudioClip EarthQuakeSound;

	// Token: 0x0400806D RID: 32877
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x0400806E RID: 32878
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool Initiated;

	// Token: 0x0400806F RID: 32879
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_Button;

	// Token: 0x04008070 RID: 32880
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PulsingLight m_PulsingLight;

	// Token: 0x04008071 RID: 32881
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_PlayerAudioSource;

	// Token: 0x04008072 RID: 32882
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_DeviceAudioSource;

	// Token: 0x04008073 RID: 32883
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_OriginalButtonPos;

	// Token: 0x04008074 RID: 32884
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_OriginalButtonColor;

	// Token: 0x04008075 RID: 32885
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_OriginalPulsingLightMaxIntensity;
}
