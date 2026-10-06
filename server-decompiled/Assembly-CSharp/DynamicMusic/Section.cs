using System;
using System.Collections;
using System.Text;
using MusicUtils.Enums;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A5A RID: 6746
	[Preserve]
	public abstract class Section : ContentPlayer, ISection, IPlayable, IFadeable, ICleanable
	{
		// Token: 0x17001977 RID: 6519
		// (get) Token: 0x0600CCBC RID: 52412 RVA: 0x004AA956 File Offset: 0x004A8B56
		// (set) Token: 0x0600CCBD RID: 52413 RVA: 0x004AA95E File Offset: 0x004A8B5E
		public SectionType Sect { get; set; }

		// Token: 0x17001978 RID: 6520
		// (get) Token: 0x0600CCBE RID: 52414 RVA: 0x004AA967 File Offset: 0x004A8B67
		// (set) Token: 0x0600CCBF RID: 52415 RVA: 0x004AA96F File Offset: 0x004A8B6F
		public bool IsInitialized
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001979 RID: 6521
		// (get) Token: 0x0600CCC1 RID: 52417 RVA: 0x004AA993 File Offset: 0x004A8B93
		// (set) Token: 0x0600CCC0 RID: 52416 RVA: 0x004AA978 File Offset: 0x004A8B78
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

		// Token: 0x0600CCC2 RID: 52418 RVA: 0x004AA9B3 File Offset: 0x004A8BB3
		public override void Init()
		{
			this.coroutines = new EnumDictionary<MusicActionType, Coroutine>();
		}

		// Token: 0x0600CCC3 RID: 52419 RVA: 0x004AA9C0 File Offset: 0x004A8BC0
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

		// Token: 0x0600CCC4 RID: 52420 RVA: 0x004AAAAC File Offset: 0x004A8CAC
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

		// Token: 0x0600CCC5 RID: 52421 RVA: 0x004AAADF File Offset: 0x004A8CDF
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

		// Token: 0x0600CCC6 RID: 52422 RVA: 0x004AAB12 File Offset: 0x004A8D12
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

		// Token: 0x0600CCC7 RID: 52423 RVA: 0x004AAB48 File Offset: 0x004A8D48
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

		// Token: 0x0600CCC8 RID: 52424 RVA: 0x004AAC00 File Offset: 0x004A8E00
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

		// Token: 0x0600CCC9 RID: 52425 RVA: 0x004AAC93 File Offset: 0x004A8E93
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

		// Token: 0x0600CCCA RID: 52426 RVA: 0x004AACA2 File Offset: 0x004A8EA2
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

		// Token: 0x0600CCCB RID: 52427
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract IEnumerator PlayCoroutine();

		// Token: 0x0600CCCC RID: 52428 RVA: 0x004AACB1 File Offset: 0x004A8EB1
		public virtual IEnumerator PreloadRoutine()
		{
			yield return null;
			yield break;
		}

		// Token: 0x0600CCCD RID: 52429 RVA: 0x004AACBC File Offset: 0x004A8EBC
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

		// Token: 0x0600CCCE RID: 52430 RVA: 0x004AAD9C File Offset: 0x004A8F9C
		[PublicizedFrom(EAccessModifier.Protected)]
		public Section()
		{
		}

		// Token: 0x04009B7B RID: 39803
		[PublicizedFrom(EAccessModifier.Protected)]
		public static GameObject parent = new GameObject("Music");

		// Token: 0x04009B7C RID: 39804
		[PublicizedFrom(EAccessModifier.Protected)]
		public static GameRandom rng = GameRandomManager.Instance.CreateGameRandom();

		// Token: 0x04009B7E RID: 39806
		[PublicizedFrom(EAccessModifier.Protected)]
		public AudioSource src;

		// Token: 0x04009B7F RID: 39807
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumDictionary<MusicActionType, Coroutine> coroutines;

		// Token: 0x04009B80 RID: 39808
		[PublicizedFrom(EAccessModifier.Protected)]
		public Coroutine LoadRoutine;

		// Token: 0x04009B82 RID: 39810
		[PublicizedFrom(EAccessModifier.Private)]
		public const float fadeTime = 3f;
	}
}
