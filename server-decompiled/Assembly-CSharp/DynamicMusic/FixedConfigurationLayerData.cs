using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A3F RID: 6719
	[Preserve]
	public class FixedConfigurationLayerData : ICountable
	{
		// Token: 0x0600CBD8 RID: 52184 RVA: 0x004A7C96 File Offset: 0x004A5E96
		public FixedConfigurationLayerData()
		{
			this.LayerInstances = new List<List<PlacementType>>();
		}

		// Token: 0x170018EC RID: 6380
		// (get) Token: 0x0600CBD9 RID: 52185 RVA: 0x004A7CA9 File Offset: 0x004A5EA9
		public int Count
		{
			get
			{
				return this.LayerInstances.Count;
			}
		}

		// Token: 0x0600CBDA RID: 52186 RVA: 0x004A7CB6 File Offset: 0x004A5EB6
		public void Add(List<PlacementType> _list)
		{
			this.LayerInstances.Add(_list);
		}

		// Token: 0x04009AF2 RID: 39666
		public List<List<PlacementType>> LayerInstances;
	}
}
