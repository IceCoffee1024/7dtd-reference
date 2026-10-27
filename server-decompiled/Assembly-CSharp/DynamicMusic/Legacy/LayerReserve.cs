using System;
using System.Collections.Generic;
using DynamicMusic.Legacy.ObjectModel;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001AF2 RID: 6898
	public static class LayerReserve
	{
		// Token: 0x0600CFCB RID: 53195 RVA: 0x004B357F File Offset: 0x004B177F
		public static void Tick()
		{
			LayerReserve.Load();
		}

		// Token: 0x0600CFCC RID: 53196 RVA: 0x004B3588 File Offset: 0x004B1788
		[PublicizedFrom(EAccessModifier.Private)]
		public static void Load()
		{
			if (LayerReserve.CurrentLoading == null)
			{
				if (LayerReserve.toLoad.Count > 0)
				{
					LayerReserve.CurrentLoading = LayerReserve.toLoad.Dequeue();
					LayerReserve.CurrentLoading.Load();
				}
				return;
			}
			if (!LayerReserve.CurrentLoading.IsLoaded)
			{
				LayerReserve.CurrentLoading.Load();
				return;
			}
			if (LayerReserve.toLoad.Count > 0)
			{
				LayerReserve.CurrentLoading = LayerReserve.toLoad.Dequeue();
				LayerReserve.CurrentLoading.Load();
				return;
			}
			LayerReserve.CurrentLoading = null;
		}

		// Token: 0x0600CFCD RID: 53197 RVA: 0x004B3607 File Offset: 0x004B1807
		public static void AddLoad(InstrumentID _id)
		{
			if (LayerReserve.toLoad == null)
			{
				LayerReserve.toLoad = new Queue<InstrumentID>();
			}
			LayerReserve.toLoad.Enqueue(_id);
		}

		// Token: 0x04009D53 RID: 40275
		[PublicizedFrom(EAccessModifier.Private)]
		public static Queue<InstrumentID> toLoad;

		// Token: 0x04009D54 RID: 40276
		[PublicizedFrom(EAccessModifier.Private)]
		public static InstrumentID CurrentLoading;
	}
}
