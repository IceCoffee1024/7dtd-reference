using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A46 RID: 6726
	public interface IConfigurable
	{
		// Token: 0x0600CB52 RID: 52050
		void SetConfiguration(IList<PlacementType> _placements);
	}
}
