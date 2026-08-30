using System;
using System.Collections;
using System.Collections.Generic;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A4D RID: 6733
	[Preserve]
	public class BloodmoonLayerMixer : LayerMixer<BloodmoonConfiguration>
	{
		// Token: 0x0600CC20 RID: 52256 RVA: 0x004A8824 File Offset: 0x004A6A24
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

		// Token: 0x170018F6 RID: 6390
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

		// Token: 0x0600CC22 RID: 52258 RVA: 0x004A8A18 File Offset: 0x004A6C18
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

		// Token: 0x04009B19 RID: 39705
		public static float ThreatLevel = 0.75f;

		// Token: 0x04009B1A RID: 39706
		[PublicizedFrom(EAccessModifier.Private)]
		public static EntityPlayerLocal player;

		// Token: 0x04009B1B RID: 39707
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cIncrement = 3.7792895E-06f;

		// Token: 0x04009B1C RID: 39708
		[PublicizedFrom(EAccessModifier.Private)]
		public EnumDictionary<LayerType, LayerParams> paramsFor;
	}
}
