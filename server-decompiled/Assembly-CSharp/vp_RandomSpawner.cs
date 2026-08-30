using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015D1 RID: 5585
[RequireComponent(typeof(AudioSource))]
public class vp_RandomSpawner : MonoBehaviour
{
	// Token: 0x0600AE81 RID: 44673 RVA: 0x004105E8 File Offset: 0x0040E7E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		if (this.SpawnObjects == null)
		{
			return;
		}
		int num = global::UnityEngine.Random.Range(0, this.SpawnObjects.Count);
		if (this.SpawnObjects[num] == null)
		{
			return;
		}
		((GameObject)vp_Utility.Instantiate(this.SpawnObjects[num], base.transform.position, base.transform.rotation)).transform.Rotate(global::UnityEngine.Random.rotation.eulerAngles);
		this.m_Audio = base.GetComponent<AudioSource>();
		this.m_Audio.playOnAwake = true;
		if (this.Sound != null)
		{
			this.m_Audio.rolloffMode = AudioRolloffMode.Linear;
			this.m_Audio.clip = this.Sound;
			this.m_Audio.pitch = global::UnityEngine.Random.Range(this.SoundMinPitch, this.SoundMaxPitch) * Time.timeScale;
			this.m_Audio.Play();
		}
	}

	// Token: 0x04008214 RID: 33300
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04008215 RID: 33301
	public AudioClip Sound;

	// Token: 0x04008216 RID: 33302
	public float SoundMinPitch = 0.8f;

	// Token: 0x04008217 RID: 33303
	public float SoundMaxPitch = 1.2f;

	// Token: 0x04008218 RID: 33304
	public bool RandomAngle = true;

	// Token: 0x04008219 RID: 33305
	public List<GameObject> SpawnObjects;
}
