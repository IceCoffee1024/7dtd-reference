using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A3D RID: 6717
	[Preserve]
	public class Configuration : AbstractConfiguration, IFiniteConfiguration, IConfiguration<IList<PlacementType>>, IConfiguration
	{
		// Token: 0x170018EA RID: 6378
		// (get) Token: 0x0600CBCC RID: 52172 RVA: 0x004A7A4E File Offset: 0x004A5C4E
		// (set) Token: 0x0600CBCD RID: 52173 RVA: 0x004A7A56 File Offset: 0x004A5C56
		public Dictionary<LayerType, IList<PlacementType>> Layers
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CBCE RID: 52174 RVA: 0x004A7A5F File Offset: 0x004A5C5F
		public Configuration()
		{
			this.Layers = new Dictionary<LayerType, IList<PlacementType>>();
		}

		// Token: 0x0600CBCF RID: 52175 RVA: 0x004A7A72 File Offset: 0x004A5C72
		public override int CountFor(LayerType _layer)
		{
			if (!this.Layers.ContainsKey(_layer))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600CBD0 RID: 52176 RVA: 0x004A7A88 File Offset: 0x004A5C88
		public override void ParseFromXml(XElement _xmlNode)
		{
			base.ParseFromXml(_xmlNode);
			foreach (XElement xelement in _xmlNode.Elements("layer"))
			{
				this.ParseLayers(xelement);
			}
		}

		// Token: 0x0600CBD1 RID: 52177 RVA: 0x004A7AE8 File Offset: 0x004A5CE8
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
