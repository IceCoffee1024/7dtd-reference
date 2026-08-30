using System;

namespace UniLinq
{
	// Token: 0x02001766 RID: 5990
	[PublicizedFrom(EAccessModifier.Internal)]
	public static class Check
	{
		// Token: 0x0600B91C RID: 47388 RVA: 0x004517B6 File Offset: 0x0044F9B6
		public static void Source(object source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
		}

		// Token: 0x0600B91D RID: 47389 RVA: 0x004517C6 File Offset: 0x0044F9C6
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

		// Token: 0x0600B91E RID: 47390 RVA: 0x004517E4 File Offset: 0x0044F9E4
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

		// Token: 0x0600B91F RID: 47391 RVA: 0x00451810 File Offset: 0x0044FA10
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

		// Token: 0x0600B920 RID: 47392 RVA: 0x0045182E File Offset: 0x0044FA2E
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

		// Token: 0x0600B921 RID: 47393 RVA: 0x0045184C File Offset: 0x0044FA4C
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

		// Token: 0x0600B922 RID: 47394 RVA: 0x0045186A File Offset: 0x0044FA6A
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

		// Token: 0x0600B923 RID: 47395 RVA: 0x00451888 File Offset: 0x0044FA88
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

		// Token: 0x0600B924 RID: 47396 RVA: 0x004518A6 File Offset: 0x0044FAA6
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

		// Token: 0x0600B925 RID: 47397 RVA: 0x004518D2 File Offset: 0x0044FAD2
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

		// Token: 0x0600B926 RID: 47398 RVA: 0x004518FE File Offset: 0x0044FAFE
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

		// Token: 0x0600B927 RID: 47399 RVA: 0x0045192A File Offset: 0x0044FB2A
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

		// Token: 0x0600B928 RID: 47400 RVA: 0x00451958 File Offset: 0x0044FB58
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

		// Token: 0x0600B929 RID: 47401 RVA: 0x004519AC File Offset: 0x0044FBAC
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
