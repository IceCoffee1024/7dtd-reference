using System;
using System.Collections;
using System.Collections.Generic;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A79 RID: 6777
	[Preserve]
	public class BloodmoonLayerMixer : LayerMixer<BloodmoonConfiguration>
	{
		// Token: 0x0600CD78 RID: 52600 RVA: 0x004ACCF0 File Offset: 0x004AAEF0
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

		// Token: 0x17001998 RID: 6552
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

		// Token: 0x0600CD7A RID: 52602 RVA: 0x004ACEE4 File Offset: 0x004AB0E4
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

		// Token: 0x04009BDF RID: 39903
		public static float ThreatLevel = 0.75f;

		// Token: 0x04009BE0 RID: 39904
		[PublicizedFrom(EAccessModifier.Private)]
		public static EntityPlayerLocal player;

		// Token: 0x04009BE1 RID: 39905
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cIncrement = 3.7792895E-06f;

		// Token: 0x04009BE2 RID: 39906
		[PublicizedFrom(EAccessModifier.Private)]
		public EnumDictionary<LayerType, LayerParams> paramsFor;
	}
}
