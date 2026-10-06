using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A69 RID: 6761
	[Preserve]
	public class Configuration : AbstractConfiguration, IFiniteConfiguration, IConfiguration<IList<PlacementType>>, IConfiguration
	{
		// Token: 0x1700198C RID: 6540
		// (get) Token: 0x0600CD24 RID: 52516 RVA: 0x004ABF1A File Offset: 0x004AA11A
		// (set) Token: 0x0600CD25 RID: 52517 RVA: 0x004ABF22 File Offset: 0x004AA122
		public Dictionary<LayerType, IList<PlacementType>> Layers
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CD26 RID: 52518 RVA: 0x004ABF2B File Offset: 0x004AA12B
		public Configuration()
		{
			this.Layers = new Dictionary<LayerType, IList<PlacementType>>();
		}

		// Token: 0x0600CD27 RID: 52519 RVA: 0x004ABF3E File Offset: 0x004AA13E
		public override int CountFor(LayerType _layer)
		{
			if (!this.Layers.ContainsKey(_layer))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600CD28 RID: 52520 RVA: 0x004ABF54 File Offset: 0x004AA154
		public override void ParseFromXml(XElement _xmlNode)
		{
			base.ParseFromXml(_xmlNode);
			foreach (XElement xelement in _xmlNode.Elements("layer"))
			{
				this.ParseLayers(xelement);
			}
		}

		// Token: 0x0600CD29 RID: 52521 RVA: 0x004ABFB4 File Offset: 0x004AA1B4
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
