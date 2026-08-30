using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001588 RID: 5512
[RequireComponent(typeof(AudioSource))]
public class vp_Debris : MonoBehaviour
{
	// Token: 0x0600AC5A RID: 44122 RVA: 0x00404D20 File Offset: 0x00402F20
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

	// Token: 0x0600AC5B RID: 44123 RVA: 0x00404DB4 File Offset: 0x00402FB4
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

	// Token: 0x0600AC5C RID: 44124 RVA: 0x00404F65 File Offset: 0x00403165
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.m_Destroy && !base.GetComponent<AudioSource>().isPlaying)
		{
			vp_Utility.Destroy(base.gameObject);
		}
	}

	// Token: 0x04008001 RID: 32769
	public float Radius = 2f;

	// Token: 0x04008002 RID: 32770
	public float Force = 10f;

	// Token: 0x04008003 RID: 32771
	public float UpForce = 1f;

	// Token: 0x04008004 RID: 32772
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04008005 RID: 32773
	public List<AudioClip> Sounds = new List<AudioClip>();

	// Token: 0x04008006 RID: 32774
	public float SoundMinPitch = 0.8f;

	// Token: 0x04008007 RID: 32775
	public float SoundMaxPitch = 1.2f;

	// Token: 0x04008008 RID: 32776
	public float LifeTime = 5f;

	// Token: 0x04008009 RID: 32777
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Destroy;

	// Token: 0x0400800A RID: 32778
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider[] m_Colliders;

	// Token: 0x0400800B RID: 32779
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Collider, Dictionary<string, object>> m_PiecesInitial = new Dictionary<Collider, Dictionary<string, object>>();
}
