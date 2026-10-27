using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A9B RID: 6811
	[Preserve]
	public class FixedConfiguration : AbstractConfiguration, IConfiguration<FixedConfigurationLayerData>, IConfiguration
	{
		// Token: 0x170019D5 RID: 6613
		// (get) Token: 0x0600CE3E RID: 52798 RVA: 0x004AFB20 File Offset: 0x004ADD20
		// (set) Token: 0x0600CE3F RID: 52799 RVA: 0x004AFB28 File Offset: 0x004ADD28
		public Dictionary<LayerType, FixedConfigurationLayerData> Layers
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CE40 RID: 52800 RVA: 0x004AFB31 File Offset: 0x004ADD31
		public FixedConfiguration()
		{
			this.Layers = new Dictionary<LayerType, FixedConfigurationLayerData>();
		}

		// Token: 0x0600CE41 RID: 52801 RVA: 0x004AFB44 File Offset: 0x004ADD44
		public override int CountFor(LayerType _layer)
		{
			FixedConfigurationLayerData fixedConfigurationLayerData;
			if (this.Layers.TryGetValue(_layer, out fixedConfigurationLayerData))
			{
				return fixedConfigurationLayerData.Count;
			}
			return 0;
		}

		// Token: 0x0600CE42 RID: 52802 RVA: 0x004AFB6C File Offset: 0x004ADD6C
		public override void ParseFromXml(XElement _xmlNode)
		{
			base.ParseFromXml(_xmlNode);
			foreach (XElement xelement in _xmlNode.Elements("layer"))
			{
				this.ParseLayers(xelement);
			}
		}

		// Token: 0x0600CE43 RID: 52803 RVA: 0x004AFBCC File Offset: 0x004ADDCC
		[PublicizedFrom(EAccessModifier.Private)]
		public void ParseLayers(XElement e)
		{
			List<PlacementType> list = new List<PlacementType>();
			foreach (string text in e.GetAttribute("value").Split(',', StringSplitOptions.None))
			{
				list.Add((PlacementType)byte.Parse(text));
			}
			LayerType layerType = EnumUtils.Parse<LayerType>(e.GetAttribute("key"), false);
			FixedConfigurationLayerData fixedConfigurationLayerData;
			if (!this.Layers.TryGetValue(layerType, out fixedConfigurationLayerData))
			{
				this.Layers.Add(layerType, fixedConfigurationLayerData = new FixedConfigurationLayerData());
			}
			fixedConfigurationLayerData.Add(list);
		}
	}
}
