using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A3E RID: 6718
	[Preserve]
	public class FixedConfiguration : AbstractConfiguration, IConfiguration<FixedConfigurationLayerData>, IConfiguration
	{
		// Token: 0x170018EB RID: 6379
		// (get) Token: 0x0600CBD2 RID: 52178 RVA: 0x004A7B58 File Offset: 0x004A5D58
		// (set) Token: 0x0600CBD3 RID: 52179 RVA: 0x004A7B60 File Offset: 0x004A5D60
		public Dictionary<LayerType, FixedConfigurationLayerData> Layers
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CBD4 RID: 52180 RVA: 0x004A7B69 File Offset: 0x004A5D69
		public FixedConfiguration()
		{
			this.Layers = new Dictionary<LayerType, FixedConfigurationLayerData>();
		}

		// Token: 0x0600CBD5 RID: 52181 RVA: 0x004A7B7C File Offset: 0x004A5D7C
		public override int CountFor(LayerType _layer)
		{
			FixedConfigurationLayerData fixedConfigurationLayerData;
			if (this.Layers.TryGetValue(_layer, out fixedConfigurationLayerData))
			{
				return fixedConfigurationLayerData.Count;
			}
			return 0;
		}

		// Token: 0x0600CBD6 RID: 52182 RVA: 0x004A7BA4 File Offset: 0x004A5DA4
		public override void ParseFromXml(XElement _xmlNode)
		{
			base.ParseFromXml(_xmlNode);
			foreach (XElement xelement in _xmlNode.Elements("layer"))
			{
				this.ParseLayers(xelement);
			}
		}

		// Token: 0x0600CBD7 RID: 52183 RVA: 0x004A7C04 File Offset: 0x004A5E04
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
