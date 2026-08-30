using System;
using System.Collections;
using UniLinq;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A22 RID: 6690
	public abstract class SingleClipPlayer : Section, ISection, IPlayable, IFadeable, ICleanable
	{
		// Token: 0x0600CA7C RID: 51836 RVA: 0x004A1E29 File Offset: 0x004A0029
		public override void Init()
		{
			base.Init();
			this.LoadRoutine = GameManager.Instance.StartCoroutine(this.InitializationCoroutine());
		}

		// Token: 0x0600CA7D RID: 51837 RVA: 0x004A1E47 File Offset: 0x004A0047
		[PublicizedFrom(EAccessModifier.Private)]
		public SingleClip GetSingleClip()
		{
			return Content.AllContent.OfType<SingleClip>().First<SingleClip>((SingleClip c) => c.Section == base.Sect);
		}

		// Token: 0x0600CA7E RID: 51838 RVA: 0x004A1E64 File Offset: 0x004A0064
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

		// Token: 0x0600CA7F RID: 51839 RVA: 0x004A1E73 File Offset: 0x004A0073
		[PublicizedFrom(EAccessModifier.Protected)]
		public SingleClipPlayer()
		{
		}

		// Token: 0x040099D6 RID: 39382
		[PublicizedFrom(EAccessModifier.Protected)]
		public SingleClip clip;
	}
}
