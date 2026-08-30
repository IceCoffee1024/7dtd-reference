using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015A3 RID: 5539
[RequireComponent(typeof(AudioSource))]
public class vp_Debris : MonoBehaviour
{
	// Token: 0x0600AD57 RID: 44375 RVA: 0x0040A060 File Offset: 0x00408260
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

	// Token: 0x0600AD58 RID: 44376 RVA: 0x0040A0F4 File Offset: 0x004082F4
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

	// Token: 0x0600AD59 RID: 44377 RVA: 0x0040A2A5 File Offset: 0x004084A5
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.m_Destroy && !base.GetComponent<AudioSource>().isPlaying)
		{
			vp_Utility.Destroy(base.gameObject);
		}
	}

	// Token: 0x040080DA RID: 32986
	public float Radius = 2f;

	// Token: 0x040080DB RID: 32987
	public float Force = 10f;

	// Token: 0x040080DC RID: 32988
	public float UpForce = 1f;

	// Token: 0x040080DD RID: 32989
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040080DE RID: 32990
	public List<AudioClip> Sounds = new List<AudioClip>();

	// Token: 0x040080DF RID: 32991
	public float SoundMinPitch = 0.8f;

	// Token: 0x040080E0 RID: 32992
	public float SoundMaxPitch = 1.2f;

	// Token: 0x040080E1 RID: 32993
	public float LifeTime = 5f;

	// Token: 0x040080E2 RID: 32994
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Destroy;

	// Token: 0x040080E3 RID: 32995
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider[] m_Colliders;

	// Token: 0x040080E4 RID: 32996
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Collider, Dictionary<string, object>> m_PiecesInitial = new Dictionary<Collider, Dictionary<string, object>>();
}
