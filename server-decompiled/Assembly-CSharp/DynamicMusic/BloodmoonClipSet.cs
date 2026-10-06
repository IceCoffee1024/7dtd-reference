using System;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A6C RID: 6764
	[Preserve]
	public class BloodmoonClipSet : LayeredContent
	{
		// Token: 0x0600CD33 RID: 52531 RVA: 0x004AC190 File Offset: 0x004AA390
		public override float GetSample(PlacementType _placement, int _idx, params float[] _params)
		{
			return this.clips[_placement].GetSample(_idx, _params);
		}

		// Token: 0x0600CD34 RID: 52532 RVA: 0x004AC1A8 File Offset: 0x004AA3A8
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
