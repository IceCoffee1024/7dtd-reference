using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A17 RID: 6679
	[Preserve]
	public class FixedConfiguration : AbstractConfiguration, IConfiguration<FixedConfigurationLayerData>, IConfiguration
	{
		// Token: 0x17001879 RID: 6265
		// (get) Token: 0x0600CA49 RID: 51785 RVA: 0x004A1764 File Offset: 0x0049F964
		// (set) Token: 0x0600CA4A RID: 51786 RVA: 0x004A176C File Offset: 0x0049F96C
		public Dictionary<LayerType, FixedConfigurationLayerData> Layers
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CA4B RID: 51787 RVA: 0x004A1775 File Offset: 0x0049F975
		public FixedConfiguration()
		{
			this.Layers = new Dictionary<LayerType, FixedConfigurationLayerData>();
		}

		// Token: 0x0600CA4C RID: 51788 RVA: 0x004A1788 File Offset: 0x0049F988
		public override int CountFor(LayerType _layer)
		{
			FixedConfigurationLayerData fixedConfigurationLayerData;
			if (this.Layers.TryGetValue(_layer, out fixedConfigurationLayerData))
			{
				return fixedConfigurationLayerData.Count;
			}
			return 0;
		}

		// Token: 0x0600CA4D RID: 51789 RVA: 0x004A17B0 File Offset: 0x0049F9B0
		public override void ParseFromXml(XElement _xmlNode)
		{
			base.ParseFromXml(_xmlNode);
			foreach (XElement xelement in _xmlNode.Elements("layer"))
			{
				this.ParseLayers(xelement);
			}
		}

		// Token: 0x0600CA4E RID: 51790 RVA: 0x004A1810 File Offset: 0x0049FA10
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
