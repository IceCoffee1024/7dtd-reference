using System;
using System.Collections;
using MusicUtils.Enums;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A92 RID: 6802
	[Preserve]
	public class Theme : SingleClipPlayer, ISection, IPlayable, IFadeable, ICleanable
	{
		// Token: 0x0600CE0F RID: 52751 RVA: 0x004AEAA8 File Offset: 0x004ACCA8
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

		// Token: 0x0600CE10 RID: 52752 RVA: 0x004AEAB7 File Offset: 0x004ACCB7
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
