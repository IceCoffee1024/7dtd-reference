using System;
using System.Collections;
using UniLinq;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A49 RID: 6729
	public abstract class SingleClipPlayer : Section, ISection, IPlayable, IFadeable, ICleanable
	{
		// Token: 0x0600CC05 RID: 52229 RVA: 0x004A821D File Offset: 0x004A641D
		public override void Init()
		{
			base.Init();
			this.LoadRoutine = GameManager.Instance.StartCoroutine(this.InitializationCoroutine());
		}

		// Token: 0x0600CC06 RID: 52230 RVA: 0x004A823B File Offset: 0x004A643B
		[PublicizedFrom(EAccessModifier.Private)]
		public SingleClip GetSingleClip()
		{
			return Content.AllContent.OfType<SingleClip>().First<SingleClip>((SingleClip c) => c.Section == base.Sect);
		}

		// Token: 0x0600CC07 RID: 52231 RVA: 0x004A8258 File Offset: 0x004A6458
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual IEnumerator InitializationCoroutine()
		{
			this.IsReady = false;
			this.clip = this.GetSingleClip();
			if (this.clip == null)
			{
				Log.Warning("content could not be cast as an object of type 'SingleClip'");
			}
			else
			{
				yield return this.clip.Load();
				GameObject gameObject = DataLoader.LoadAsset<GameObject>(Content.SourcePathFor[base.Sect], false);
				this.src = gameObject.GetComponent<AudioSource>();
				this.src = global::UnityEngine.Object.Instantiate<AudioSource>(this.src);
				this.src.name = base.Sect.ToString();
				this.src.transform.SetParent(Section.parent.transform);
				this.src.clip = this.clip.Clip;
				this.IsReady = (base.IsInitialized = true);
			}
			this.LoadRoutine = null;
			yield break;
		}

		// Token: 0x0600CC08 RID: 52232 RVA: 0x004A8267 File Offset: 0x004A6467
		[PublicizedFrom(EAccessModifier.Protected)]
		public SingleClipPlayer()
		{
		}

		// Token: 0x04009B07 RID: 39687
		[PublicizedFrom(EAccessModifier.Protected)]
		public SingleClip clip;
	}
}
