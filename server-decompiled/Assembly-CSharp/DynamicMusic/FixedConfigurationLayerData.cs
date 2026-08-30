using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A18 RID: 6680
	[Preserve]
	public class FixedConfigurationLayerData : ICountable
	{
		// Token: 0x0600CA4F RID: 51791 RVA: 0x004A18A2 File Offset: 0x0049FAA2
		public FixedConfigurationLayerData()
		{
			this.LayerInstances = new List<List<PlacementType>>();
		}

		// Token: 0x1700187A RID: 6266
		// (get) Token: 0x0600CA50 RID: 51792 RVA: 0x004A18B5 File Offset: 0x0049FAB5
		public int Count
		{
			get
			{
				return this.LayerInstances.Count;
			}
		}

		// Token: 0x0600CA51 RID: 51793 RVA: 0x004A18C2 File Offset: 0x0049FAC2
		public void Add(List<PlacementType> _list)
		{
			this.LayerInstances.Add(_list);
		}

		// Token: 0x040099C1 RID: 39361
		public List<List<PlacementType>> LayerInstances;
	}
}
