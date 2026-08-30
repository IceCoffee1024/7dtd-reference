using System;
using System.Collections;
using MusicUtils.Enums;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A3F RID: 6719
	[Preserve]
	public class Theme : SingleClipPlayer, ISection, IPlayable, IFadeable, ICleanable
	{
		// Token: 0x0600CB2E RID: 52014 RVA: 0x004A41E8 File Offset: 0x004A23E8
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

		// Token: 0x0600CB2F RID: 52015 RVA: 0x004A41F7 File Offset: 0x004A23F7
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
