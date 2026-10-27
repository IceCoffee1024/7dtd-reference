using System;
using System.Collections;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001ABD RID: 6845
	[Preserve]
	public abstract class LayeredSection<T> : Section, ISection, IPlayable, IFadeable, ICleanable where T : ILayerMixer, new()
	{
		// Token: 0x0600CEFF RID: 52991 RVA: 0x004B2092 File Offset: 0x004B0292
		public LayeredSection()
		{
			this.Mixer = new T();
		}

		// Token: 0x0600CF00 RID: 52992 RVA: 0x004B20A8 File Offset: 0x004B02A8
		public override void Init()
		{
			base.Init();
			this.Reset();
			this.IsReady = (base.IsInitialized = true);
		}

		// Token: 0x0600CF01 RID: 52993 RVA: 0x004B20D4 File Offset: 0x004B02D4
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

		// Token: 0x0600CF02 RID: 52994 RVA: 0x004B2111 File Offset: 0x004B0311
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

		// Token: 0x0600CF03 RID: 52995 RVA: 0x004B2120 File Offset: 0x004B0320
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

		// Token: 0x0600CF04 RID: 52996 RVA: 0x004B212F File Offset: 0x004B032F
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

		// Token: 0x0600CF05 RID: 52997 RVA: 0x004B2140 File Offset: 0x004B0340
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

		// Token: 0x04009D00 RID: 40192
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ProfilerMarker s_FillStreamMarker = new ProfilerMarker("DynamicMusic.LayeredSection.FillStream");

		// Token: 0x04009D01 RID: 40193
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly ProfilerMarker s_LoadContentMarker = new ProfilerMarker("DynamicMusic.LayeredSection.LoadContentCoroutine");

		// Token: 0x04009D02 RID: 40194
		[PublicizedFrom(EAccessModifier.Protected)]
		public T Mixer;

		// Token: 0x04009D03 RID: 40195
		public int cursor;

		// Token: 0x04009D04 RID: 40196
		[PublicizedFrom(EAccessModifier.Private)]
		public bool preloaded;
	}
}
