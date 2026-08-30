using System;
using System.Collections;
using System.Text;
using MusicUtils.Enums;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A2E RID: 6702
	[Preserve]
	public abstract class Section : ContentPlayer, ISection, IPlayable, IFadeable, ICleanable
	{
		// Token: 0x170018D5 RID: 6357
		// (get) Token: 0x0600CB64 RID: 52068 RVA: 0x004A648A File Offset: 0x004A468A
		// (set) Token: 0x0600CB65 RID: 52069 RVA: 0x004A6492 File Offset: 0x004A4692
		public SectionType Sect { get; set; }

		// Token: 0x170018D6 RID: 6358
		// (get) Token: 0x0600CB66 RID: 52070 RVA: 0x004A649B File Offset: 0x004A469B
		// (set) Token: 0x0600CB67 RID: 52071 RVA: 0x004A64A3 File Offset: 0x004A46A3
		public bool IsInitialized
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170018D7 RID: 6359
		// (get) Token: 0x0600CB69 RID: 52073 RVA: 0x004A64C7 File Offset: 0x004A46C7
		// (set) Token: 0x0600CB68 RID: 52072 RVA: 0x004A64AC File Offset: 0x004A46AC
		public override float Volume
		{
			get
			{
				if (!this.src)
				{
					return 0f;
				}
				return this.src.volume;
			}
			set
			{
				if (this.src)
				{
					this.src.volume = value;
				}
			}
		}

		// Token: 0x0600CB6A RID: 52074 RVA: 0x004A64E7 File Offset: 0x004A46E7
		public override void Init()
		{
			this.coroutines = new EnumDictionary<MusicActionType, Coroutine>();
		}

		// Token: 0x0600CB6B RID: 52075 RVA: 0x004A64F4 File Offset: 0x004A46F4
		public override void Play()
		{
			if (!this.coroutines.ContainsKey(MusicActionType.Play))
			{
				base.Play();
				this.coroutines.Add(MusicActionType.Play, GameManager.Instance.StartCoroutine(this.PlayCoroutine()));
				Log.Out(string.Format("Played {0}", this.Sect));
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(string.Format("Attempted to play {0}, while play was running", this.Sect));
			stringBuilder.AppendLine("Currently running coroutines: ");
			foreach (MusicActionType musicActionType in this.coroutines.Keys)
			{
				stringBuilder.AppendLine(musicActionType.ToString());
			}
			Log.Warning(stringBuilder.ToString());
		}

		// Token: 0x0600CB6C RID: 52076 RVA: 0x004A65E0 File Offset: 0x004A47E0
		public override void Pause()
		{
			base.Pause();
			AudioSource audioSource = this.src;
			if (audioSource != null)
			{
				audioSource.Pause();
			}
			Log.Out(string.Format("Paused {0}", this.Sect));
		}

		// Token: 0x0600CB6D RID: 52077 RVA: 0x004A6613 File Offset: 0x004A4813
		public override void UnPause()
		{
			base.UnPause();
			AudioSource audioSource = this.src;
			if (audioSource != null)
			{
				audioSource.UnPause();
			}
			Log.Out(string.Format("Unpaused {0}", this.Sect));
		}

		// Token: 0x0600CB6E RID: 52078 RVA: 0x004A6646 File Offset: 0x004A4846
		public override void Stop()
		{
			base.Stop();
			AudioSource audioSource = this.src;
			if (audioSource != null)
			{
				audioSource.Stop();
			}
			Log.Out(string.Format("Stopped {0}", this.Sect));
		}

		// Token: 0x0600CB6F RID: 52079 RVA: 0x004A667C File Offset: 0x004A487C
		public virtual void FadeIn()
		{
			Coroutine coroutine;
			if (this.coroutines.TryGetValue(MusicActionType.FadeOut, out coroutine))
			{
				GameManager.Instance.StopCoroutine(coroutine);
				this.coroutines.Remove(MusicActionType.FadeOut);
			}
			if (this.IsPaused || this.coroutines.ContainsKey(MusicActionType.Play))
			{
				this.UnPause();
			}
			else
			{
				this.Play();
			}
			Log.Out(string.Format("Fading in {0}", this.Sect));
			Coroutine coroutine2;
			if (this.coroutines.TryGetValue(MusicActionType.FadeIn, out coroutine2))
			{
				GameManager.Instance.StopCoroutine(coroutine2);
				this.coroutines.Remove(MusicActionType.FadeIn);
			}
			this.coroutines.Add(MusicActionType.FadeIn, GameManager.Instance.StartCoroutine(this.FadeInCoroutine()));
		}

		// Token: 0x0600CB70 RID: 52080 RVA: 0x004A6734 File Offset: 0x004A4934
		public virtual void FadeOut()
		{
			Coroutine coroutine;
			if (this.coroutines.TryGetValue(MusicActionType.FadeIn, out coroutine))
			{
				GameManager.Instance.StopCoroutine(coroutine);
				this.coroutines.Remove(MusicActionType.FadeIn);
			}
			Log.Out(string.Format("Fading out {0}", this.Sect));
			Coroutine coroutine2;
			if (this.coroutines.TryGetValue(MusicActionType.FadeOut, out coroutine2))
			{
				GameManager.Instance.StopCoroutine(coroutine2);
				this.coroutines.Remove(MusicActionType.FadeOut);
			}
			this.coroutines.Add(MusicActionType.FadeOut, GameManager.Instance.StartCoroutine(this.FadeOutCoroutine()));
		}

		// Token: 0x0600CB71 RID: 52081 RVA: 0x004A67C7 File Offset: 0x004A49C7
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual IEnumerator FadeInCoroutine()
		{
			double dspTime = AudioSettings.dspTime;
			double endTime = AudioSettings.dspTime + 3.0;
			double num = endTime - AudioSettings.dspTime;
			double perc = 1.0 - num / 3.0;
			float startVol = this.Volume;
			while (perc <= 1.0)
			{
				this.Volume = Mathf.Lerp(startVol, 1f, (float)perc);
				num = endTime - AudioSettings.dspTime;
				perc = 1.0 - num / 3.0;
				yield return null;
			}
			this.Volume = 1f;
			this.coroutines.Remove(MusicActionType.FadeIn);
			Log.Out(string.Format("fadeInCo complete on {0}", this.Sect));
			yield break;
		}

		// Token: 0x0600CB72 RID: 52082 RVA: 0x004A67D6 File Offset: 0x004A49D6
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual IEnumerator FadeOutCoroutine()
		{
			double dspTime = AudioSettings.dspTime;
			double endTime = AudioSettings.dspTime + 3.0;
			double num = endTime - AudioSettings.dspTime;
			double perc = 1.0 - num / 3.0;
			float startVol = this.Volume;
			while (perc <= 1.0)
			{
				this.Volume = Mathf.Lerp(startVol, 0f, (float)perc);
				num = endTime - AudioSettings.dspTime;
				perc = 1.0 - num / 3.0;
				yield return null;
			}
			this.Volume = 0f;
			this.Pause();
			double timerStart = AudioSettings.dspTime;
			yield return new WaitUntil(() => AudioSettings.dspTime - timerStart >= 60.0 || !this.IsPaused);
			if (this.IsPaused)
			{
				this.Stop();
			}
			else
			{
				Log.Out(string.Format("{0} was resumed. FadeOut coroutine has been exited.", this.Sect));
			}
			this.coroutines.Remove(MusicActionType.FadeOut);
			Log.Out(string.Format("fadeOutCo complete on {0}", this.Sect));
			yield break;
		}

		// Token: 0x0600CB73 RID: 52083
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract IEnumerator PlayCoroutine();

		// Token: 0x0600CB74 RID: 52084 RVA: 0x004A67E5 File Offset: 0x004A49E5
		public virtual IEnumerator PreloadRoutine()
		{
			yield return null;
			yield break;
		}

		// Token: 0x0600CB75 RID: 52085 RVA: 0x004A67F0 File Offset: 0x004A49F0
		public virtual void CleanUp()
		{
			AudioSource audioSource = this.src;
			if (audioSource != null)
			{
				audioSource.Stop();
			}
			foreach (Coroutine coroutine in this.coroutines.Values)
			{
				GameManager.Instance.StopCoroutine(coroutine);
			}
			if (this.LoadRoutine != null)
			{
				GameManager.Instance.StopCoroutine(this.LoadRoutine);
				this.LoadRoutine = null;
			}
			this.coroutines.Clear();
			this.coroutines = null;
			if (this.src)
			{
				global::UnityEngine.Object.Destroy(this.src.gameObject);
				this.src = null;
			}
			if (Section.parent != null)
			{
				Section.parent.transform.DetachChildren();
			}
		}

		// Token: 0x0600CB76 RID: 52086 RVA: 0x004A68D0 File Offset: 0x004A4AD0
		[PublicizedFrom(EAccessModifier.Protected)]
		public Section()
		{
		}

		// Token: 0x04009AB5 RID: 39605
		[PublicizedFrom(EAccessModifier.Protected)]
		public static GameObject parent = new GameObject("Music");

		// Token: 0x04009AB6 RID: 39606
		[PublicizedFrom(EAccessModifier.Protected)]
		public static GameRandom rng = GameRandomManager.Instance.CreateGameRandom();

		// Token: 0x04009AB8 RID: 39608
		[PublicizedFrom(EAccessModifier.Protected)]
		public AudioSource src;

		// Token: 0x04009AB9 RID: 39609
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumDictionary<MusicActionType, Coroutine> coroutines;

		// Token: 0x04009ABA RID: 39610
		[PublicizedFrom(EAccessModifier.Protected)]
		public Coroutine LoadRoutine;

		// Token: 0x04009ABC RID: 39612
		[PublicizedFrom(EAccessModifier.Private)]
		public const float fadeTime = 3f;
	}
}
