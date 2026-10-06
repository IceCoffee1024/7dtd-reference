using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Webserver.FileCache
{
	// Token: 0x02001B34 RID: 6964
	public abstract class AbstractCache
	{
		// Token: 0x0600D14F RID: 53583
		public abstract byte[] GetFileContent(string _filename);

		// Token: 0x0600D150 RID: 53584
		[return: TupleElementNames(new string[] { "filesDropped", "bytesDropped" })]
		public abstract ValueTuple<int, int> Invalidate();

		// Token: 0x0600D151 RID: 53585 RVA: 0x004BFB3A File Offset: 0x004BDD3A
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractCache()
		{
			AbstractCache.caches.Add(this);
		}

		// Token: 0x0600D152 RID: 53586 RVA: 0x004BFB50 File Offset: 0x004BDD50
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

		// Token: 0x04009EB6 RID: 40630
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly List<AbstractCache> caches = new List<AbstractCache>();
	}
}
