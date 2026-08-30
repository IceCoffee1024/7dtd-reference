using System;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A41 RID: 6721
	[Preserve]
	public class ClipSet : LayeredContent
	{
		// Token: 0x0600CBDE RID: 52190 RVA: 0x004A7D7C File Offset: 0x004A5F7C
		public override float GetSample(PlacementType _placement, int _idx, params float[] _params)
		{
			IClipAdapter clipAdapter;
			if (!this.clips.TryGetValue(_placement, out clipAdapter))
			{
				clipAdapter = this.clips[PlacementType.Loop];
			}
			return clipAdapter.GetSample(_idx, _params);
		}

		// Token: 0x0600CBDF RID: 52191 RVA: 0x004A7DB0 File Offset: 0x004A5FB0
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
