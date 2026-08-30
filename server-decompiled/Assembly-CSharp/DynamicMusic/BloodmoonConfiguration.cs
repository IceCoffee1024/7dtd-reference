using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A13 RID: 6675
	[Preserve]
	public class BloodmoonConfiguration : AbstractConfiguration, IConfiguration<LayerState>, IConfiguration
	{
		// Token: 0x17001876 RID: 6262
		// (get) Token: 0x0600CA39 RID: 51769 RVA: 0x004A1526 File Offset: 0x0049F726
		// (set) Token: 0x0600CA3A RID: 51770 RVA: 0x004A152E File Offset: 0x0049F72E
		public Dictionary<LayerType, LayerState> Layers
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CA3B RID: 51771 RVA: 0x004A1537 File Offset: 0x0049F737
		public BloodmoonConfiguration()
		{
			this.Layers = new Dictionary<LayerType, LayerState>();
		}

		// Token: 0x0600CA3C RID: 51772 RVA: 0x004A154A File Offset: 0x0049F74A
		public override int CountFor(LayerType _layer)
		{
			if (!this.Layers.ContainsKey(_layer))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600CA3D RID: 51773 RVA: 0x004A1560 File Offset: 0x0049F760
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

		// Token: 0x0600CA3E RID: 51774 RVA: 0x004A1628 File Offset: 0x0049F828
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
