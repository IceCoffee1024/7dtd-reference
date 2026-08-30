using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001A9D RID: 6813
	public class Layer : Dictionary<int, InstrumentID>
	{
		// Token: 0x0600CDAA RID: 52650 RVA: 0x004AC6CD File Offset: 0x004AA8CD
		public Layer()
		{
			if (Layer.Random == null)
			{
				Layer.Random = GameRandomManager.Instance.CreateGameRandom();
			}
		}

		// Token: 0x0600CDAB RID: 52651 RVA: 0x004AC6EB File Offset: 0x004AA8EB
		public InstrumentID GetInstrumentID()
		{
			if (this.idQ == null || this.idQ.Count < 3)
			{
				this.PopulateQueue();
			}
			LayerReserve.AddLoad(this.idQ.ElementAt<InstrumentID>(1));
			return this.idQ.Dequeue();
		}

		// Token: 0x0600CDAC RID: 52652 RVA: 0x004AC728 File Offset: 0x004AA928
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

		// Token: 0x0600CDAD RID: 52653 RVA: 0x004AC789 File Offset: 0x004AA989
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

		// Token: 0x04009BD5 RID: 39893
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<InstrumentID> idQ;

		// Token: 0x04009BD6 RID: 39894
		[PublicizedFrom(EAccessModifier.Private)]
		public static GameRandom Random;
	}
}
