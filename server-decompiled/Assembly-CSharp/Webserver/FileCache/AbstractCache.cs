using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Webserver.FileCache
{
	// Token: 0x02001B65 RID: 7013
	public abstract class AbstractCache
	{
		// Token: 0x0600D263 RID: 53859
		public abstract byte[] GetFileContent(string _filename);

		// Token: 0x0600D264 RID: 53860
		[return: TupleElementNames(new string[] { "filesDropped", "bytesDropped" })]
		public abstract ValueTuple<int, int> Invalidate();

		// Token: 0x0600D265 RID: 53861 RVA: 0x004C3662 File Offset: 0x004C1862
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractCache()
		{
			AbstractCache.caches.Add(this);
		}

		// Token: 0x0600D266 RID: 53862 RVA: 0x004C3678 File Offset: 0x004C1878
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

		// Token: 0x04009F9F RID: 40863
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly List<AbstractCache> caches = new List<AbstractCache>();
	}
}
