using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A9A RID: 6810
	[Preserve]
	public class Configuration : AbstractConfiguration, IFiniteConfiguration, IConfiguration<IList<PlacementType>>, IConfiguration
	{
		// Token: 0x170019D4 RID: 6612
		// (get) Token: 0x0600CE38 RID: 52792 RVA: 0x004AFA16 File Offset: 0x004ADC16
		// (set) Token: 0x0600CE39 RID: 52793 RVA: 0x004AFA1E File Offset: 0x004ADC1E
		public Dictionary<LayerType, IList<PlacementType>> Layers
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CE3A RID: 52794 RVA: 0x004AFA27 File Offset: 0x004ADC27
		public Configuration()
		{
			this.Layers = new Dictionary<LayerType, IList<PlacementType>>();
		}

		// Token: 0x0600CE3B RID: 52795 RVA: 0x004AFA3A File Offset: 0x004ADC3A
		public override int CountFor(LayerType _layer)
		{
			if (!this.Layers.ContainsKey(_layer))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600CE3C RID: 52796 RVA: 0x004AFA50 File Offset: 0x004ADC50
		public override void ParseFromXml(XElement _xmlNode)
		{
			base.ParseFromXml(_xmlNode);
			foreach (XElement xelement in _xmlNode.Elements("layer"))
			{
				this.ParseLayers(xelement);
			}
		}

		// Token: 0x0600CE3D RID: 52797 RVA: 0x004AFAB0 File Offset: 0x004ADCB0
		[PublicizedFrom(EAccessModifier.Private)]
		public void ParseLayers(XElement e)
		{
			List<PlacementType> list = new List<PlacementType>();
			foreach (string text in e.GetAttribute("value").Split(',', StringSplitOptions.None))
			{
				list.Add((PlacementType)byte.Parse(text));
			}
			LayerType layerType = EnumUtils.Parse<LayerType>(e.GetAttribute("key"), false);
			this.Layers.Add(layerType, list);
		}
	}
}
