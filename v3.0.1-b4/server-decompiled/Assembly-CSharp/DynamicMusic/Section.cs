using System;
using System.Collections;
using System.Text;
using MusicUtils.Enums;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A07 RID: 6663
	[Preserve]
	public abstract class Section : ContentPlayer, ISection, IPlayable, IFadeable, ICleanable
	{
		// Token: 0x17001863 RID: 6243
		// (get) Token: 0x0600C9DB RID: 51675 RVA: 0x004A0096 File Offset: 0x0049E296
		// (set) Token: 0x0600C9DC RID: 51676 RVA: 0x004A009E File Offset: 0x0049E29E
		public SectionType Sect { get; set; }

		// Token: 0x17001864 RID: 6244
		// (get) Token: 0x0600C9DD RID: 51677 RVA: 0x004A00A7 File Offset: 0x0049E2A7
		// (set) Token: 0x0600C9DE RID: 51678 RVA: 0x004A00AF File Offset: 0x0049E2AF
		public bool IsInitialized
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001865 RID: 6245
		// (get) Token: 0x0600C9E0 RID: 51680 RVA: 0x004A00D3 File Offset: 0x0049E2D3
		// (set) Token: 0x0600C9DF RID: 51679 RVA: 0x004A00B8 File Offset: 0x0049E2B8
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

		// Token: 0x0600C9E1 RID: 51681 RVA: 0x004A00F3 File Offset: 0x0049E2F3
		public override void Init()
		{
			this.coroutines = new EnumDictionary<MusicActionType, Coroutine>();
		}

		// Token: 0x0600C9E2 RID: 51682 RVA: 0x004A0100 File Offset: 0x0049E300
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

		// Token: 0x0600C9E3 RID: 51683 RVA: 0x004A01EC File Offset: 0x0049E3EC
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

		// Token: 0x0600C9E4 RID: 51684 RVA: 0x004A021F File Offset: 0x0049E41F
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

		// Token: 0x0600C9E5 RID: 51685 RVA: 0x004A0252 File Offset: 0x0049E452
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

		// Token: 0x0600C9E6 RID: 51686 RVA: 0x004A0288 File Offset: 0x0049E488
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

		// Token: 0x0600C9E7 RID: 51687 RVA: 0x004A0340 File Offset: 0x0049E540
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

		// Token: 0x0600C9E8 RID: 51688 RVA: 0x004A03D3 File Offset: 0x0049E5D3
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

		// Token: 0x0600C9E9 RID: 51689 RVA: 0x004A03E2 File Offset: 0x0049E5E2
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

		// Token: 0x0600C9EA RID: 51690
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract IEnumerator PlayCoroutine();

		// Token: 0x0600C9EB RID: 51691 RVA: 0x004A03F1 File Offset: 0x0049E5F1
		public virtual IEnumerator PreloadRoutine()
		{
			yield return null;
			yield break;
		}

		// Token: 0x0600C9EC RID: 51692 RVA: 0x004A03FC File Offset: 0x0049E5FC
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

		// Token: 0x0600C9ED RID: 51693 RVA: 0x004A04DC File Offset: 0x0049E6DC
		[PublicizedFrom(EAccessModifier.Protected)]
		public Section()
		{
		}

		// Token: 0x04009984 RID: 39300
		[PublicizedFrom(EAccessModifier.Protected)]
		public static GameObject parent = new GameObject("Music");

		// Token: 0x04009985 RID: 39301
		[PublicizedFrom(EAccessModifier.Protected)]
		public static GameRandom rng = GameRandomManager.Instance.CreateGameRandom();

		// Token: 0x04009987 RID: 39303
		[PublicizedFrom(EAccessModifier.Protected)]
		public AudioSource src;

		// Token: 0x04009988 RID: 39304
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumDictionary<MusicActionType, Coroutine> coroutines;

		// Token: 0x04009989 RID: 39305
		[PublicizedFrom(EAccessModifier.Protected)]
		public Coroutine LoadRoutine;

		// Token: 0x0400998B RID: 39307
		[PublicizedFrom(EAccessModifier.Private)]
		public const float fadeTime = 3f;
	}
}
