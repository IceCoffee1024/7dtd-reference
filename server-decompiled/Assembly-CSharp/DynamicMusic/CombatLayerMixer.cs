using System;
using System.Collections;
using System.Collections.Generic;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A8A RID: 6794
	[Preserve]
	public class CombatLayerMixer : FixedLayerMixer
	{
		// Token: 0x0600CDE1 RID: 52705 RVA: 0x004AE4C8 File Offset: 0x004AC6C8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void updateHyperbar(int _idx)
		{
			this.hyperbar = _idx / (Content.SamplesFor[base.Sect] * 2) % this.maxHyperbar;
		}

		// Token: 0x0600CDE2 RID: 52706 RVA: 0x004AE4EB File Offset: 0x004AC6EB
		public override IEnumerator Load()
		{
			yield return this.<>n__0();
			this.maxHyperbar = this.config.Layers.Values.First<FixedConfigurationLayerData>().LayerInstances.First<List<PlacementType>>().Count;
			yield break;
		}

		// Token: 0x04009C13 RID: 39955
		[PublicizedFrom(EAccessModifier.Private)]
		public int maxHyperbar;
	}
}
