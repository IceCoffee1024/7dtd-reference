using System;
using System.Collections;
using UniLinq;
using UnityEngine;

namespace DynamicMusic
{
	// Token: 0x02001AA6 RID: 6822
	public abstract class SingleClipPlayer : Section, ISection, IPlayable, IFadeable, ICleanable
	{
		// Token: 0x0600CE71 RID: 52849 RVA: 0x004B01E5 File Offset: 0x004AE3E5
		public override void Init()
		{
			base.Init();
			this.LoadRoutine = GameManager.Instance.StartCoroutine(this.InitializationCoroutine());
		}

		// Token: 0x0600CE72 RID: 52850 RVA: 0x004B0203 File Offset: 0x004AE403
		[PublicizedFrom(EAccessModifier.Private)]
		public SingleClip GetSingleClip()
		{
			return Content.AllContent.OfType<SingleClip>().First<SingleClip>((SingleClip c) => c.Section == base.Sect);
		}

		// Token: 0x0600CE73 RID: 52851 RVA: 0x004B0220 File Offset: 0x004AE420
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

		// Token: 0x0600CE74 RID: 52852 RVA: 0x004B022F File Offset: 0x004AE42F
		[PublicizedFrom(EAccessModifier.Protected)]
		public SingleClipPlayer()
		{
		}

		// Token: 0x04009CB6 RID: 40118
		[PublicizedFrom(EAccessModifier.Protected)]
		public SingleClip clip;
	}
}
