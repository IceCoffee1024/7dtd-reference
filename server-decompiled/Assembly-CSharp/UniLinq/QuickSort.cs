using System;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x02001793 RID: 6035
	[PublicizedFrom(EAccessModifier.Internal)]
	public class QuickSort<TElement>
	{
		// Token: 0x0600BB5A RID: 47962 RVA: 0x00459914 File Offset: 0x00457B14
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

		// Token: 0x0600BB5B RID: 47963 RVA: 0x00459990 File Offset: 0x00457B90
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

		// Token: 0x0600BB5C RID: 47964 RVA: 0x004599B5 File Offset: 0x00457BB5
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

		// Token: 0x0600BB5D RID: 47965 RVA: 0x004599E5 File Offset: 0x00457BE5
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

		// Token: 0x04008C2A RID: 35882
		[PublicizedFrom(EAccessModifier.Private)]
		public TElement[] elements;

		// Token: 0x04008C2B RID: 35883
		[PublicizedFrom(EAccessModifier.Private)]
		public int[] indexes;

		// Token: 0x04008C2C RID: 35884
		[PublicizedFrom(EAccessModifier.Private)]
		public SortContext<TElement> context;
	}
}
