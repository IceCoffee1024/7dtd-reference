using System;
using System.Collections.Generic;
using DynamicMusic.Legacy.ObjectModel;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001A6E RID: 6766
	public static class LayerReserve
	{
		// Token: 0x0600CBD6 RID: 52182 RVA: 0x004A51C3 File Offset: 0x004A33C3
		public static void Tick()
		{
			LayerReserve.Load();
		}

		// Token: 0x0600CBD7 RID: 52183 RVA: 0x004A51CC File Offset: 0x004A33CC
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

		// Token: 0x0600CBD8 RID: 52184 RVA: 0x004A524B File Offset: 0x004A344B
		public static void AddLoad(InstrumentID _id)
		{
			if (LayerReserve.toLoad == null)
			{
				LayerReserve.toLoad = new Queue<InstrumentID>();
			}
			LayerReserve.toLoad.Enqueue(_id);
		}

		// Token: 0x04009A73 RID: 39539
		[PublicizedFrom(EAccessModifier.Private)]
		public static Queue<InstrumentID> toLoad;

		// Token: 0x04009A74 RID: 39540
		[PublicizedFrom(EAccessModifier.Private)]
		public static InstrumentID CurrentLoading;
	}
}
