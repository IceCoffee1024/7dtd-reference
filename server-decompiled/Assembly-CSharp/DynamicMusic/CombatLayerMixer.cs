using System;
using System.Collections;
using System.Collections.Generic;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A5E RID: 6750
	[Preserve]
	public class CombatLayerMixer : FixedLayerMixer
	{
		// Token: 0x0600CC89 RID: 52361 RVA: 0x004A9FFC File Offset: 0x004A81FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void updateHyperbar(int _idx)
		{
			this.hyperbar = _idx / (Content.SamplesFor[base.Sect] * 2) % this.maxHyperbar;
		}

		// Token: 0x0600CC8A RID: 52362 RVA: 0x004AA01F File Offset: 0x004A821F
		public override IEnumerator Load()
		{
			yield return this.<>n__0();
			this.maxHyperbar = this.config.Layers.Values.First<FixedConfigurationLayerData>().LayerInstances.First<List<PlacementType>>().Count;
			yield break;
		}

		// Token: 0x04009B4D RID: 39757
		[PublicizedFrom(EAccessModifier.Private)]
		public int maxHyperbar;
	}
}
