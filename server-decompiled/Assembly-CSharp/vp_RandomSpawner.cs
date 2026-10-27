using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001605 RID: 5637
[RequireComponent(typeof(AudioSource))]
public class vp_RandomSpawner : MonoBehaviour
{
	// Token: 0x0600AF8C RID: 44940 RVA: 0x00417840 File Offset: 0x00415A40
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

	// Token: 0x040082D8 RID: 33496
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040082D9 RID: 33497
	public AudioClip Sound;

	// Token: 0x040082DA RID: 33498
	public float SoundMinPitch = 0.8f;

	// Token: 0x040082DB RID: 33499
	public float SoundMaxPitch = 1.2f;

	// Token: 0x040082DC RID: 33500
	public bool RandomAngle = true;

	// Token: 0x040082DD RID: 33501
	public List<GameObject> SpawnObjects;
}
