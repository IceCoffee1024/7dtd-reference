using System;
using System.Collections;
using System.Collections.Generic;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A26 RID: 6694
	[Preserve]
	public class BloodmoonLayerMixer : LayerMixer<BloodmoonConfiguration>
	{
		// Token: 0x0600CA97 RID: 51863 RVA: 0x004A2430 File Offset: 0x004A0630
		public BloodmoonLayerMixer()
		{
			BloodmoonLayerMixer.player = GameManager.Instance.World.GetPrimaryPlayer();
			this.paramsFor = new EnumDictionary<LayerType, LayerParams>();
			Enum.GetValues(typeof(LayerType)).Cast<LayerType>().ToList<LayerType>()
				.ForEach(delegate(LayerType lyr)
				{
					this.paramsFor.Add(lyr, new LayerParams(0f, 1f));
				});
		}

		// Token: 0x17001884 RID: 6276
		public override float this[int _idx]
		{
			get
			{
				if (BloodmoonLayerMixer.player == null)
				{
					BloodmoonLayerMixer.player = GameManager.Instance.World.GetPrimaryPlayer();
				}
				float num = ((BloodmoonLayerMixer.player == null) ? 0f : BloodmoonLayerMixer.player.ThreatLevel.Numeric);
				float num2 = 0f;
				foreach (KeyValuePair<LayerType, LayerState> keyValuePair in this.config.Layers)
				{
					LayerParams layerParams = this.paramsFor[keyValuePair.Key];
					layerParams.Volume = Mathf.Clamp01(layerParams.Volume + ((keyValuePair.Value.Get(num) == LayerStateType.disabled) ? (-3.7792895E-06f) : 3.7792895E-06f));
					layerParams.Mix = Mathf.Clamp01(layerParams.Mix + ((keyValuePair.Value.Get(num) != LayerStateType.hi) ? (-3.7792895E-06f) : 3.7792895E-06f));
					foreach (LayeredContent layeredContent in this.clipSetsFor[keyValuePair.Key])
					{
						num2 += layeredContent.GetSample(PlacementType.Loop, _idx, new float[] { layerParams.Volume, layerParams.Mix });
					}
				}
				return (float)Math.Tanh((double)num2);
			}
		}

		// Token: 0x0600CA99 RID: 51865 RVA: 0x004A2624 File Offset: 0x004A0824
		public override IEnumerator Load()
		{
			yield return this.<>n__0();
			foreach (LayerParams layerParams in this.paramsFor.Values)
			{
				layerParams.Mix = 1f;
				layerParams.Volume = 0f;
			}
			foreach (LayerType layer in this.config.Layers.Keys)
			{
				LayeredContent content = LayeredContent.Get<BloodmoonClipSet>(SectionType.Bloodmoon, layer);
				yield return content.Load();
				this.clipSetsFor.Add(layer, new List<LayeredContent> { content });
				content = null;
			}
			Dictionary<LayerType, LayerState>.KeyCollection.Enumerator enumerator2 = default(Dictionary<LayerType, LayerState>.KeyCollection.Enumerator);
			BloodmoonLayerMixer.player = GameManager.Instance.World.GetPrimaryPlayer();
			yield break;
			yield break;
		}

		// Token: 0x040099E8 RID: 39400
		public static float ThreatLevel = 0.75f;

		// Token: 0x040099E9 RID: 39401
		[PublicizedFrom(EAccessModifier.Private)]
		public static EntityPlayerLocal player;

		// Token: 0x040099EA RID: 39402
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cIncrement = 3.7792895E-06f;

		// Token: 0x040099EB RID: 39403
		[PublicizedFrom(EAccessModifier.Private)]
		public EnumDictionary<LayerType, LayerParams> paramsFor;
	}
}
