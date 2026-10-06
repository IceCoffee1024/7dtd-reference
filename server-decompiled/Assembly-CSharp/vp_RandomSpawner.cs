using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015F3 RID: 5619
[RequireComponent(typeof(AudioSource))]
public class vp_RandomSpawner : MonoBehaviour
{
	// Token: 0x0600AF34 RID: 44852 RVA: 0x0041543C File Offset: 0x0041363C
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

	// Token: 0x04008297 RID: 33431
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04008298 RID: 33432
	public AudioClip Sound;

	// Token: 0x04008299 RID: 33433
	public float SoundMinPitch = 0.8f;

	// Token: 0x0400829A RID: 33434
	public float SoundMaxPitch = 1.2f;

	// Token: 0x0400829B RID: 33435
	public bool RandomAngle = true;

	// Token: 0x0400829C RID: 33436
	public List<GameObject> SpawnObjects;
}
