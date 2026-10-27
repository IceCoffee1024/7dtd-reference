using System;
using System.Collections;
using System.Text;
using MusicUtils.Enums;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A8B RID: 6795
	[Preserve]
	public abstract class Section : ContentPlayer, ISection, IPlayable, IFadeable, ICleanable
	{
		// Token: 0x170019BF RID: 6591
		// (get) Token: 0x0600CDD0 RID: 52688 RVA: 0x004AE452 File Offset: 0x004AC652
		// (set) Token: 0x0600CDD1 RID: 52689 RVA: 0x004AE45A File Offset: 0x004AC65A
		public SectionType Sect { get; set; }

		// Token: 0x170019C0 RID: 6592
		// (get) Token: 0x0600CDD2 RID: 52690 RVA: 0x004AE463 File Offset: 0x004AC663
		// (set) Token: 0x0600CDD3 RID: 52691 RVA: 0x004AE46B File Offset: 0x004AC66B
		public bool IsInitialized
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019C1 RID: 6593
		// (get) Token: 0x0600CDD5 RID: 52693 RVA: 0x004AE48F File Offset: 0x004AC68F
		// (set) Token: 0x0600CDD4 RID: 52692 RVA: 0x004AE474 File Offset: 0x004AC674
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

		// Token: 0x0600CDD6 RID: 52694 RVA: 0x004AE4AF File Offset: 0x004AC6AF
		public override void Init()
		{
			this.coroutines = new EnumDictionary<MusicActionType, Coroutine>();
		}

		// Token: 0x0600CDD7 RID: 52695 RVA: 0x004AE4BC File Offset: 0x004AC6BC
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

		// Token: 0x0600CDD8 RID: 52696 RVA: 0x004AE5A8 File Offset: 0x004AC7A8
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

		// Token: 0x0600CDD9 RID: 52697 RVA: 0x004AE5DB File Offset: 0x004AC7DB
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

		// Token: 0x0600CDDA RID: 52698 RVA: 0x004AE60E File Offset: 0x004AC80E
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

		// Token: 0x0600CDDB RID: 52699 RVA: 0x004AE644 File Offset: 0x004AC844
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

		// Token: 0x0600CDDC RID: 52700 RVA: 0x004AE6FC File Offset: 0x004AC8FC
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

		// Token: 0x0600CDDD RID: 52701 RVA: 0x004AE78F File Offset: 0x004AC98F
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

		// Token: 0x0600CDDE RID: 52702 RVA: 0x004AE79E File Offset: 0x004AC99E
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

		// Token: 0x0600CDDF RID: 52703
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract IEnumerator PlayCoroutine();

		// Token: 0x0600CDE0 RID: 52704 RVA: 0x004AE7AD File Offset: 0x004AC9AD
		public virtual IEnumerator PreloadRoutine()
		{
			yield return null;
			yield break;
		}

		// Token: 0x0600CDE1 RID: 52705 RVA: 0x004AE7B8 File Offset: 0x004AC9B8
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

		// Token: 0x0600CDE2 RID: 52706 RVA: 0x004AE898 File Offset: 0x004ACA98
		[PublicizedFrom(EAccessModifier.Protected)]
		public Section()
		{
		}

		// Token: 0x04009C64 RID: 40036
		[PublicizedFrom(EAccessModifier.Protected)]
		public static GameObject parent = new GameObject("Music");

		// Token: 0x04009C65 RID: 40037
		[PublicizedFrom(EAccessModifier.Protected)]
		public static GameRandom rng = GameRandomManager.Instance.CreateGameRandom();

		// Token: 0x04009C67 RID: 40039
		[PublicizedFrom(EAccessModifier.Protected)]
		public AudioSource src;

		// Token: 0x04009C68 RID: 40040
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumDictionary<MusicActionType, Coroutine> coroutines;

		// Token: 0x04009C69 RID: 40041
		[PublicizedFrom(EAccessModifier.Protected)]
		public Coroutine LoadRoutine;

		// Token: 0x04009C6B RID: 40043
		[PublicizedFrom(EAccessModifier.Private)]
		public const float fadeTime = 3f;
	}
}
