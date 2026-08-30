using System;
using System.Collections;
using UnityEngine;

namespace Audio
{
	// Token: 0x02001B17 RID: 6935
	public class PlayAndCleanup
	{
		// Token: 0x0600D025 RID: 53285 RVA: 0x004BC3F8 File Offset: 0x004BA5F8
		public PlayAndCleanup(LoopingPair _lp, string _soundGroupName)
		{
			this.lp = _lp;
			double num;
			this.lp.sgoBegin.src.PlayScheduled(num = AudioSettings.dspTime + 0.05);
			this.lp.sgoLoop.src.PlayScheduled(num + (double)this.lp.sgoBegin.src.clip.samples / 44100.0);
			Manager.AddPlayingAudioSource(this.lp.sgoBegin.src);
			Manager.AddPlayingAudioSource(this.lp.sgoLoop.src);
			if (!string.IsNullOrEmpty(_soundGroupName))
			{
				Manager.AddPlayAndCleanupAudioSource(_soundGroupName, this.lp.sgoBegin.src);
				Manager.AddPlayAndCleanupAudioSource(_soundGroupName, this.lp.sgoLoop.src);
			}
			GameManager.Instance.StartCoroutine(this.StopBeginWhenDone(this.lp.sgoBegin.src.clip.length, _soundGroupName));
		}

		// Token: 0x0600D026 RID: 53286 RVA: 0x004BC4FE File Offset: 0x004BA6FE
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator StopBeginWhenDone(float waitTime, string soundGroupName)
		{
			yield return new WaitForSeconds(waitTime + 0.1f);
			if (GameManager.Instance.IsPaused())
			{
				yield return new WaitForSeconds(0.1f);
			}
			if (this.lp.sgoBegin.src == null)
			{
				yield break;
			}
			if (this.lp.sgoBegin.src.isPlaying)
			{
				yield return new WaitForSeconds(0.1f);
			}
			if (this.lp.sgoBegin.src == null)
			{
				yield break;
			}
			Manager.RemovePlayingAudioSource(this.lp.sgoBegin.src);
			if (!string.IsNullOrEmpty(soundGroupName))
			{
				Manager.RemovePlayAndCleanupAudioSource(soundGroupName, this.lp.sgoBegin.src);
			}
			if (this.lp.sgoBegin.go)
			{
				global::UnityEngine.Object.Destroy(this.lp.sgoBegin.go);
			}
			yield break;
		}

		// Token: 0x0600D027 RID: 53287 RVA: 0x004BC51C File Offset: 0x004BA71C
		public PlayAndCleanup(GameObject _go, AudioSource _source, float _occlusion = 0f, float delay = 0f, bool isLooping = false, bool hasLoopingAnalog = false, string soundGroupName = "")
		{
			this.go = _go;
			this.src = _source;
			float num = 1f - _occlusion;
			float num2 = Utils.FastAbs(Manager.currentListenerPosition.y - this.go.transform.position.y);
			num2 = Utils.FastClamp01(num2 / 30f);
			this.src.volume *= (1f - num2) * num;
			if (num < 0.95f)
			{
				this.go.AddComponent<AudioLowPassFilter>().cutoffFrequency = Utils.FastLerp(10f, 5000f, Mathf.Pow(num, 2f));
			}
			if (delay > 0f)
			{
				this.src.PlayDelayed(delay);
			}
			else
			{
				Manager.PlaySource(this.src);
			}
			Manager.AddPlayingAudioSource(this.src);
			if (!string.IsNullOrEmpty(soundGroupName))
			{
				Manager.AddPlayAndCleanupAudioSource(soundGroupName, this.src);
			}
			if (!isLooping)
			{
				float num3 = _source.clip.length * (1f + Utils.FastClamp01(1f - _source.pitch)) + delay;
				GameManager.Instance.StartCoroutine(this.StopWhenDone(num3, soundGroupName));
			}
		}

		// Token: 0x0600D028 RID: 53288 RVA: 0x004BC648 File Offset: 0x004BA848
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator StopWhenDone(float waitTime, string soundGroupName)
		{
			yield return new WaitForSeconds(waitTime + 0.001f);
			if (GameManager.Instance.IsPaused())
			{
				yield return new WaitForSeconds(0.1f);
			}
			Manager.RemovePlayingAudioSource(this.src);
			if (!string.IsNullOrEmpty(soundGroupName))
			{
				Manager.RemovePlayAndCleanupAudioSource(soundGroupName, this.src);
			}
			if (this.go)
			{
				global::UnityEngine.Object.Destroy(this.go);
			}
			yield break;
		}

		// Token: 0x04009E90 RID: 40592
		[PublicizedFrom(EAccessModifier.Private)]
		public GameObject go;

		// Token: 0x04009E91 RID: 40593
		[PublicizedFrom(EAccessModifier.Private)]
		public AudioSource src;

		// Token: 0x04009E92 RID: 40594
		[PublicizedFrom(EAccessModifier.Private)]
		public LoopingPair lp;
	}
}
