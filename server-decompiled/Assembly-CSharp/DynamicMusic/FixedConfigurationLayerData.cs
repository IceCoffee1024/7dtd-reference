using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A9C RID: 6812
	[Preserve]
	public class FixedConfigurationLayerData : ICountable
	{
		// Token: 0x0600CE44 RID: 52804 RVA: 0x004AFC5E File Offset: 0x004ADE5E
		public FixedConfigurationLayerData()
		{
			this.LayerInstances = new List<List<PlacementType>>();
		}

		// Token: 0x170019D6 RID: 6614
		// (get) Token: 0x0600CE45 RID: 52805 RVA: 0x004AFC71 File Offset: 0x004ADE71
		public int Count
		{
			get
			{
				return this.LayerInstances.Count;
			}
		}

		// Token: 0x0600CE46 RID: 52806 RVA: 0x004AFC7E File Offset: 0x004ADE7E
		public void Add(List<PlacementType> _list)
		{
			this.LayerInstances.Add(_list);
		}

		// Token: 0x04009CA1 RID: 40097
		public List<List<PlacementType>> LayerInstances;
	}
}
