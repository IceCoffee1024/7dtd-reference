using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001A76 RID: 6774
	public class Layer : Dictionary<int, InstrumentID>
	{
		// Token: 0x0600CC21 RID: 52257 RVA: 0x004A62D9 File Offset: 0x004A44D9
		public Layer()
		{
			if (Layer.Random == null)
			{
				Layer.Random = GameRandomManager.Instance.CreateGameRandom();
			}
		}

		// Token: 0x0600CC22 RID: 52258 RVA: 0x004A62F7 File Offset: 0x004A44F7
		public InstrumentID GetInstrumentID()
		{
			if (this.idQ == null || this.idQ.Count < 3)
			{
				this.PopulateQueue();
			}
			LayerReserve.AddLoad(this.idQ.ElementAt<InstrumentID>(1));
			return this.idQ.Dequeue();
		}

		// Token: 0x0600CC23 RID: 52259 RVA: 0x004A6334 File Offset: 0x004A4534
		public void PopulateQueue()
		{
			if (this.idQ == null)
			{
				this.idQ = new Queue<InstrumentID>(base.Values.OrderBy<InstrumentID, int>((InstrumentID e) => Layer.Random.RandomRange(int.MaxValue)));
				LayerReserve.AddLoad(this.idQ.Peek());
				return;
			}
			this.RefillQueue();
		}

		// Token: 0x0600CC24 RID: 52260 RVA: 0x004A6395 File Offset: 0x004A4595
		[PublicizedFrom(EAccessModifier.Private)]
		public void RefillQueue()
		{
			Array.ForEach<InstrumentID>(base.Values.OrderBy<InstrumentID, int>(delegate(InstrumentID e)
			{
				if (e.Name.Equals(this.idQ.Peek().Name) || e.Name.Equals(this.idQ.ElementAt<InstrumentID>(1).Name))
				{
					return int.MaxValue;
				}
				return Layer.Random.RandomRange(int.MaxValue);
			}).ToArray<InstrumentID>(), new Action<InstrumentID>(this.idQ.Enqueue));
		}

		// Token: 0x04009AA4 RID: 39588
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<InstrumentID> idQ;

		// Token: 0x04009AA5 RID: 39589
		[PublicizedFrom(EAccessModifier.Private)]
		public static GameRandom Random;
	}
}
