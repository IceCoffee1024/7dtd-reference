using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015B6 RID: 5558
[RequireComponent(typeof(AudioSource))]
public class vp_RandomSpawner : MonoBehaviour
{
	// Token: 0x0600AD84 RID: 44420 RVA: 0x0040B2A8 File Offset: 0x004094A8
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

	// Token: 0x0400813B RID: 33083
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x0400813C RID: 33084
	public AudioClip Sound;

	// Token: 0x0400813D RID: 33085
	public float SoundMinPitch = 0.8f;

	// Token: 0x0400813E RID: 33086
	public float SoundMaxPitch = 1.2f;

	// Token: 0x0400813F RID: 33087
	public bool RandomAngle = true;

	// Token: 0x04008140 RID: 33088
	public List<GameObject> SpawnObjects;
}
