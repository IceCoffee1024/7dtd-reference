using System;
using System.Collections;
using MusicUtils.Enums;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A35 RID: 6709
	[Preserve]
	public class Combat : LayeredSection<CombatLayerMixer>
	{
		// Token: 0x0600CAF6 RID: 51958 RVA: 0x004A3B38 File Offset: 0x004A1D38
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
