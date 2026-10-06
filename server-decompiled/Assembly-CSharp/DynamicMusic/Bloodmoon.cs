using System;
using System.Collections;
using MusicUtils.Enums;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A86 RID: 6790
	[Preserve]
	public class Bloodmoon : LayeredSection<BloodmoonLayerMixer>
	{
		// Token: 0x0600CDCC RID: 52684 RVA: 0x004AE2B4 File Offset: 0x004AC4B4
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

		// Token: 0x0600CDCD RID: 52685 RVA: 0x004AE2C4 File Offset: 0x004AC4C4
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
