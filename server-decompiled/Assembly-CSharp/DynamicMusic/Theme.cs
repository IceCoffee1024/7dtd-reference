using System;
using System.Collections;
using MusicUtils.Enums;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A66 RID: 6758
	[Preserve]
	public class Theme : SingleClipPlayer, ISection, IPlayable, IFadeable, ICleanable
	{
		// Token: 0x0600CCB7 RID: 52407 RVA: 0x004AA5DC File Offset: 0x004A87DC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IEnumerator InitializationCoroutine()
		{
			yield return this.<>n__0();
			if (this.src != null)
			{
				this.src.loop = true;
			}
			this.IsReady = true;
			yield break;
		}

		// Token: 0x0600CCB8 RID: 52408 RVA: 0x004AA5EB File Offset: 0x004A87EB
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IEnumerator PlayCoroutine()
		{
			yield return new WaitUntil(() => this.IsReady);
			AudioSource src = this.src;
			if (src != null)
			{
				src.Play();
			}
			this.coroutines.Remove(MusicActionType.Play);
			yield break;
		}
	}
}
