using System;

namespace UniLinq
{
	// Token: 0x020017B8 RID: 6072
	[PublicizedFrom(EAccessModifier.Internal)]
	public static class Check
	{
		// Token: 0x0600BBF6 RID: 48118 RVA: 0x0045BA4E File Offset: 0x00459C4E
		public static void Source(object source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
		}

		// Token: 0x0600BBF7 RID: 48119 RVA: 0x0045BA5E File Offset: 0x00459C5E
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

		// Token: 0x0600BBF8 RID: 48120 RVA: 0x0045BA7C File Offset: 0x00459C7C
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

		// Token: 0x0600BBF9 RID: 48121 RVA: 0x0045BAA8 File Offset: 0x00459CA8
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

		// Token: 0x0600BBFA RID: 48122 RVA: 0x0045BAC6 File Offset: 0x00459CC6
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

		// Token: 0x0600BBFB RID: 48123 RVA: 0x0045BAE4 File Offset: 0x00459CE4
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

		// Token: 0x0600BBFC RID: 48124 RVA: 0x0045BB02 File Offset: 0x00459D02
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

		// Token: 0x0600BBFD RID: 48125 RVA: 0x0045BB20 File Offset: 0x00459D20
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

		// Token: 0x0600BBFE RID: 48126 RVA: 0x0045BB3E File Offset: 0x00459D3E
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

		// Token: 0x0600BBFF RID: 48127 RVA: 0x0045BB6A File Offset: 0x00459D6A
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

		// Token: 0x0600BC00 RID: 48128 RVA: 0x0045BB96 File Offset: 0x00459D96
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

		// Token: 0x0600BC01 RID: 48129 RVA: 0x0045BBC2 File Offset: 0x00459DC2
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

		// Token: 0x0600BC02 RID: 48130 RVA: 0x0045BBF0 File Offset: 0x00459DF0
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

		// Token: 0x0600BC03 RID: 48131 RVA: 0x0045BC44 File Offset: 0x00459E44
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
