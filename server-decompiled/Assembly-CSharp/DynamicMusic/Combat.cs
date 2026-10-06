using System;
using System.Collections;
using MusicUtils.Enums;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A88 RID: 6792
	[Preserve]
	public class Combat : LayeredSection<CombatLayerMixer>
	{
		// Token: 0x0600CDD7 RID: 52695 RVA: 0x004AE3F8 File Offset: 0x004AC5F8
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
