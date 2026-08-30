using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Webserver.FileCache
{
	// Token: 0x02001AE1 RID: 6881
	public abstract class AbstractCache
	{
		// Token: 0x0600CE6E RID: 52846
		public abstract byte[] GetFileContent(string _filename);

		// Token: 0x0600CE6F RID: 52847
		[return: TupleElementNames(new string[] { "filesDropped", "bytesDropped" })]
		public abstract ValueTuple<int, int> Invalidate();

		// Token: 0x0600CE70 RID: 52848 RVA: 0x004B527A File Offset: 0x004B347A
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractCache()
		{
			AbstractCache.caches.Add(this);
		}

		// Token: 0x0600CE71 RID: 52849 RVA: 0x004B5290 File Offset: 0x004B3490
		public static ValueTuple<int, int> InvalidateAllCaches()
		{
			int num = 0;
			int num2 = 0;
			foreach (AbstractCache abstractCache in AbstractCache.caches)
			{
				ValueTuple<int, int> valueTuple = abstractCache.Invalidate();
				int item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				num += item;
				num2 += item2;
			}
			return new ValueTuple<int, int>(num, num2);
		}

		// Token: 0x04009CBF RID: 40127
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly List<AbstractCache> caches = new List<AbstractCache>();
	}
}
