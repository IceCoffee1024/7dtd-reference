using System;

namespace UniLinq
{
	// Token: 0x0200178C RID: 6028
	[PublicizedFrom(EAccessModifier.Internal)]
	public static class Check
	{
		// Token: 0x0600BA9F RID: 47775 RVA: 0x0045752E File Offset: 0x0045572E
		public static void Source(object source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
		}

		// Token: 0x0600BAA0 RID: 47776 RVA: 0x0045753E File Offset: 0x0045573E
		public static void Source1AndSource2(object source1, object source2)
		{
			if (source1 == null)
			{
				throw new ArgumentNullException("source1");
			}
			if (source2 == null)
			{
				throw new ArgumentNullException("source2");
			}
		}

		// Token: 0x0600BAA1 RID: 47777 RVA: 0x0045755C File Offset: 0x0045575C
		public static void SourceAndFuncAndSelector(object source, object func, object selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (func == null)
			{
				throw new ArgumentNullException("func");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
		}

		// Token: 0x0600BAA2 RID: 47778 RVA: 0x00457588 File Offset: 0x00455788
		public static void SourceAndFunc(object source, object func)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (func == null)
			{
				throw new ArgumentNullException("func");
			}
		}

		// Token: 0x0600BAA3 RID: 47779 RVA: 0x004575A6 File Offset: 0x004557A6
		public static void SourceAndSelector(object source, object selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
		}

		// Token: 0x0600BAA4 RID: 47780 RVA: 0x004575C4 File Offset: 0x004557C4
		public static void SourceAndPredicate(object source, object predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
		}

		// Token: 0x0600BAA5 RID: 47781 RVA: 0x004575E2 File Offset: 0x004557E2
		public static void FirstAndSecond(object first, object second)
		{
			if (first == null)
			{
				throw new ArgumentNullException("first");
			}
			if (second == null)
			{
				throw new ArgumentNullException("second");
			}
		}

		// Token: 0x0600BAA6 RID: 47782 RVA: 0x00457600 File Offset: 0x00455800
		public static void SourceAndKeySelector(object source, object keySelector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (keySelector == null)
			{
				throw new ArgumentNullException("keySelector");
			}
		}

		// Token: 0x0600BAA7 RID: 47783 RVA: 0x0045761E File Offset: 0x0045581E
		public static void SourceAndKeyElementSelectors(object source, object keySelector, object elementSelector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (keySelector == null)
			{
				throw new ArgumentNullException("keySelector");
			}
			if (elementSelector == null)
			{
				throw new ArgumentNullException("elementSelector");
			}
		}

		// Token: 0x0600BAA8 RID: 47784 RVA: 0x0045764A File Offset: 0x0045584A
		public static void SourceAndKeyResultSelectors(object source, object keySelector, object resultSelector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (keySelector == null)
			{
				throw new ArgumentNullException("keySelector");
			}
			if (resultSelector == null)
			{
				throw new ArgumentNullException("resultSelector");
			}
		}

		// Token: 0x0600BAA9 RID: 47785 RVA: 0x00457676 File Offset: 0x00455876
		public static void SourceAndCollectionSelectorAndResultSelector(object source, object collectionSelector, object resultSelector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (collectionSelector == null)
			{
				throw new ArgumentNullException("collectionSelector");
			}
			if (resultSelector == null)
			{
				throw new ArgumentNullException("resultSelector");
			}
		}

		// Token: 0x0600BAAA RID: 47786 RVA: 0x004576A2 File Offset: 0x004558A2
		public static void SourceAndCollectionSelectors(object source, object collectionSelector, object selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (collectionSelector == null)
			{
				throw new ArgumentNullException("collectionSelector");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
		}

		// Token: 0x0600BAAB RID: 47787 RVA: 0x004576D0 File Offset: 0x004558D0
		public static void JoinSelectors(object outer, object inner, object outerKeySelector, object innerKeySelector, object resultSelector)
		{
			if (outer == null)
			{
				throw new ArgumentNullException("outer");
			}
			if (inner == null)
			{
				throw new ArgumentNullException("inner");
			}
			if (outerKeySelector == null)
			{
				throw new ArgumentNullException("outerKeySelector");
			}
			if (innerKeySelector == null)
			{
				throw new ArgumentNullException("innerKeySelector");
			}
			if (resultSelector == null)
			{
				throw new ArgumentNullException("resultSelector");
			}
		}

		// Token: 0x0600BAAC RID: 47788 RVA: 0x00457724 File Offset: 0x00455924
		public static void GroupBySelectors(object source, object keySelector, object elementSelector, object resultSelector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (keySelector == null)
			{
				throw new ArgumentNullException("keySelector");
			}
			if (elementSelector == null)
			{
				throw new ArgumentNullException("elementSelector");
			}
			if (resultSelector == null)
			{
				throw new ArgumentNullException("resultSelector");
			}
		}
	}
}
