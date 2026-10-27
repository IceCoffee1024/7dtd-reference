using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001ACA RID: 6858
	public interface IConfigurable
	{
		// Token: 0x0600CF47 RID: 53063
		void SetConfiguration(IList<PlacementType> _placements);
	}
}
