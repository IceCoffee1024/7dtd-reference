using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A6D RID: 6765
	public interface IConfigurable
	{
		// Token: 0x0600CCDB RID: 52443
		void SetConfiguration(IList<PlacementType> _placements);
	}
}
