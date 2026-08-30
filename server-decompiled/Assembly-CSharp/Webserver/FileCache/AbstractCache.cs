using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Webserver.FileCache
{
	// Token: 0x02001B08 RID: 6920
	public abstract class AbstractCache
	{
		// Token: 0x0600CFF7 RID: 53239
		public abstract byte[] GetFileContent(string _filename);

		// Token: 0x0600CFF8 RID: 53240
		[return: TupleElementNames(new string[] { "filesDropped", "bytesDropped" })]
		public abstract ValueTuple<int, int> Invalidate();

		// Token: 0x0600CFF9 RID: 53241 RVA: 0x004BB66E File Offset: 0x004B986E
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractCache()
		{
			AbstractCache.caches.Add(this);
		}

		// Token: 0x0600CFFA RID: 53242 RVA: 0x004BB684 File Offset: 0x004B9884
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

		// Token: 0x04009DF0 RID: 40432
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly List<AbstractCache> caches = new List<AbstractCache>();
	}
}
