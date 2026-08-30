using System;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A1A RID: 6682
	[Preserve]
	public class ClipSet : LayeredContent
	{
		// Token: 0x0600CA55 RID: 51797 RVA: 0x004A1988 File Offset: 0x0049FB88
		public override float GetSample(PlacementType _placement, int _idx, params float[] _params)
		{
			IClipAdapter clipAdapter;
			if (!this.clips.TryGetValue(_placement, out clipAdapter))
			{
				clipAdapter = this.clips[PlacementType.Loop];
			}
			return clipAdapter.GetSample(_idx, _params);
		}

		// Token: 0x0600CA56 RID: 51798 RVA: 0x004A19BC File Offset: 0x0049FBBC
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
