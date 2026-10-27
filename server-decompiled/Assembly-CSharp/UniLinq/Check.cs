using System;

namespace UniLinq
{
	// Token: 0x020017E9 RID: 6121
	[PublicizedFrom(EAccessModifier.Internal)]
	public static class Check
	{
		// Token: 0x0600BD0A RID: 48394 RVA: 0x0045F3FA File Offset: 0x0045D5FA
		public static void Source(object source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
		}

		// Token: 0x0600BD0B RID: 48395 RVA: 0x0045F40A File Offset: 0x0045D60A
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

		// Token: 0x0600BD0C RID: 48396 RVA: 0x0045F428 File Offset: 0x0045D628
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

		// Token: 0x0600BD0D RID: 48397 RVA: 0x0045F454 File Offset: 0x0045D654
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

		// Token: 0x0600BD0E RID: 48398 RVA: 0x0045F472 File Offset: 0x0045D672
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

		// Token: 0x0600BD0F RID: 48399 RVA: 0x0045F490 File Offset: 0x0045D690
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

		// Token: 0x0600BD10 RID: 48400 RVA: 0x0045F4AE File Offset: 0x0045D6AE
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

		// Token: 0x0600BD11 RID: 48401 RVA: 0x0045F4CC File Offset: 0x0045D6CC
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

		// Token: 0x0600BD12 RID: 48402 RVA: 0x0045F4EA File Offset: 0x0045D6EA
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

		// Token: 0x0600BD13 RID: 48403 RVA: 0x0045F516 File Offset: 0x0045D716
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

		// Token: 0x0600BD14 RID: 48404 RVA: 0x0045F542 File Offset: 0x0045D742
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

		// Token: 0x0600BD15 RID: 48405 RVA: 0x0045F56E File Offset: 0x0045D76E
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

		// Token: 0x0600BD16 RID: 48406 RVA: 0x0045F59C File Offset: 0x0045D79C
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

		// Token: 0x0600BD17 RID: 48407 RVA: 0x0045F5F0 File Offset: 0x0045D7F0
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
