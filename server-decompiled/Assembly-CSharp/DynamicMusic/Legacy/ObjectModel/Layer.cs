using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001AFA RID: 6906
	public class Layer : Dictionary<int, InstrumentID>
	{
		// Token: 0x0600D016 RID: 53270 RVA: 0x004B4695 File Offset: 0x004B2895
		public Layer()
		{
			if (Layer.Random == null)
			{
				Layer.Random = GameRandomManager.Instance.CreateGameRandom();
			}
		}

		// Token: 0x0600D017 RID: 53271 RVA: 0x004B46B3 File Offset: 0x004B28B3
		public InstrumentID GetInstrumentID()
		{
			if (this.idQ == null || this.idQ.Count < 3)
			{
				this.PopulateQueue();
			}
			LayerReserve.AddLoad(this.idQ.ElementAt<InstrumentID>(1));
			return this.idQ.Dequeue();
		}

		// Token: 0x0600D018 RID: 53272 RVA: 0x004B46F0 File Offset: 0x004B28F0
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

		// Token: 0x0600D019 RID: 53273 RVA: 0x004B4751 File Offset: 0x004B2951
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

		// Token: 0x04009D84 RID: 40324
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<InstrumentID> idQ;

		// Token: 0x04009D85 RID: 40325
		[PublicizedFrom(EAccessModifier.Private)]
		public static GameRandom Random;
	}
}
