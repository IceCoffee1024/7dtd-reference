using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A16 RID: 6678
	[Preserve]
	public class Configuration : AbstractConfiguration, IFiniteConfiguration, IConfiguration<IList<PlacementType>>, IConfiguration
	{
		// Token: 0x17001878 RID: 6264
		// (get) Token: 0x0600CA43 RID: 51779 RVA: 0x004A165A File Offset: 0x0049F85A
		// (set) Token: 0x0600CA44 RID: 51780 RVA: 0x004A1662 File Offset: 0x0049F862
		public Dictionary<LayerType, IList<PlacementType>> Layers
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CA45 RID: 51781 RVA: 0x004A166B File Offset: 0x0049F86B
		public Configuration()
		{
			this.Layers = new Dictionary<LayerType, IList<PlacementType>>();
		}

		// Token: 0x0600CA46 RID: 51782 RVA: 0x004A167E File Offset: 0x0049F87E
		public override int CountFor(LayerType _layer)
		{
			if (!this.Layers.ContainsKey(_layer))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600CA47 RID: 51783 RVA: 0x004A1694 File Offset: 0x0049F894
		public override void ParseFromXml(XElement _xmlNode)
		{
			base.ParseFromXml(_xmlNode);
			foreach (XElement xelement in _xmlNode.Elements("layer"))
			{
				this.ParseLayers(xelement);
			}
		}

		// Token: 0x0600CA48 RID: 51784 RVA: 0x004A16F4 File Offset: 0x0049F8F4
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
