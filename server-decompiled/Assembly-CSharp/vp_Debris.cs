using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015D7 RID: 5591
[RequireComponent(typeof(AudioSource))]
public class vp_Debris : MonoBehaviour
{
	// Token: 0x0600AE62 RID: 44642 RVA: 0x004112B8 File Offset: 0x0040F4B8
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

	// Token: 0x0600AE63 RID: 44643 RVA: 0x0041134C File Offset: 0x0040F54C
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

	// Token: 0x0600AE64 RID: 44644 RVA: 0x004114FD File Offset: 0x0040F6FD
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.m_Destroy && !base.GetComponent<AudioSource>().isPlaying)
		{
			vp_Utility.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400819E RID: 33182
	public float Radius = 2f;

	// Token: 0x0400819F RID: 33183
	public float Force = 10f;

	// Token: 0x040081A0 RID: 33184
	public float UpForce = 1f;

	// Token: 0x040081A1 RID: 33185
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040081A2 RID: 33186
	public List<AudioClip> Sounds = new List<AudioClip>();

	// Token: 0x040081A3 RID: 33187
	public float SoundMinPitch = 0.8f;

	// Token: 0x040081A4 RID: 33188
	public float SoundMaxPitch = 1.2f;

	// Token: 0x040081A5 RID: 33189
	public float LifeTime = 5f;

	// Token: 0x040081A6 RID: 33190
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Destroy;

	// Token: 0x040081A7 RID: 33191
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider[] m_Colliders;

	// Token: 0x040081A8 RID: 33192
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Collider, Dictionary<string, object>> m_PiecesInitial = new Dictionary<Collider, Dictionary<string, object>>();
}
