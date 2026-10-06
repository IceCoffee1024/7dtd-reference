using System;
using System.Collections.Generic;
using DynamicMusic.Legacy.ObjectModel;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001AC1 RID: 6849
	public static class LayerReserve
	{
		// Token: 0x0600CEB7 RID: 52919 RVA: 0x004AFA83 File Offset: 0x004ADC83
		public static void Tick()
		{
			LayerReserve.Load();
		}

		// Token: 0x0600CEB8 RID: 52920 RVA: 0x004AFA8C File Offset: 0x004ADC8C
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

		// Token: 0x0600CEB9 RID: 52921 RVA: 0x004AFB0B File Offset: 0x004ADD0B
		public static void AddLoad(InstrumentID _id)
		{
			if (LayerReserve.toLoad == null)
			{
				LayerReserve.toLoad = new Queue<InstrumentID>();
			}
			LayerReserve.toLoad.Enqueue(_id);
		}

		// Token: 0x04009C6A RID: 40042
		[PublicizedFrom(EAccessModifier.Private)]
		public static Queue<InstrumentID> toLoad;

		// Token: 0x04009C6B RID: 40043
		[PublicizedFrom(EAccessModifier.Private)]
		public static InstrumentID CurrentLoading;
	}
}
