using System;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017B9 RID: 6073
	[PublicizedFrom(EAccessModifier.Internal)]
	public class QuickSort<TElement>
	{
		// Token: 0x0600BCDD RID: 48349 RVA: 0x0045F68C File Offset: 0x0045D88C
		[PublicizedFrom(EAccessModifier.Private)]
		public QuickSort(IEnumerable<TElement> source, SortContext<TElement> context)
		{
			List<TElement> list = new List<TElement>();
			foreach (TElement telement in source)
			{
				list.Add(telement);
			}
			this.elements = list.ToArray();
			this.indexes = QuickSort<TElement>.CreateIndexes(this.elements.Length);
			this.context = context;
		}

		// Token: 0x0600BCDE RID: 48350 RVA: 0x0045F708 File Offset: 0x0045D908
		[PublicizedFrom(EAccessModifier.Private)]
		public static int[] CreateIndexes(int length)
		{
			int[] array = new int[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = i;
			}
			return array;
		}

		// Token: 0x0600BCDF RID: 48351 RVA: 0x0045F72D File Offset: 0x0045D92D
		[PublicizedFrom(EAccessModifier.Private)]
		public void PerformSort()
		{
			if (this.elements.Length <= 1)
			{
				return;
			}
			this.context.Initialize(this.elements);
			Array.Sort<int>(this.indexes, this.context);
		}

		// Token: 0x0600BCE0 RID: 48352 RVA: 0x0045F75D File Offset: 0x0045D95D
		public static IEnumerable<TElement> Sort(IEnumerable<TElement> source, SortContext<TElement> context)
		{
			QuickSort<TElement> sorter = new QuickSort<TElement>(source, context);
			sorter.PerformSort();
			int num;
			for (int i = 0; i < sorter.elements.Length; i = num + 1)
			{
				yield return sorter.elements[sorter.indexes[i]];
				num = i;
			}
			yield break;
		}

		// Token: 0x04008D46 RID: 36166
		[PublicizedFrom(EAccessModifier.Private)]
		public TElement[] elements;

		// Token: 0x04008D47 RID: 36167
		[PublicizedFrom(EAccessModifier.Private)]
		public int[] indexes;

		// Token: 0x04008D48 RID: 36168
		[PublicizedFrom(EAccessModifier.Private)]
		public SortContext<TElement> context;
	}
}
