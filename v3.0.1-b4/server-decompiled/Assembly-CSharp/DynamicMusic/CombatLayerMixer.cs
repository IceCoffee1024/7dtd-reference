using System;
using System.Collections;
using System.Collections.Generic;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A37 RID: 6711
	[Preserve]
	public class CombatLayerMixer : FixedLayerMixer
	{
		// Token: 0x0600CB00 RID: 51968 RVA: 0x004A3C08 File Offset: 0x004A1E08
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void updateHyperbar(int _idx)
		{
			this.hyperbar = _idx / (Content.SamplesFor[base.Sect] * 2) % this.maxHyperbar;
		}

		// Token: 0x0600CB01 RID: 51969 RVA: 0x004A3C2B File Offset: 0x004A1E2B
		public override IEnumerator Load()
		{
			yield return this.<>n__0();
			this.maxHyperbar = this.config.Layers.Values.First<FixedConfigurationLayerData>().LayerInstances.First<List<PlacementType>>().Count;
			yield break;
		}

		// Token: 0x04009A1C RID: 39452
		[PublicizedFrom(EAccessModifier.Private)]
		public int maxHyperbar;
	}
}
