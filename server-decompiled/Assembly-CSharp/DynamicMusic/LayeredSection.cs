using System;
using System.Collections;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A60 RID: 6752
	[Preserve]
	public abstract class LayeredSection<T> : Section, ISection, IPlayable, IFadeable, ICleanable where T : ILayerMixer, new()
	{
		// Token: 0x0600CC93 RID: 52371 RVA: 0x004AA0CA File Offset: 0x004A82CA
		public LayeredSection()
		{
			this.Mixer = new T();
		}

		// Token: 0x0600CC94 RID: 52372 RVA: 0x004AA0E0 File Offset: 0x004A82E0
		public override void Init()
		{
			base.Init();
			this.Reset();
			this.IsReady = (base.IsInitialized = true);
		}

		// Token: 0x0600CC95 RID: 52373 RVA: 0x004AA10C File Offset: 0x004A830C
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void Reset()
		{
			this.cursor = 0;
			this.IsReady = (this.IsDone = false);
			if (this.src)
			{
				this.src.loop = true;
			}
		}

		// Token: 0x0600CC96 RID: 52374 RVA: 0x004AA149 File Offset: 0x004A8349
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IEnumerator PlayCoroutine()
		{
			yield return this.LoadContentCoroutine();
			AudioSource src = this.src;
			if (src != null)
			{
				src.Play();
			}
			yield break;
		}

		// Token: 0x0600CC97 RID: 52375 RVA: 0x004AA158 File Offset: 0x004A8358
		public override IEnumerator PreloadRoutine()
		{
			Log.Out("Preloading LayeredSection {0} : Type: {1}", new object[]
			{
				base.Sect.ToString(),
				base.GetType().ToString()
			});
			yield return this.LoadContentCoroutine();
			this.preloaded = true;
			yield break;
		}

		// Token: 0x0600CC98 RID: 52376 RVA: 0x004AA167 File Offset: 0x004A8367
		[PublicizedFrom(EAccessModifier.Protected)]
		public IEnumerator LoadContentCoroutine()
		{
			this.IsReady = false;
			this.Mixer.Sect = base.Sect;
			if (!this.preloaded)
			{
				yield return this.Mixer.Load();
			}
			else
			{
				this.preloaded = false;
				yield return null;
			}
			if (!this.src)
			{
				using (LayeredSection<T>.s_LoadContentMarker.Auto())
				{
					AudioSource component = DataLoader.LoadAsset<GameObject>(Content.SourcePathFor[base.Sect], false).GetComponent<AudioSource>();
					this.src = global::UnityEngine.Object.Instantiate<AudioSource>(component);
					this.src.transform.SetParent(Section.parent.transform);
					this.src.name = base.Sect.ToString();
					this.src.loop = true;
					this.src.priority = 0;
					this.src.clip = AudioClip.Create(base.Sect.ToString(), Content.SamplesFor[base.Sect], 2, 44100, true, new AudioClip.PCMReaderCallback(this.FillStream));
				}
			}
			this.IsReady = (base.IsInitialized = true);
			this.LoadRoutine = null;
			yield break;
		}

		// Token: 0x0600CC99 RID: 52377 RVA: 0x004AA178 File Offset: 0x004A8378
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void FillStream(float[] data)
		{
			using (LayeredSection<T>.s_FillStreamMarker.Auto())
			{
				for (int i = 0; i < data.Length; i++)
				{
					int num = i;
					int num2 = this.cursor;
					this.cursor = num2 + 1;
					data[num] = this.Mixer[num2];
				}
			}
		}

		// Token: 0x04009B51 RID: 39761
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ProfilerMarker s_FillStreamMarker = new ProfilerMarker("DynamicMusic.LayeredSection.FillStream");

		// Token: 0x04009B52 RID: 39762
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ProfilerMarker s_LoadContentMarker = new ProfilerMarker("DynamicMusic.LayeredSection.LoadContentCoroutine");

		// Token: 0x04009B53 RID: 39763
		[PublicizedFrom(EAccessModifier.Protected)]
		public T Mixer;

		// Token: 0x04009B54 RID: 39764
		public int cursor;

		// Token: 0x04009B55 RID: 39765
		[PublicizedFrom(EAccessModifier.Private)]
		public bool preloaded;
	}
}
