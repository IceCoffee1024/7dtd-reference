using System;
using System.Collections.Generic;
using DynamicMusic.Legacy.ObjectModel;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001A95 RID: 6805
	public static class LayerReserve
	{
		// Token: 0x0600CD5F RID: 52575 RVA: 0x004AB5B7 File Offset: 0x004A97B7
		public static void Tick()
		{
			LayerReserve.Load();
		}

		// Token: 0x0600CD60 RID: 52576 RVA: 0x004AB5C0 File Offset: 0x004A97C0
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

		// Token: 0x0600CD61 RID: 52577 RVA: 0x004AB63F File Offset: 0x004A983F
		public static void AddLoad(InstrumentID _id)
		{
			if (LayerReserve.toLoad == null)
			{
				LayerReserve.toLoad = new Queue<InstrumentID>();
			}
			LayerReserve.toLoad.Enqueue(_id);
		}

		// Token: 0x04009BA4 RID: 39844
		[PublicizedFrom(EAccessModifier.Private)]
		public static Queue<InstrumentID> toLoad;

		// Token: 0x04009BA5 RID: 39845
		[PublicizedFrom(EAccessModifier.Private)]
		public static InstrumentID CurrentLoading;
	}
}
