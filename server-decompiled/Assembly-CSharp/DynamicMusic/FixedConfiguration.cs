using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A6A RID: 6762
	[Preserve]
	public class FixedConfiguration : AbstractConfiguration, IConfiguration<FixedConfigurationLayerData>, IConfiguration
	{
		// Token: 0x1700198D RID: 6541
		// (get) Token: 0x0600CD2A RID: 52522 RVA: 0x004AC024 File Offset: 0x004AA224
		// (set) Token: 0x0600CD2B RID: 52523 RVA: 0x004AC02C File Offset: 0x004AA22C
		public Dictionary<LayerType, FixedConfigurationLayerData> Layers
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CD2C RID: 52524 RVA: 0x004AC035 File Offset: 0x004AA235
		public FixedConfiguration()
		{
			this.Layers = new Dictionary<LayerType, FixedConfigurationLayerData>();
		}

		// Token: 0x0600CD2D RID: 52525 RVA: 0x004AC048 File Offset: 0x004AA248
		public override int CountFor(LayerType _layer)
		{
			FixedConfigurationLayerData fixedConfigurationLayerData;
			if (this.Layers.TryGetValue(_layer, out fixedConfigurationLayerData))
			{
				return fixedConfigurationLayerData.Count;
			}
			return 0;
		}

		// Token: 0x0600CD2E RID: 52526 RVA: 0x004AC070 File Offset: 0x004AA270
		public override void ParseFromXml(XElement _xmlNode)
		{
			base.ParseFromXml(_xmlNode);
			foreach (XElement xelement in _xmlNode.Elements("layer"))
			{
				this.ParseLayers(xelement);
			}
		}

		// Token: 0x0600CD2F RID: 52527 RVA: 0x004AC0D0 File Offset: 0x004AA2D0
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
