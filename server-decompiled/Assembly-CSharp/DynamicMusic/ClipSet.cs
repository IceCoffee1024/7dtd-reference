using System;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A6D RID: 6765
	[Preserve]
	public class ClipSet : LayeredContent
	{
		// Token: 0x0600CD36 RID: 52534 RVA: 0x004AC248 File Offset: 0x004AA448
		public override float GetSample(PlacementType _placement, int _idx, params float[] _params)
		{
			IClipAdapter clipAdapter;
			if (!this.clips.TryGetValue(_placement, out clipAdapter))
			{
				clipAdapter = this.clips[PlacementType.Loop];
			}
			return clipAdapter.GetSample(_idx, _params);
		}

		// Token: 0x0600CD37 RID: 52535 RVA: 0x004AC27C File Offset: 0x004AA47C
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
