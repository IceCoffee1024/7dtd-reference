using System;
using System.Collections;
using UniLinq;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001A75 RID: 6773
	public abstract class SingleClipPlayer : Section, ISection, IPlayable, IFadeable, ICleanable
	{
		// Token: 0x0600CD5D RID: 52573 RVA: 0x004AC6E9 File Offset: 0x004AA8E9
		public override void Init()
		{
			base.Init();
			this.LoadRoutine = GameManager.Instance.StartCoroutine(this.InitializationCoroutine());
		}

		// Token: 0x0600CD5E RID: 52574 RVA: 0x004AC707 File Offset: 0x004AA907
		[PublicizedFrom(EAccessModifier.Private)]
		public SingleClip GetSingleClip()
		{
			return Content.AllContent.OfType<SingleClip>().First<SingleClip>((SingleClip c) => c.Section == base.Sect);
		}

		// Token: 0x0600CD5F RID: 52575 RVA: 0x004AC724 File Offset: 0x004AA924
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

		// Token: 0x0600CD60 RID: 52576 RVA: 0x004AC733 File Offset: 0x004AA933
		[PublicizedFrom(EAccessModifier.Protected)]
		public SingleClipPlayer()
		{
		}

		// Token: 0x04009BCD RID: 39885
		[PublicizedFrom(EAccessModifier.Protected)]
		public SingleClip clip;
	}
}
