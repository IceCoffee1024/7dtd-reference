using System;
using System.Xml.Linq;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A9E RID: 6814
	[Preserve]
	public class ClipSet : LayeredContent
	{
		// Token: 0x0600CE4A RID: 52810 RVA: 0x004AFD44 File Offset: 0x004ADF44
		public override float GetSample(PlacementType _placement, int _idx, params float[] _params)
		{
			IClipAdapter clipAdapter;
			if (!this.clips.TryGetValue(_placement, out clipAdapter))
			{
				clipAdapter = this.clips[PlacementType.Loop];
			}
			return clipAdapter.GetSample(_idx, _params);
		}

		// Token: 0x0600CE4B RID: 52811 RVA: 0x004AFD78 File Offset: 0x004ADF78
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
