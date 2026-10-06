using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A66 RID: 6758
	[Preserve]
	public class BloodmoonConfiguration : AbstractConfiguration, IConfiguration<LayerState>, IConfiguration
	{
		// Token: 0x1700198A RID: 6538
		// (get) Token: 0x0600CD1A RID: 52506 RVA: 0x004ABDE6 File Offset: 0x004A9FE6
		// (set) Token: 0x0600CD1B RID: 52507 RVA: 0x004ABDEE File Offset: 0x004A9FEE
		public Dictionary<LayerType, LayerState> Layers
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CD1C RID: 52508 RVA: 0x004ABDF7 File Offset: 0x004A9FF7
		public BloodmoonConfiguration()
		{
			this.Layers = new Dictionary<LayerType, LayerState>();
		}

		// Token: 0x0600CD1D RID: 52509 RVA: 0x004ABE0A File Offset: 0x004AA00A
		public override int CountFor(LayerType _layer)
		{
			if (!this.Layers.ContainsKey(_layer))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600CD1E RID: 52510 RVA: 0x004ABE20 File Offset: 0x004AA020
		public override void ParseFromXml(XElement _xmlNode)
		{
			base.ParseFromXml(_xmlNode);
			foreach (XElement xelement in _xmlNode.Elements("layer"))
			{
				LayerType layerType = EnumUtils.Parse<LayerType>(xelement.GetAttribute("key"), false);
				float lo = float.Parse(xelement.GetAttribute("lo"));
				float hi = float.Parse(xelement.GetAttribute("hi"));
				this.Layers.Add(layerType, new LayerState((float tl) => BloodmoonConfiguration.getState(tl, lo, hi)));
			}
		}

		// Token: 0x0600CD1F RID: 52511 RVA: 0x004ABEE8 File Offset: 0x004AA0E8
		[PublicizedFrom(EAccessModifier.Private)]
		public static LayerStateType getState(float _threatLevel, float _enabledThreshold, float _hiThreshold)
		{
			if (_threatLevel < _enabledThreshold)
			{
				return LayerStateType.disabled;
			}
			if (_threatLevel >= _hiThreshold)
			{
				return LayerStateType.hi;
			}
			return LayerStateType.lo;
		}
	}
}
