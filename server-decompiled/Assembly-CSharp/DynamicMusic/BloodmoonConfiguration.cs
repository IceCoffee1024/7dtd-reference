using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A3A RID: 6714
	[Preserve]
	public class BloodmoonConfiguration : AbstractConfiguration, IConfiguration<LayerState>, IConfiguration
	{
		// Token: 0x170018E8 RID: 6376
		// (get) Token: 0x0600CBC2 RID: 52162 RVA: 0x004A791A File Offset: 0x004A5B1A
		// (set) Token: 0x0600CBC3 RID: 52163 RVA: 0x004A7922 File Offset: 0x004A5B22
		public Dictionary<LayerType, LayerState> Layers
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CBC4 RID: 52164 RVA: 0x004A792B File Offset: 0x004A5B2B
		public BloodmoonConfiguration()
		{
			this.Layers = new Dictionary<LayerType, LayerState>();
		}

		// Token: 0x0600CBC5 RID: 52165 RVA: 0x004A793E File Offset: 0x004A5B3E
		public override int CountFor(LayerType _layer)
		{
			if (!this.Layers.ContainsKey(_layer))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600CBC6 RID: 52166 RVA: 0x004A7954 File Offset: 0x004A5B54
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

		// Token: 0x0600CBC7 RID: 52167 RVA: 0x004A7A1C File Offset: 0x004A5C1C
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
