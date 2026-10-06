using System;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017E5 RID: 6117
	[PublicizedFrom(EAccessModifier.Internal)]
	public class QuickSort<TElement>
	{
		// Token: 0x0600BE34 RID: 48692 RVA: 0x00463BAC File Offset: 0x00461DAC
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

		// Token: 0x0600BE35 RID: 48693 RVA: 0x00463C28 File Offset: 0x00461E28
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

		// Token: 0x0600BE36 RID: 48694 RVA: 0x00463C4D File Offset: 0x00461E4D
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

		// Token: 0x0600BE37 RID: 48695 RVA: 0x00463C7D File Offset: 0x00461E7D
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

		// Token: 0x04008E0B RID: 36363
		[PublicizedFrom(EAccessModifier.Private)]
		public TElement[] elements;

		// Token: 0x04008E0C RID: 36364
		[PublicizedFrom(EAccessModifier.Private)]
		public int[] indexes;

		// Token: 0x04008E0D RID: 36365
		[PublicizedFrom(EAccessModifier.Private)]
		public SortContext<TElement> context;
	}
}
