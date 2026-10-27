using System;
using System.Collections;
using MusicUtils.Enums;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001AB7 RID: 6839
	[Preserve]
	public class Bloodmoon : LayeredSection<BloodmoonLayerMixer>
	{
		// Token: 0x0600CEE0 RID: 52960 RVA: 0x004B1DB0 File Offset: 0x004AFFB0
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

		// Token: 0x0600CEE1 RID: 52961 RVA: 0x004B1DC0 File Offset: 0x004AFFC0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void FillStream(float[] data)
		{
			for (int i = 0; i < data.Length; i++)
			{
				int num = i;
				LayerMixer<BloodmoonConfiguration> mixer = this.Mixer;
				int cursor = this.cursor;
				this.cursor = cursor + 1;
				data[num] = mixer[cursor];
				this.cursor %= Content.SamplesFor[base.Sect] * 2;
			}
		}
	}
}
