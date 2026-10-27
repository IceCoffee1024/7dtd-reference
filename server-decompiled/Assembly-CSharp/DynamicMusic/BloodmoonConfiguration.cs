using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A97 RID: 6807
	[Preserve]
	public class BloodmoonConfiguration : AbstractConfiguration, IConfiguration<LayerState>, IConfiguration
	{
		// Token: 0x170019D2 RID: 6610
		// (get) Token: 0x0600CE2E RID: 52782 RVA: 0x004AF8E2 File Offset: 0x004ADAE2
		// (set) Token: 0x0600CE2F RID: 52783 RVA: 0x004AF8EA File Offset: 0x004ADAEA
		public Dictionary<LayerType, LayerState> Layers
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CE30 RID: 52784 RVA: 0x004AF8F3 File Offset: 0x004ADAF3
		public BloodmoonConfiguration()
		{
			this.Layers = new Dictionary<LayerType, LayerState>();
		}

		// Token: 0x0600CE31 RID: 52785 RVA: 0x004AF906 File Offset: 0x004ADB06
		public override int CountFor(LayerType _layer)
		{
			if (!this.Layers.ContainsKey(_layer))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600CE32 RID: 52786 RVA: 0x004AF91C File Offset: 0x004ADB1C
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

		// Token: 0x0600CE33 RID: 52787 RVA: 0x004AF9E4 File Offset: 0x004ADBE4
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
