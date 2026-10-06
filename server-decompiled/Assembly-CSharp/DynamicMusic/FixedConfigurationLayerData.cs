using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A6B RID: 6763
	[Preserve]
	public class FixedConfigurationLayerData : ICountable
	{
		// Token: 0x0600CD30 RID: 52528 RVA: 0x004AC162 File Offset: 0x004AA362
		public FixedConfigurationLayerData()
		{
			this.LayerInstances = new List<List<PlacementType>>();
		}

		// Token: 0x1700198E RID: 6542
		// (get) Token: 0x0600CD31 RID: 52529 RVA: 0x004AC175 File Offset: 0x004AA375
		public int Count
		{
			get
			{
				return this.LayerInstances.Count;
			}
		}

		// Token: 0x0600CD32 RID: 52530 RVA: 0x004AC182 File Offset: 0x004AA382
		public void Add(List<PlacementType> _list)
		{
			this.LayerInstances.Add(_list);
		}

		// Token: 0x04009BB8 RID: 39864
		public List<List<PlacementType>> LayerInstances;
	}
}
