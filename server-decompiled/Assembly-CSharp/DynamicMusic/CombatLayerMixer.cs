using System;
using System.Collections;
using System.Collections.Generic;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001ABB RID: 6843
	[Preserve]
	public class CombatLayerMixer : FixedLayerMixer
	{
		// Token: 0x0600CEF5 RID: 52981 RVA: 0x004B1FC4 File Offset: 0x004B01C4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void updateHyperbar(int _idx)
		{
			this.hyperbar = _idx / (Content.SamplesFor[base.Sect] * 2) % this.maxHyperbar;
		}

		// Token: 0x0600CEF6 RID: 52982 RVA: 0x004B1FE7 File Offset: 0x004B01E7
		public override IEnumerator Load()
		{
			yield return this.<>n__0();
			this.maxHyperbar = this.config.Layers.Values.First<FixedConfigurationLayerData>().LayerInstances.First<List<PlacementType>>().Count;
			yield break;
		}

		// Token: 0x04009CFC RID: 40188
		[PublicizedFrom(EAccessModifier.Private)]
		public int maxHyperbar;
	}
}
