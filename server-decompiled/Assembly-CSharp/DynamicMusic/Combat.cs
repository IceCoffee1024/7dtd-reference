using System;
using System.Collections;
using MusicUtils.Enums;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A5C RID: 6748
	[Preserve]
	public class Combat : LayeredSection<CombatLayerMixer>
	{
		// Token: 0x0600CC7F RID: 52351 RVA: 0x004A9F2C File Offset: 0x004A812C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override IEnumerator PlayCoroutine()
		{
			yield return this.<>n__0();
			yield return new WaitUntil(() => !this.src.isPlaying && !this.IsPaused);
			this.Reset();
			this.Mixer.Unload();
			this.coroutines.Remove(MusicActionType.Play);
			yield break;
		}
	}
}
