using System;
using System.Collections;
using MusicUtils.Enums;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001AC3 RID: 6851
	[Preserve]
	public class Theme : SingleClipPlayer, ISection, IPlayable, IFadeable, ICleanable
	{
		// Token: 0x0600CF23 RID: 53027 RVA: 0x004B25A4 File Offset: 0x004B07A4
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

		// Token: 0x0600CF24 RID: 53028 RVA: 0x004B25B3 File Offset: 0x004B07B3
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
