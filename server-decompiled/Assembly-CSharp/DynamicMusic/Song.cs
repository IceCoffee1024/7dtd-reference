using System;
using System.Collections;
using MusicUtils.Enums;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A90 RID: 6800
	[Preserve]
	public class Song : SingleClipPlayer, ISection, IPlayable, IFadeable, ICleanable
	{
		// Token: 0x0600CE05 RID: 52741 RVA: 0x004AE9B8 File Offset: 0x004ACBB8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IEnumerator PlayCoroutine()
		{
			yield return new WaitUntil(() => this.IsReady);
			AudioSource src = this.src;
			if (src != null)
			{
				src.Play();
			}
			yield return new WaitUntil(() => !this.src.isPlaying && !this.IsPaused);
			if (this.IsPlaying)
			{
				this.Stop();
			}
			this.IsDone = true;
			this.coroutines.Remove(MusicActionType.Play);
			yield break;
		}
	}
}
