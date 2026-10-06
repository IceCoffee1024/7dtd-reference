using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001AC9 RID: 6857
	public class Layer : Dictionary<int, InstrumentID>
	{
		// Token: 0x0600CF02 RID: 52994 RVA: 0x004B0B99 File Offset: 0x004AED99
		public Layer()
		{
			if (Layer.Random == null)
			{
				Layer.Random = GameRandomManager.Instance.CreateGameRandom();
			}
		}

		// Token: 0x0600CF03 RID: 52995 RVA: 0x004B0BB7 File Offset: 0x004AEDB7
		public InstrumentID GetInstrumentID()
		{
			if (this.idQ == null || this.idQ.Count < 3)
			{
				this.PopulateQueue();
			}
			LayerReserve.AddLoad(this.idQ.ElementAt<InstrumentID>(1));
			return this.idQ.Dequeue();
		}

		// Token: 0x0600CF04 RID: 52996 RVA: 0x004B0BF4 File Offset: 0x004AEDF4
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

		// Token: 0x0600CF05 RID: 52997 RVA: 0x004B0C55 File Offset: 0x004AEE55
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

		// Token: 0x04009C9B RID: 40091
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<InstrumentID> idQ;

		// Token: 0x04009C9C RID: 40092
		[PublicizedFrom(EAccessModifier.Private)]
		public static GameRandom Random;
	}
}
