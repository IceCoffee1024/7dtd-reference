using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A99 RID: 6809
	public interface IConfigurable
	{
		// Token: 0x0600CE33 RID: 52787
		void SetConfiguration(IList<PlacementType> _placements);
	}
}
