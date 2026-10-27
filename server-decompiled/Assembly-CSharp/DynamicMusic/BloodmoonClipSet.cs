using System;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A9D RID: 6813
	[Preserve]
	public class BloodmoonClipSet : LayeredContent
	{
		// Token: 0x0600CE47 RID: 52807 RVA: 0x004AFC8C File Offset: 0x004ADE8C
		public override float GetSample(PlacementType _placement, int _idx, params float[] _params)
		{
			return this.clips[_placement].GetSample(_idx, _params);
		}

		// Token: 0x0600CE48 RID: 52808 RVA: 0x004AFCA4 File Offset: 0x004ADEA4
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
