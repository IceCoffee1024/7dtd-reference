using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015C5 RID: 5573
[RequireComponent(typeof(AudioSource))]
public class vp_Debris : MonoBehaviour
{
	// Token: 0x0600AE0A RID: 44554 RVA: 0x0040EEB4 File Offset: 0x0040D0B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_Audio = base.GetComponent<AudioSource>();
		this.m_Colliders = base.GetComponentsInChildren<Collider>();
		foreach (Collider collider in this.m_Colliders)
		{
			if (collider.GetComponent<Rigidbody>())
			{
				this.m_PiecesInitial.Add(collider, new Dictionary<string, object>
				{
					{
						"Position",
						collider.transform.localPosition
					},
					{
						"Rotation",
						collider.transform.localRotation
					}
				});
			}
		}
	}

	// Token: 0x0600AE0B RID: 44555 RVA: 0x0040EF48 File Offset: 0x0040D148
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.m_Destroy = false;
		this.m_Audio.playOnAwake = true;
		foreach (Collider collider in this.m_Colliders)
		{
			if (collider.GetComponent<Rigidbody>())
			{
				collider.transform.localPosition = (Vector3)this.m_PiecesInitial[collider]["Position"];
				collider.transform.localRotation = (Quaternion)this.m_PiecesInitial[collider]["Rotation"];
				collider.GetComponent<Rigidbody>().velocity = Vector3.zero;
				collider.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				collider.GetComponent<Rigidbody>().AddExplosionForce(this.Force / Time.timeScale / vp_TimeUtility.AdjustedTimeScale, base.transform.position, this.Radius, this.UpForce);
				Collider c = collider;
				vp_Timer.In(global::UnityEngine.Random.Range(this.LifeTime * 0.5f, this.LifeTime * 0.95f), delegate
				{
					if (c != null)
					{
						vp_Utility.Destroy(c.gameObject);
					}
				}, null);
			}
		}
		vp_Timer.In(this.LifeTime, delegate
		{
			this.m_Destroy = true;
		}, null);
		if (this.Sounds.Count > 0)
		{
			this.m_Audio.rolloffMode = AudioRolloffMode.Linear;
			this.m_Audio.clip = this.Sounds[global::UnityEngine.Random.Range(0, this.Sounds.Count)];
			this.m_Audio.pitch = global::UnityEngine.Random.Range(this.SoundMinPitch, this.SoundMaxPitch) * Time.timeScale;
			this.m_Audio.Play();
		}
	}

	// Token: 0x0600AE0C RID: 44556 RVA: 0x0040F0F9 File Offset: 0x0040D2F9
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.m_Destroy && !base.GetComponent<AudioSource>().isPlaying)
		{
			vp_Utility.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400815D RID: 33117
	public float Radius = 2f;

	// Token: 0x0400815E RID: 33118
	public float Force = 10f;

	// Token: 0x0400815F RID: 33119
	public float UpForce = 1f;

	// Token: 0x04008160 RID: 33120
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04008161 RID: 33121
	public List<AudioClip> Sounds = new List<AudioClip>();

	// Token: 0x04008162 RID: 33122
	public float SoundMinPitch = 0.8f;

	// Token: 0x04008163 RID: 33123
	public float SoundMaxPitch = 1.2f;

	// Token: 0x04008164 RID: 33124
	public float LifeTime = 5f;

	// Token: 0x04008165 RID: 33125
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Destroy;

	// Token: 0x04008166 RID: 33126
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider[] m_Colliders;

	// Token: 0x04008167 RID: 33127
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Collider, Dictionary<string, object>> m_PiecesInitial = new Dictionary<Collider, Dictionary<string, object>>();
}
