using System;
using System.Collections;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A8C RID: 6796
	[Preserve]
	public abstract class LayeredSection<T> : Section, ISection, IPlayable, IFadeable, ICleanable where T : ILayerMixer, new()
	{
		// Token: 0x0600CDEB RID: 52715 RVA: 0x004AE596 File Offset: 0x004AC796
		public LayeredSection()
		{
			this.Mixer = new T();
		}

		// Token: 0x0600CDEC RID: 52716 RVA: 0x004AE5AC File Offset: 0x004AC7AC
		public override void Init()
		{
			base.Init();
			this.Reset();
			this.IsReady = (base.IsInitialized = true);
		}

		// Token: 0x0600CDED RID: 52717 RVA: 0x004AE5D8 File Offset: 0x004AC7D8
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

		// Token: 0x0600CDEE RID: 52718 RVA: 0x004AE615 File Offset: 0x004AC815
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

		// Token: 0x0600CDEF RID: 52719 RVA: 0x004AE624 File Offset: 0x004AC824
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

		// Token: 0x0600CDF0 RID: 52720 RVA: 0x004AE633 File Offset: 0x004AC833
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

		// Token: 0x0600CDF1 RID: 52721 RVA: 0x004AE644 File Offset: 0x004AC844
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

		// Token: 0x04009C17 RID: 39959
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ProfilerMarker s_FillStreamMarker = new ProfilerMarker("DynamicMusic.LayeredSection.FillStream");

		// Token: 0x04009C18 RID: 39960
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ProfilerMarker s_LoadContentMarker = new ProfilerMarker("DynamicMusic.LayeredSection.LoadContentCoroutine");

		// Token: 0x04009C19 RID: 39961
		[PublicizedFrom(EAccessModifier.Protected)]
		public T Mixer;

		// Token: 0x04009C1A RID: 39962
		public int cursor;

		// Token: 0x04009C1B RID: 39963
		[PublicizedFrom(EAccessModifier.Private)]
		public bool preloaded;
	}
}
