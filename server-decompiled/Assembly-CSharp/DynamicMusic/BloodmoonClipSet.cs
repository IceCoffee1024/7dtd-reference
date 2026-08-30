using System;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A19 RID: 6681
	[Preserve]
	public class BloodmoonClipSet : LayeredContent
	{
		// Token: 0x0600CA52 RID: 51794 RVA: 0x004A18D0 File Offset: 0x0049FAD0
		public override float GetSample(PlacementType _placement, int _idx, params float[] _params)
		{
			return this.clips[_placement].GetSample(_idx, _params);
		}

		// Token: 0x0600CA53 RID: 51795 RVA: 0x004A18E8 File Offset: 0x0049FAE8
		public override void ParseFromXml(XElement _xmlNode)
		{
			base.ParseFromXml(_xmlNode);
			foreach (XElement xelement in _xmlNode.Elements("clip"))
			{
				PlacementType placementType = EnumUtils.Parse<PlacementType>(xelement.GetAttribute("key"), false);
				IClipAdapter clipAdapter = LayeredContent.CreateClipAdapter(xelement.GetAttribute("type"));
				clipAdapter.ParseXml(xelement);
				this.clips.Add(placementType, clipAdapter);
			}
		}
	}
}
