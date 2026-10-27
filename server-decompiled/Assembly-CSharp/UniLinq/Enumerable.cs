using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017EA RID: 6122
	public static class Enumerable
	{
		// Token: 0x0600BD18 RID: 48408 RVA: 0x0045F62C File Offset: 0x0045D82C
		public static TSource Aggregate<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
		{
			Check.SourceAndFunc(source, func);
			TSource tsource3;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Enumerable.EmptySequence();
				}
				TSource tsource = enumerator.Current;
				while (enumerator.MoveNext())
				{
					TSource tsource2 = enumerator.Current;
					tsource = func(tsource, tsource2);
				}
				tsource3 = tsource;
			}
			return tsource3;
		}

		// Token: 0x0600BD19 RID: 48409 RVA: 0x0045F694 File Offset: 0x0045D894
		public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
		{
			Check.SourceAndFunc(source, func);
			TAccumulate taccumulate = seed;
			foreach (TSource tsource in source)
			{
				taccumulate = func(taccumulate, tsource);
			}
			return taccumulate;
		}

		// Token: 0x0600BD1A RID: 48410 RVA: 0x0045F6E8 File Offset: 0x0045D8E8
		public static TResult Aggregate<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
		{
			Check.SourceAndFunc(source, func);
			if (resultSelector == null)
			{
				throw new ArgumentNullException("resultSelector");
			}
			TAccumulate taccumulate = seed;
			foreach (TSource tsource in source)
			{
				taccumulate = func(taccumulate, tsource);
			}
			return resultSelector(taccumulate);
		}

		// Token: 0x0600BD1B RID: 48411 RVA: 0x0045F750 File Offset: 0x0045D950
		public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			foreach (TSource tsource in source)
			{
				if (!predicate(tsource))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600BD1C RID: 48412 RVA: 0x0045F7A8 File Offset: 0x0045D9A8
		public static bool Any<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			ICollection<TSource> collection = source as ICollection<TSource>;
			if (collection != null)
			{
				return collection.Count > 0;
			}
			bool flag;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				flag = enumerator.MoveNext();
			}
			return flag;
		}

		// Token: 0x0600BD1D RID: 48413 RVA: 0x0045F7FC File Offset: 0x0045D9FC
		public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			foreach (TSource tsource in source)
			{
				if (predicate(tsource))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600BD1E RID: 48414 RVA: 0x0012B879 File Offset: 0x00129A79
		public static IEnumerable<TSource> AsEnumerable<TSource>(this IEnumerable<TSource> source)
		{
			return source;
		}

		// Token: 0x0600BD1F RID: 48415 RVA: 0x0045F854 File Offset: 0x0045DA54
		public static double Average(this IEnumerable<int> source)
		{
			Check.Source(source);
			long num = 0L;
			int num2 = 0;
			foreach (int num3 in source)
			{
				checked
				{
					num += unchecked((long)num3);
				}
				num2++;
			}
			if (num2 == 0)
			{
				throw Enumerable.EmptySequence();
			}
			return (double)num / (double)num2;
		}

		// Token: 0x0600BD20 RID: 48416 RVA: 0x0045F8B8 File Offset: 0x0045DAB8
		public static double Average(this IEnumerable<long> source)
		{
			Check.Source(source);
			long num = 0L;
			long num2 = 0L;
			foreach (long num3 in source)
			{
				num += num3;
				num2 += 1L;
			}
			if (num2 == 0L)
			{
				throw Enumerable.EmptySequence();
			}
			return (double)num / (double)num2;
		}

		// Token: 0x0600BD21 RID: 48417 RVA: 0x0045F91C File Offset: 0x0045DB1C
		public static double Average(this IEnumerable<double> source)
		{
			Check.Source(source);
			double num = 0.0;
			long num2 = 0L;
			foreach (double num3 in source)
			{
				num += num3;
				num2 += 1L;
			}
			if (num2 == 0L)
			{
				throw Enumerable.EmptySequence();
			}
			return num / (double)num2;
		}

		// Token: 0x0600BD22 RID: 48418 RVA: 0x0045F988 File Offset: 0x0045DB88
		public static float Average(this IEnumerable<float> source)
		{
			Check.Source(source);
			float num = 0f;
			long num2 = 0L;
			foreach (float num3 in source)
			{
				num += num3;
				num2 += 1L;
			}
			if (num2 == 0L)
			{
				throw Enumerable.EmptySequence();
			}
			return num / (float)num2;
		}

		// Token: 0x0600BD23 RID: 48419 RVA: 0x0045F9F0 File Offset: 0x0045DBF0
		public static decimal Average(this IEnumerable<decimal> source)
		{
			Check.Source(source);
			decimal num = 0m;
			long num2 = 0L;
			foreach (decimal num3 in source)
			{
				num += num3;
				num2 += 1L;
			}
			if (num2 == 0L)
			{
				throw Enumerable.EmptySequence();
			}
			return num / num2;
		}

		// Token: 0x0600BD24 RID: 48420 RVA: 0x0045FA64 File Offset: 0x0045DC64
		[PublicizedFrom(EAccessModifier.Private)]
		public static TResult? AverageNullable<TElement, TAggregate, TResult>(this IEnumerable<TElement?> source, Func<TAggregate, TElement, TAggregate> func, Func<TAggregate, long, TResult> result) where TElement : struct where TAggregate : struct where TResult : struct
		{
			Check.Source(source);
			TAggregate taggregate = default(TAggregate);
			long num = 0L;
			foreach (TElement? telement in source)
			{
				if (telement != null)
				{
					taggregate = func(taggregate, telement.Value);
					num += 1L;
				}
			}
			if (num == 0L)
			{
				return null;
			}
			return new TResult?(result(taggregate, num));
		}

		// Token: 0x0600BD25 RID: 48421 RVA: 0x0045FAF0 File Offset: 0x0045DCF0
		public static double? Average(this IEnumerable<int?> source)
		{
			Check.Source(source);
			long num = 0L;
			long num2 = 0L;
			foreach (int? num3 in source)
			{
				if (num3 != null)
				{
					num += (long)num3.Value;
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new double?((double)num / (double)num2);
		}

		// Token: 0x0600BD26 RID: 48422 RVA: 0x0045FB70 File Offset: 0x0045DD70
		public static double? Average(this IEnumerable<long?> source)
		{
			Check.Source(source);
			long num = 0L;
			long num2 = 0L;
			foreach (long? num3 in source)
			{
				if (num3 != null)
				{
					checked
					{
						num += num3.Value;
					}
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new double?((double)num / (double)num2);
		}

		// Token: 0x0600BD27 RID: 48423 RVA: 0x0045FBF0 File Offset: 0x0045DDF0
		public static double? Average(this IEnumerable<double?> source)
		{
			Check.Source(source);
			double num = 0.0;
			long num2 = 0L;
			foreach (double? num3 in source)
			{
				if (num3 != null)
				{
					num += num3.Value;
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new double?(num / (double)num2);
		}

		// Token: 0x0600BD28 RID: 48424 RVA: 0x0045FC74 File Offset: 0x0045DE74
		public static decimal? Average(this IEnumerable<decimal?> source)
		{
			Check.Source(source);
			decimal num = 0m;
			long num2 = 0L;
			foreach (decimal? num3 in source)
			{
				if (num3 != null)
				{
					num += num3.Value;
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new decimal?(num / num2);
		}

		// Token: 0x0600BD29 RID: 48425 RVA: 0x0045FD04 File Offset: 0x0045DF04
		public static float? Average(this IEnumerable<float?> source)
		{
			Check.Source(source);
			float num = 0f;
			long num2 = 0L;
			foreach (float? num3 in source)
			{
				if (num3 != null)
				{
					num += num3.Value;
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new float?(num / (float)num2);
		}

		// Token: 0x0600BD2A RID: 48426 RVA: 0x0045FD84 File Offset: 0x0045DF84
		public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
		{
			Check.SourceAndSelector(source, selector);
			long num = 0L;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				num += (long)selector(tsource);
				num2 += 1L;
			}
			if (num2 == 0L)
			{
				throw Enumerable.EmptySequence();
			}
			return (double)num / (double)num2;
		}

		// Token: 0x0600BD2B RID: 48427 RVA: 0x0045FDF0 File Offset: 0x0045DFF0
		public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
		{
			Check.SourceAndSelector(source, selector);
			long num = 0L;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				int? num3 = selector(tsource);
				if (num3 != null)
				{
					num += (long)num3.Value;
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new double?((double)num / (double)num2);
		}

		// Token: 0x0600BD2C RID: 48428 RVA: 0x0045FE78 File Offset: 0x0045E078
		public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
		{
			Check.SourceAndSelector(source, selector);
			long num = 0L;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				checked
				{
					num += selector(tsource);
				}
				num2 += 1L;
			}
			if (num2 == 0L)
			{
				throw Enumerable.EmptySequence();
			}
			return (double)num / (double)num2;
		}

		// Token: 0x0600BD2D RID: 48429 RVA: 0x0045FEE4 File Offset: 0x0045E0E4
		public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
		{
			Check.SourceAndSelector(source, selector);
			long num = 0L;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				long? num3 = selector(tsource);
				if (num3 != null)
				{
					checked
					{
						num += num3.Value;
					}
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new double?((double)num / (double)num2);
		}

		// Token: 0x0600BD2E RID: 48430 RVA: 0x0045FF6C File Offset: 0x0045E16C
		public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
		{
			Check.SourceAndSelector(source, selector);
			double num = 0.0;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				num += selector(tsource);
				num2 += 1L;
			}
			if (num2 == 0L)
			{
				throw Enumerable.EmptySequence();
			}
			return num / (double)num2;
		}

		// Token: 0x0600BD2F RID: 48431 RVA: 0x0045FFE0 File Offset: 0x0045E1E0
		public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
		{
			Check.SourceAndSelector(source, selector);
			double num = 0.0;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				double? num3 = selector(tsource);
				if (num3 != null)
				{
					num += num3.Value;
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new double?(num / (double)num2);
		}

		// Token: 0x0600BD30 RID: 48432 RVA: 0x00460070 File Offset: 0x0045E270
		public static float Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
		{
			Check.SourceAndSelector(source, selector);
			float num = 0f;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				num += selector(tsource);
				num2 += 1L;
			}
			if (num2 == 0L)
			{
				throw Enumerable.EmptySequence();
			}
			return num / (float)num2;
		}

		// Token: 0x0600BD31 RID: 48433 RVA: 0x004600E0 File Offset: 0x0045E2E0
		public static float? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
		{
			Check.SourceAndSelector(source, selector);
			float num = 0f;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				float? num3 = selector(tsource);
				if (num3 != null)
				{
					num += num3.Value;
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new float?(num / (float)num2);
		}

		// Token: 0x0600BD32 RID: 48434 RVA: 0x0046016C File Offset: 0x0045E36C
		public static decimal Average<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
		{
			Check.SourceAndSelector(source, selector);
			decimal num = 0m;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				num += selector(tsource);
				num2 += 1L;
			}
			if (num2 == 0L)
			{
				throw Enumerable.EmptySequence();
			}
			return num / num2;
		}

		// Token: 0x0600BD33 RID: 48435 RVA: 0x004601E8 File Offset: 0x0045E3E8
		public static decimal? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
		{
			Check.SourceAndSelector(source, selector);
			decimal num = 0m;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				decimal? num3 = selector(tsource);
				if (num3 != null)
				{
					num += num3.Value;
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new decimal?(num / num2);
		}

		// Token: 0x0600BD34 RID: 48436 RVA: 0x00460280 File Offset: 0x0045E480
		public static IEnumerable<TResult> Cast<TResult>(this IEnumerable source)
		{
			Check.Source(source);
			IEnumerable<TResult> enumerable = source as IEnumerable<TResult>;
			if (enumerable != null)
			{
				return enumerable;
			}
			return Enumerable.CreateCastIterator<TResult>(source);
		}

		// Token: 0x0600BD35 RID: 48437 RVA: 0x004602A5 File Offset: 0x0045E4A5
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateCastIterator<TResult>(IEnumerable source)
		{
			foreach (object obj in source)
			{
				TResult tresult = (TResult)((object)obj);
				yield return tresult;
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BD36 RID: 48438 RVA: 0x004602B5 File Offset: 0x0045E4B5
		public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			Check.FirstAndSecond(first, second);
			return Enumerable.CreateConcatIterator<TSource>(first, second);
		}

		// Token: 0x0600BD37 RID: 48439 RVA: 0x004602C5 File Offset: 0x0045E4C5
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateConcatIterator<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			foreach (TSource tsource in first)
			{
				yield return tsource;
			}
			IEnumerator<TSource> enumerator = null;
			foreach (TSource tsource2 in second)
			{
				yield return tsource2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BD38 RID: 48440 RVA: 0x004602DC File Offset: 0x0045E4DC
		public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value)
		{
			ICollection<TSource> collection = source as ICollection<TSource>;
			if (collection != null)
			{
				return collection.Contains(value);
			}
			return source.Contains(value, null);
		}

		// Token: 0x0600BD39 RID: 48441 RVA: 0x00460304 File Offset: 0x0045E504
		public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
		{
			Check.Source(source);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			foreach (TSource tsource in source)
			{
				if (comparer.Equals(tsource, value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600BD3A RID: 48442 RVA: 0x00460368 File Offset: 0x0045E568
		public static int Count<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			ICollection<TSource> collection = source as ICollection<TSource>;
			if (collection != null)
			{
				return collection.Count;
			}
			int num = 0;
			checked
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x0600BD3B RID: 48443 RVA: 0x004603C0 File Offset: 0x0045E5C0
		public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndSelector(source, predicate);
			int num = 0;
			checked
			{
				foreach (TSource tsource in source)
				{
					if (predicate(tsource))
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x0600BD3C RID: 48444 RVA: 0x00460418 File Offset: 0x0045E618
		public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source)
		{
			return source.DefaultIfEmpty(default(TSource));
		}

		// Token: 0x0600BD3D RID: 48445 RVA: 0x00460434 File Offset: 0x0045E634
		public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
		{
			Check.Source(source);
			return Enumerable.CreateDefaultIfEmptyIterator<TSource>(source, defaultValue);
		}

		// Token: 0x0600BD3E RID: 48446 RVA: 0x00460443 File Offset: 0x0045E643
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateDefaultIfEmptyIterator<TSource>(IEnumerable<TSource> source, TSource defaultValue)
		{
			bool empty = true;
			foreach (TSource tsource in source)
			{
				empty = false;
				yield return tsource;
			}
			IEnumerator<TSource> enumerator = null;
			if (empty)
			{
				yield return defaultValue;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600BD3F RID: 48447 RVA: 0x0046045A File Offset: 0x0045E65A
		public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source)
		{
			return source.Distinct<TSource>(null);
		}

		// Token: 0x0600BD40 RID: 48448 RVA: 0x00460463 File Offset: 0x0045E663
		public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
		{
			Check.Source(source);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			return Enumerable.CreateDistinctIterator<TSource>(source, comparer);
		}

		// Token: 0x0600BD41 RID: 48449 RVA: 0x0046047C File Offset: 0x0045E67C
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateDistinctIterator<TSource>(IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
		{
			HashSet<TSource> items = new HashSet<TSource>(comparer);
			foreach (TSource tsource in source)
			{
				if (!items.Contains(tsource))
				{
					items.Add(tsource);
					yield return tsource;
				}
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BD42 RID: 48450 RVA: 0x00460494 File Offset: 0x0045E694
		[PublicizedFrom(EAccessModifier.Private)]
		public static TSource ElementAt<TSource>(this IEnumerable<TSource> source, int index, Enumerable.Fallback fallback)
		{
			long num = 0L;
			foreach (TSource tsource in source)
			{
				long num2 = (long)index;
				long num3 = num;
				num = num3 + 1L;
				if (num2 == num3)
				{
					return tsource;
				}
			}
			if (fallback == Enumerable.Fallback.Throw)
			{
				throw new ArgumentOutOfRangeException();
			}
			return default(TSource);
		}

		// Token: 0x0600BD43 RID: 48451 RVA: 0x004604FC File Offset: 0x0045E6FC
		public static TSource ElementAt<TSource>(this IEnumerable<TSource> source, int index)
		{
			Check.Source(source);
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			IList<TSource> list = source as IList<TSource>;
			if (list != null)
			{
				return list[index];
			}
			return source.ElementAt<TSource>(index, Enumerable.Fallback.Throw);
		}

		// Token: 0x0600BD44 RID: 48452 RVA: 0x00460534 File Offset: 0x0045E734
		public static TSource ElementAtOrDefault<TSource>(this IEnumerable<TSource> source, int index)
		{
			Check.Source(source);
			if (index < 0)
			{
				return default(TSource);
			}
			IList<TSource> list = source as IList<TSource>;
			if (list == null)
			{
				return source.ElementAt<TSource>(index, Enumerable.Fallback.Default);
			}
			if (index >= list.Count)
			{
				return default(TSource);
			}
			return list[index];
		}

		// Token: 0x0600BD45 RID: 48453 RVA: 0x00460582 File Offset: 0x0045E782
		public static IEnumerable<TResult> Empty<TResult>()
		{
			return new TResult[0];
		}

		// Token: 0x0600BD46 RID: 48454 RVA: 0x0046058A File Offset: 0x0045E78A
		public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			return first.Except<TSource>(second, null);
		}

		// Token: 0x0600BD47 RID: 48455 RVA: 0x00460594 File Offset: 0x0045E794
		public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Check.FirstAndSecond(first, second);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			return Enumerable.CreateExceptIterator<TSource>(first, second, comparer);
		}

		// Token: 0x0600BD48 RID: 48456 RVA: 0x004605AF File Offset: 0x0045E7AF
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateExceptIterator<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			HashSet<TSource> items = new HashSet<TSource>(second, comparer);
			foreach (TSource tsource in first)
			{
				if (items.Add(tsource))
				{
					yield return tsource;
				}
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BD49 RID: 48457 RVA: 0x004605D0 File Offset: 0x0045E7D0
		[PublicizedFrom(EAccessModifier.Private)]
		public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Enumerable.Fallback fallback)
		{
			foreach (TSource tsource in source)
			{
				if (predicate(tsource))
				{
					return tsource;
				}
			}
			if (fallback == Enumerable.Fallback.Throw)
			{
				throw Enumerable.NoMatchingElement();
			}
			return default(TSource);
		}

		// Token: 0x0600BD4A RID: 48458 RVA: 0x00460634 File Offset: 0x0045E834
		public static TSource First<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			IList<TSource> list = source as IList<TSource>;
			if (list != null)
			{
				if (list.Count != 0)
				{
					return list[0];
				}
			}
			else
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return enumerator.Current;
					}
				}
			}
			throw Enumerable.EmptySequence();
		}

		// Token: 0x0600BD4B RID: 48459 RVA: 0x0046069C File Offset: 0x0045E89C
		public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.First<TSource>(predicate, Enumerable.Fallback.Throw);
		}

		// Token: 0x0600BD4C RID: 48460 RVA: 0x004606B0 File Offset: 0x0045E8B0
		public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return default(TSource);
		}

		// Token: 0x0600BD4D RID: 48461 RVA: 0x00460704 File Offset: 0x0045E904
		public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.First<TSource>(predicate, Enumerable.Fallback.Default);
		}

		// Token: 0x0600BD4E RID: 48462 RVA: 0x00460715 File Offset: 0x0045E915
		public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.GroupBy<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BD4F RID: 48463 RVA: 0x0046071F File Offset: 0x0045E91F
		public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return source.CreateGroupByIterator<TSource, TKey>(keySelector, comparer);
		}

		// Token: 0x0600BD50 RID: 48464 RVA: 0x00460730 File Offset: 0x0045E930
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<IGrouping<TKey, TSource>> CreateGroupByIterator<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			Dictionary<TKey, List<TSource>> dictionary = new Dictionary<TKey, List<TSource>>(comparer);
			List<TSource> nullList = new List<TSource>();
			int counter = 0;
			int nullCounter = -1;
			foreach (TSource tsource in source)
			{
				TKey tkey = keySelector(tsource);
				if (tkey == null)
				{
					nullList.Add(tsource);
					if (nullCounter == -1)
					{
						nullCounter = counter;
						int num = counter;
						counter = num + 1;
					}
				}
				else
				{
					List<TSource> list;
					if (!dictionary.TryGetValue(tkey, out list))
					{
						list = new List<TSource>();
						dictionary.Add(tkey, list);
						int num = counter;
						counter = num + 1;
					}
					list.Add(tsource);
				}
			}
			counter = 0;
			foreach (KeyValuePair<TKey, List<TSource>> group in dictionary)
			{
				int num;
				if (counter == nullCounter)
				{
					yield return new Grouping<TKey, TSource>(default(TKey), nullList);
					num = counter;
					counter = num + 1;
				}
				yield return new Grouping<TKey, TSource>(group.Key, group.Value);
				num = counter;
				counter = num + 1;
				group = default(KeyValuePair<TKey, List<TSource>>);
			}
			Dictionary<TKey, List<TSource>>.Enumerator enumerator2 = default(Dictionary<TKey, List<TSource>>.Enumerator);
			if (counter == nullCounter)
			{
				yield return new Grouping<TKey, TSource>(default(TKey), nullList);
				int num = counter;
				counter = num + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600BD51 RID: 48465 RVA: 0x0046074E File Offset: 0x0045E94E
		public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
		{
			return source.GroupBy<TSource, TKey, TElement>(keySelector, elementSelector, null);
		}

		// Token: 0x0600BD52 RID: 48466 RVA: 0x00460759 File Offset: 0x0045E959
		public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeyElementSelectors(source, keySelector, elementSelector);
			return source.CreateGroupByIterator<TSource, TKey, TElement>(keySelector, elementSelector, comparer);
		}

		// Token: 0x0600BD53 RID: 48467 RVA: 0x0046076C File Offset: 0x0045E96C
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<IGrouping<TKey, TElement>> CreateGroupByIterator<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			Dictionary<TKey, List<TElement>> dictionary = new Dictionary<TKey, List<TElement>>(comparer);
			List<TElement> nullList = new List<TElement>();
			int counter = 0;
			int nullCounter = -1;
			foreach (TSource tsource in source)
			{
				TKey tkey = keySelector(tsource);
				TElement telement = elementSelector(tsource);
				if (tkey == null)
				{
					nullList.Add(telement);
					if (nullCounter == -1)
					{
						nullCounter = counter;
						int num = counter;
						counter = num + 1;
					}
				}
				else
				{
					List<TElement> list;
					if (!dictionary.TryGetValue(tkey, out list))
					{
						list = new List<TElement>();
						dictionary.Add(tkey, list);
						int num = counter;
						counter = num + 1;
					}
					list.Add(telement);
				}
			}
			counter = 0;
			foreach (KeyValuePair<TKey, List<TElement>> group in dictionary)
			{
				int num;
				if (counter == nullCounter)
				{
					yield return new Grouping<TKey, TElement>(default(TKey), nullList);
					num = counter;
					counter = num + 1;
				}
				yield return new Grouping<TKey, TElement>(group.Key, group.Value);
				num = counter;
				counter = num + 1;
				group = default(KeyValuePair<TKey, List<TElement>>);
			}
			Dictionary<TKey, List<TElement>>.Enumerator enumerator2 = default(Dictionary<TKey, List<TElement>>.Enumerator);
			if (counter == nullCounter)
			{
				yield return new Grouping<TKey, TElement>(default(TKey), nullList);
				int num = counter;
				counter = num + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600BD54 RID: 48468 RVA: 0x00460791 File Offset: 0x0045E991
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
		{
			return source.GroupBy<TSource, TKey, TElement, TResult>(keySelector, elementSelector, resultSelector, null);
		}

		// Token: 0x0600BD55 RID: 48469 RVA: 0x0046079D File Offset: 0x0045E99D
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Check.GroupBySelectors(source, keySelector, elementSelector, resultSelector);
			return source.CreateGroupByIterator<TSource, TKey, TElement, TResult>(keySelector, elementSelector, resultSelector, comparer);
		}

		// Token: 0x0600BD56 RID: 48470 RVA: 0x004607B3 File Offset: 0x0045E9B3
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateGroupByIterator<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			IEnumerable<IGrouping<TKey, TElement>> enumerable = source.GroupBy<TSource, TKey, TElement>(keySelector, elementSelector, comparer);
			foreach (IGrouping<TKey, TElement> grouping in enumerable)
			{
				yield return resultSelector(grouping.Key, grouping);
			}
			IEnumerator<IGrouping<TKey, TElement>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BD57 RID: 48471 RVA: 0x004607E0 File Offset: 0x0045E9E0
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
		{
			return source.GroupBy<TSource, TKey, TResult>(keySelector, resultSelector, null);
		}

		// Token: 0x0600BD58 RID: 48472 RVA: 0x004607EB File Offset: 0x0045E9EB
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeyResultSelectors(source, keySelector, resultSelector);
			return source.CreateGroupByIterator<TSource, TKey, TResult>(keySelector, resultSelector, comparer);
		}

		// Token: 0x0600BD59 RID: 48473 RVA: 0x004607FE File Offset: 0x0045E9FE
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateGroupByIterator<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			IEnumerable<IGrouping<TKey, TSource>> enumerable = source.GroupBy<TSource, TKey>(keySelector, comparer);
			foreach (IGrouping<TKey, TSource> grouping in enumerable)
			{
				yield return resultSelector(grouping.Key, grouping);
			}
			IEnumerator<IGrouping<TKey, TSource>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BD5A RID: 48474 RVA: 0x00460823 File Offset: 0x0045EA23
		public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
		{
			return outer.GroupJoin<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, null);
		}

		// Token: 0x0600BD5B RID: 48475 RVA: 0x00460831 File Offset: 0x0045EA31
		public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Check.JoinSelectors(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
			if (comparer == null)
			{
				comparer = EqualityComparer<TKey>.Default;
			}
			return outer.CreateGroupJoinIterator<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
		}

		// Token: 0x0600BD5C RID: 48476 RVA: 0x00460856 File Offset: 0x0045EA56
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateGroupJoinIterator<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			ILookup<TKey, TInner> innerKeys = inner.ToLookup<TInner, TKey>(innerKeySelector, comparer);
			foreach (TOuter touter in outer)
			{
				TKey tkey = outerKeySelector(touter);
				if (tkey != null && innerKeys.Contains(tkey))
				{
					yield return resultSelector(touter, innerKeys[tkey]);
				}
				else
				{
					yield return resultSelector(touter, Enumerable.Empty<TInner>());
				}
			}
			IEnumerator<TOuter> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BD5D RID: 48477 RVA: 0x0046088B File Offset: 0x0045EA8B
		public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			return first.Intersect<TSource>(second, null);
		}

		// Token: 0x0600BD5E RID: 48478 RVA: 0x00460895 File Offset: 0x0045EA95
		public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Check.FirstAndSecond(first, second);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			return Enumerable.CreateIntersectIterator<TSource>(first, second, comparer);
		}

		// Token: 0x0600BD5F RID: 48479 RVA: 0x004608B0 File Offset: 0x0045EAB0
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateIntersectIterator<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			HashSet<TSource> items = new HashSet<TSource>(second, comparer);
			foreach (TSource tsource in first)
			{
				if (items.Remove(tsource))
				{
					yield return tsource;
				}
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BD60 RID: 48480 RVA: 0x004608CE File Offset: 0x0045EACE
		public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Check.JoinSelectors(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
			if (comparer == null)
			{
				comparer = EqualityComparer<TKey>.Default;
			}
			return outer.CreateJoinIterator<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
		}

		// Token: 0x0600BD61 RID: 48481 RVA: 0x004608F3 File Offset: 0x0045EAF3
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateJoinIterator<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			ILookup<TKey, TInner> innerKeys = inner.ToLookup<TInner, TKey>(innerKeySelector, comparer);
			foreach (TOuter element in outer)
			{
				TKey tkey = outerKeySelector(element);
				if (tkey != null && innerKeys.Contains(tkey))
				{
					foreach (TInner tinner in innerKeys[tkey])
					{
						yield return resultSelector(element, tinner);
					}
					IEnumerator<TInner> enumerator2 = null;
				}
				element = default(TOuter);
			}
			IEnumerator<TOuter> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BD62 RID: 48482 RVA: 0x00460928 File Offset: 0x0045EB28
		public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
		{
			return outer.Join<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, null);
		}

		// Token: 0x0600BD63 RID: 48483 RVA: 0x00460938 File Offset: 0x0045EB38
		[PublicizedFrom(EAccessModifier.Private)]
		public static TSource Last<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Enumerable.Fallback fallback)
		{
			bool flag = true;
			TSource tsource = default(TSource);
			foreach (TSource tsource2 in source)
			{
				if (predicate(tsource2))
				{
					tsource = tsource2;
					flag = false;
				}
			}
			if (!flag)
			{
				return tsource;
			}
			if (fallback == Enumerable.Fallback.Throw)
			{
				throw Enumerable.NoMatchingElement();
			}
			return tsource;
		}

		// Token: 0x0600BD64 RID: 48484 RVA: 0x004609A0 File Offset: 0x0045EBA0
		public static TSource Last<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			ICollection<TSource> collection = source as ICollection<TSource>;
			if (collection != null && collection.Count == 0)
			{
				throw Enumerable.EmptySequence();
			}
			IList<TSource> list = source as IList<TSource>;
			if (list != null)
			{
				return list[list.Count - 1];
			}
			bool flag = true;
			TSource tsource = default(TSource);
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					tsource = enumerator.Current;
					flag = false;
				}
			}
			if (!flag)
			{
				return tsource;
			}
			throw Enumerable.EmptySequence();
		}

		// Token: 0x0600BD65 RID: 48485 RVA: 0x00460A34 File Offset: 0x0045EC34
		public static TSource Last<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.Last<TSource>(predicate, Enumerable.Fallback.Throw);
		}

		// Token: 0x0600BD66 RID: 48486 RVA: 0x00460A48 File Offset: 0x0045EC48
		public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			IList<TSource> list = source as IList<TSource>;
			if (list == null)
			{
				TSource tsource = default(TSource);
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						tsource = enumerator.Current;
					}
				}
				return tsource;
			}
			if (list.Count <= 0)
			{
				return default(TSource);
			}
			return list[list.Count - 1];
		}

		// Token: 0x0600BD67 RID: 48487 RVA: 0x00460AD0 File Offset: 0x0045ECD0
		public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.Last<TSource>(predicate, Enumerable.Fallback.Default);
		}

		// Token: 0x0600BD68 RID: 48488 RVA: 0x00460AE4 File Offset: 0x0045ECE4
		public static long LongCount<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			TSource[] array = source as TSource[];
			if (array != null)
			{
				return (long)array.Length;
			}
			long num = 0L;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					num += 1L;
				}
			}
			return num;
		}

		// Token: 0x0600BD69 RID: 48489 RVA: 0x00460B3C File Offset: 0x0045ED3C
		public static long LongCount<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndSelector(source, predicate);
			long num = 0L;
			foreach (TSource tsource in source)
			{
				if (predicate(tsource))
				{
					num += 1L;
				}
			}
			return num;
		}

		// Token: 0x0600BD6A RID: 48490 RVA: 0x00460B98 File Offset: 0x0045ED98
		public static int Max(this IEnumerable<int> source)
		{
			Check.Source(source);
			bool flag = true;
			int num = int.MinValue;
			foreach (int num2 in source)
			{
				num = Math.Max(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BD6B RID: 48491 RVA: 0x00460BFC File Offset: 0x0045EDFC
		public static long Max(this IEnumerable<long> source)
		{
			Check.Source(source);
			bool flag = true;
			long num = long.MinValue;
			foreach (long num2 in source)
			{
				num = Math.Max(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BD6C RID: 48492 RVA: 0x00460C64 File Offset: 0x0045EE64
		public static double Max(this IEnumerable<double> source)
		{
			Check.Source(source);
			bool flag = true;
			double num = double.MinValue;
			foreach (double num2 in source)
			{
				num = Math.Max(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BD6D RID: 48493 RVA: 0x00460CCC File Offset: 0x0045EECC
		public static float Max(this IEnumerable<float> source)
		{
			Check.Source(source);
			bool flag = true;
			float num = float.MinValue;
			foreach (float num2 in source)
			{
				num = Math.Max(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BD6E RID: 48494 RVA: 0x00460D30 File Offset: 0x0045EF30
		public static decimal Max(this IEnumerable<decimal> source)
		{
			Check.Source(source);
			bool flag = true;
			decimal num = decimal.MinValue;
			foreach (decimal num2 in source)
			{
				num = Math.Max(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BD6F RID: 48495 RVA: 0x00460D98 File Offset: 0x0045EF98
		public static int? Max(this IEnumerable<int?> source)
		{
			Check.Source(source);
			bool flag = true;
			int num = int.MinValue;
			foreach (int? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Max(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new int?(num);
		}

		// Token: 0x0600BD70 RID: 48496 RVA: 0x00460E14 File Offset: 0x0045F014
		public static long? Max(this IEnumerable<long?> source)
		{
			Check.Source(source);
			bool flag = true;
			long num = long.MinValue;
			foreach (long? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Max(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new long?(num);
		}

		// Token: 0x0600BD71 RID: 48497 RVA: 0x00460E94 File Offset: 0x0045F094
		public static double? Max(this IEnumerable<double?> source)
		{
			Check.Source(source);
			bool flag = true;
			double num = double.MinValue;
			foreach (double? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Max(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new double?(num);
		}

		// Token: 0x0600BD72 RID: 48498 RVA: 0x00460F14 File Offset: 0x0045F114
		public static float? Max(this IEnumerable<float?> source)
		{
			Check.Source(source);
			bool flag = true;
			float num = float.MinValue;
			foreach (float? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Max(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new float?(num);
		}

		// Token: 0x0600BD73 RID: 48499 RVA: 0x00460F90 File Offset: 0x0045F190
		public static decimal? Max(this IEnumerable<decimal?> source)
		{
			Check.Source(source);
			bool flag = true;
			decimal num = decimal.MinValue;
			foreach (decimal? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Max(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new decimal?(num);
		}

		// Token: 0x0600BD74 RID: 48500 RVA: 0x00461014 File Offset: 0x0045F214
		public static TSource Max<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			Comparer<TSource> @default = Comparer<TSource>.Default;
			TSource tsource = default(TSource);
			if (default(TSource) == null)
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						if (tsource2 != null && (tsource == null || @default.Compare(tsource2, tsource) > 0))
						{
							tsource = tsource2;
						}
					}
					return tsource;
				}
			}
			bool flag = true;
			foreach (TSource tsource3 in source)
			{
				if (flag)
				{
					tsource = tsource3;
					flag = false;
				}
				else if (@default.Compare(tsource3, tsource) > 0)
				{
					tsource = tsource3;
				}
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return tsource;
		}

		// Token: 0x0600BD75 RID: 48501 RVA: 0x004610F4 File Offset: 0x0045F2F4
		public static int Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			int num = int.MinValue;
			foreach (TSource tsource in source)
			{
				num = Math.Max(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BD76 RID: 48502 RVA: 0x00461160 File Offset: 0x0045F360
		public static long Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			long num = long.MinValue;
			foreach (TSource tsource in source)
			{
				num = Math.Max(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BD77 RID: 48503 RVA: 0x004611D0 File Offset: 0x0045F3D0
		public static double Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			double num = double.MinValue;
			foreach (TSource tsource in source)
			{
				num = Math.Max(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BD78 RID: 48504 RVA: 0x00461240 File Offset: 0x0045F440
		public static float Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			float num = float.MinValue;
			foreach (TSource tsource in source)
			{
				num = Math.Max(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BD79 RID: 48505 RVA: 0x004612AC File Offset: 0x0045F4AC
		public static decimal Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			decimal num = decimal.MinValue;
			foreach (TSource tsource in source)
			{
				num = Math.Max(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BD7A RID: 48506 RVA: 0x0046131C File Offset: 0x0045F51C
		[PublicizedFrom(EAccessModifier.Private)]
		public static U Iterate<T, U>(IEnumerable<T> source, U initValue, Func<T, U, U> selector)
		{
			bool flag = true;
			foreach (T t in source)
			{
				initValue = selector(t, initValue);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return initValue;
		}

		// Token: 0x0600BD7B RID: 48507 RVA: 0x00461378 File Offset: 0x0045F578
		public static int? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			int? num = null;
			foreach (TSource tsource in source)
			{
				int? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					int? num3 = num2;
					int? num4 = num;
					if ((num3.GetValueOrDefault() > num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BD7C RID: 48508 RVA: 0x00461420 File Offset: 0x0045F620
		public static long? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			long? num = null;
			foreach (TSource tsource in source)
			{
				long? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					long? num3 = num2;
					long? num4 = num;
					if ((num3.GetValueOrDefault() > num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BD7D RID: 48509 RVA: 0x004614C8 File Offset: 0x0045F6C8
		public static double? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			double? num = null;
			foreach (TSource tsource in source)
			{
				double? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					double? num3 = num2;
					double? num4 = num;
					if ((num3.GetValueOrDefault() > num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BD7E RID: 48510 RVA: 0x00461570 File Offset: 0x0045F770
		public static float? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			float? num = null;
			foreach (TSource tsource in source)
			{
				float? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					float? num3 = num2;
					float? num4 = num;
					if ((num3.GetValueOrDefault() > num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BD7F RID: 48511 RVA: 0x00461618 File Offset: 0x0045F818
		public static decimal? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			decimal? num = null;
			foreach (TSource tsource in source)
			{
				decimal? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					decimal? num3 = num2;
					decimal? num4 = num;
					if ((num3.GetValueOrDefault() > num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BD80 RID: 48512 RVA: 0x004616C4 File Offset: 0x0045F8C4
		public static TResult Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			Check.SourceAndSelector(source, selector);
			return source.Select<TSource, TResult>(selector).Max<TResult>();
		}

		// Token: 0x0600BD81 RID: 48513 RVA: 0x004616DC File Offset: 0x0045F8DC
		public static int Min(this IEnumerable<int> source)
		{
			Check.Source(source);
			bool flag = true;
			int num = int.MaxValue;
			foreach (int num2 in source)
			{
				num = Math.Min(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BD82 RID: 48514 RVA: 0x00461740 File Offset: 0x0045F940
		public static long Min(this IEnumerable<long> source)
		{
			Check.Source(source);
			bool flag = true;
			long num = long.MaxValue;
			foreach (long num2 in source)
			{
				num = Math.Min(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BD83 RID: 48515 RVA: 0x004617A8 File Offset: 0x0045F9A8
		public static double Min(this IEnumerable<double> source)
		{
			Check.Source(source);
			bool flag = true;
			double num = double.MaxValue;
			foreach (double num2 in source)
			{
				num = Math.Min(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BD84 RID: 48516 RVA: 0x00461810 File Offset: 0x0045FA10
		public static float Min(this IEnumerable<float> source)
		{
			Check.Source(source);
			bool flag = true;
			float num = float.MaxValue;
			foreach (float num2 in source)
			{
				num = Math.Min(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BD85 RID: 48517 RVA: 0x00461874 File Offset: 0x0045FA74
		public static decimal Min(this IEnumerable<decimal> source)
		{
			Check.Source(source);
			bool flag = true;
			decimal num = decimal.MaxValue;
			foreach (decimal num2 in source)
			{
				num = Math.Min(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BD86 RID: 48518 RVA: 0x004618DC File Offset: 0x0045FADC
		public static int? Min(this IEnumerable<int?> source)
		{
			Check.Source(source);
			bool flag = true;
			int num = int.MaxValue;
			foreach (int? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Min(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new int?(num);
		}

		// Token: 0x0600BD87 RID: 48519 RVA: 0x00461958 File Offset: 0x0045FB58
		public static long? Min(this IEnumerable<long?> source)
		{
			Check.Source(source);
			bool flag = true;
			long num = long.MaxValue;
			foreach (long? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Min(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new long?(num);
		}

		// Token: 0x0600BD88 RID: 48520 RVA: 0x004619D8 File Offset: 0x0045FBD8
		public static double? Min(this IEnumerable<double?> source)
		{
			Check.Source(source);
			bool flag = true;
			double num = double.MaxValue;
			foreach (double? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Min(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new double?(num);
		}

		// Token: 0x0600BD89 RID: 48521 RVA: 0x00461A58 File Offset: 0x0045FC58
		public static float? Min(this IEnumerable<float?> source)
		{
			Check.Source(source);
			bool flag = true;
			float num = float.MaxValue;
			foreach (float? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Min(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new float?(num);
		}

		// Token: 0x0600BD8A RID: 48522 RVA: 0x00461AD4 File Offset: 0x0045FCD4
		public static decimal? Min(this IEnumerable<decimal?> source)
		{
			Check.Source(source);
			bool flag = true;
			decimal num = decimal.MaxValue;
			foreach (decimal? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Min(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new decimal?(num);
		}

		// Token: 0x0600BD8B RID: 48523 RVA: 0x00461B58 File Offset: 0x0045FD58
		public static TSource Min<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			Comparer<TSource> @default = Comparer<TSource>.Default;
			TSource tsource = default(TSource);
			if (default(TSource) == null)
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						if (tsource2 != null && (tsource == null || @default.Compare(tsource2, tsource) < 0))
						{
							tsource = tsource2;
						}
					}
					return tsource;
				}
			}
			bool flag = true;
			foreach (TSource tsource3 in source)
			{
				if (flag)
				{
					tsource = tsource3;
					flag = false;
				}
				else if (@default.Compare(tsource3, tsource) < 0)
				{
					tsource = tsource3;
				}
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return tsource;
		}

		// Token: 0x0600BD8C RID: 48524 RVA: 0x00461C38 File Offset: 0x0045FE38
		public static int Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			int num = int.MaxValue;
			foreach (TSource tsource in source)
			{
				num = Math.Min(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BD8D RID: 48525 RVA: 0x00461CA4 File Offset: 0x0045FEA4
		public static long Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			long num = long.MaxValue;
			foreach (TSource tsource in source)
			{
				num = Math.Min(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BD8E RID: 48526 RVA: 0x00461D14 File Offset: 0x0045FF14
		public static double Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			double num = double.MaxValue;
			foreach (TSource tsource in source)
			{
				num = Math.Min(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BD8F RID: 48527 RVA: 0x00461D84 File Offset: 0x0045FF84
		public static float Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			float num = float.MaxValue;
			foreach (TSource tsource in source)
			{
				num = Math.Min(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BD90 RID: 48528 RVA: 0x00461DF0 File Offset: 0x0045FFF0
		public static decimal Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			decimal num = decimal.MaxValue;
			foreach (TSource tsource in source)
			{
				num = Math.Min(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BD91 RID: 48529 RVA: 0x00461E60 File Offset: 0x00460060
		public static int? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			int? num = null;
			foreach (TSource tsource in source)
			{
				int? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					int? num3 = num2;
					int? num4 = num;
					if ((num3.GetValueOrDefault() < num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BD92 RID: 48530 RVA: 0x00461F08 File Offset: 0x00460108
		public static long? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			long? num = null;
			foreach (TSource tsource in source)
			{
				long? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					long? num3 = num2;
					long? num4 = num;
					if ((num3.GetValueOrDefault() < num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BD93 RID: 48531 RVA: 0x00461FB0 File Offset: 0x004601B0
		public static float? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			float? num = null;
			foreach (TSource tsource in source)
			{
				float? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					float? num3 = num2;
					float? num4 = num;
					if ((num3.GetValueOrDefault() < num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BD94 RID: 48532 RVA: 0x00462058 File Offset: 0x00460258
		public static double? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			double? num = null;
			foreach (TSource tsource in source)
			{
				double? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					double? num3 = num2;
					double? num4 = num;
					if ((num3.GetValueOrDefault() < num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BD95 RID: 48533 RVA: 0x00462100 File Offset: 0x00460300
		public static decimal? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			decimal? num = null;
			foreach (TSource tsource in source)
			{
				decimal? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					decimal? num3 = num2;
					decimal? num4 = num;
					if ((num3.GetValueOrDefault() < num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BD96 RID: 48534 RVA: 0x004621AC File Offset: 0x004603AC
		public static TResult Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			Check.SourceAndSelector(source, selector);
			return source.Select<TSource, TResult>(selector).Min<TResult>();
		}

		// Token: 0x0600BD97 RID: 48535 RVA: 0x004621C1 File Offset: 0x004603C1
		public static IEnumerable<TResult> OfType<TResult>(this IEnumerable source)
		{
			Check.Source(source);
			return Enumerable.CreateOfTypeIterator<TResult>(source);
		}

		// Token: 0x0600BD98 RID: 48536 RVA: 0x004621CF File Offset: 0x004603CF
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateOfTypeIterator<TResult>(IEnumerable source)
		{
			foreach (object obj in source)
			{
				if (obj is TResult)
				{
					yield return (TResult)((object)obj);
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BD99 RID: 48537 RVA: 0x004621DF File Offset: 0x004603DF
		public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.OrderBy<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BD9A RID: 48538 RVA: 0x004621E9 File Offset: 0x004603E9
		public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return new OrderedSequence<TSource, TKey>(source, keySelector, comparer, SortDirection.Ascending);
		}

		// Token: 0x0600BD9B RID: 48539 RVA: 0x004621FB File Offset: 0x004603FB
		public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.OrderByDescending<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BD9C RID: 48540 RVA: 0x00462205 File Offset: 0x00460405
		public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return new OrderedSequence<TSource, TKey>(source, keySelector, comparer, SortDirection.Descending);
		}

		// Token: 0x0600BD9D RID: 48541 RVA: 0x00462217 File Offset: 0x00460417
		public static IEnumerable<int> Range(int start, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if ((long)start + (long)count - 1L > 2147483647L)
			{
				throw new ArgumentOutOfRangeException();
			}
			return Enumerable.CreateRangeIterator(start, count);
		}

		// Token: 0x0600BD9E RID: 48542 RVA: 0x00462245 File Offset: 0x00460445
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<int> CreateRangeIterator(int start, int count)
		{
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				yield return start + i;
				num = i;
			}
			yield break;
		}

		// Token: 0x0600BD9F RID: 48543 RVA: 0x0046225C File Offset: 0x0046045C
		public static IEnumerable<TResult> Repeat<TResult>(TResult element, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return Enumerable.CreateRepeatIterator<TResult>(element, count);
		}

		// Token: 0x0600BDA0 RID: 48544 RVA: 0x0046226F File Offset: 0x0046046F
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateRepeatIterator<TResult>(TResult element, int count)
		{
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				yield return element;
				num = i;
			}
			yield break;
		}

		// Token: 0x0600BDA1 RID: 48545 RVA: 0x00462286 File Offset: 0x00460486
		public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			return Enumerable.CreateReverseIterator<TSource>(source);
		}

		// Token: 0x0600BDA2 RID: 48546 RVA: 0x00462294 File Offset: 0x00460494
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateReverseIterator<TSource>(IEnumerable<TSource> source)
		{
			TSource[] array = source.ToArray<TSource>();
			int num;
			for (int i = array.Length - 1; i >= 0; i = num - 1)
			{
				yield return array[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x0600BDA3 RID: 48547 RVA: 0x004622A4 File Offset: 0x004604A4
		public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			Check.SourceAndSelector(source, selector);
			return Enumerable.CreateSelectIterator<TSource, TResult>(source, selector);
		}

		// Token: 0x0600BDA4 RID: 48548 RVA: 0x004622B4 File Offset: 0x004604B4
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateSelectIterator<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			foreach (TSource tsource in source)
			{
				yield return selector(tsource);
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BDA5 RID: 48549 RVA: 0x004622CB File Offset: 0x004604CB
		public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
		{
			Check.SourceAndSelector(source, selector);
			return Enumerable.CreateSelectIterator<TSource, TResult>(source, selector);
		}

		// Token: 0x0600BDA6 RID: 48550 RVA: 0x004622DB File Offset: 0x004604DB
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateSelectIterator<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
		{
			int counter = 0;
			foreach (TSource tsource in source)
			{
				yield return selector(tsource, counter);
				int num = counter;
				counter = num + 1;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BDA7 RID: 48551 RVA: 0x004622F2 File Offset: 0x004604F2
		public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
		{
			Check.SourceAndSelector(source, selector);
			return Enumerable.CreateSelectManyIterator<TSource, TResult>(source, selector);
		}

		// Token: 0x0600BDA8 RID: 48552 RVA: 0x00462302 File Offset: 0x00460502
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateSelectManyIterator<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
		{
			foreach (TSource tsource in source)
			{
				foreach (TResult tresult in selector(tsource))
				{
					yield return tresult;
				}
				IEnumerator<TResult> enumerator2 = null;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BDA9 RID: 48553 RVA: 0x00462319 File Offset: 0x00460519
		public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
		{
			Check.SourceAndSelector(source, selector);
			return Enumerable.CreateSelectManyIterator<TSource, TResult>(source, selector);
		}

		// Token: 0x0600BDAA RID: 48554 RVA: 0x00462329 File Offset: 0x00460529
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateSelectManyIterator<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
		{
			int counter = 0;
			foreach (TSource tsource in source)
			{
				foreach (TResult tresult in selector(tsource, counter))
				{
					yield return tresult;
				}
				IEnumerator<TResult> enumerator2 = null;
				int num = counter;
				counter = num + 1;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BDAB RID: 48555 RVA: 0x00462340 File Offset: 0x00460540
		public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
		{
			Check.SourceAndCollectionSelectors(source, collectionSelector, resultSelector);
			return Enumerable.CreateSelectManyIterator<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		// Token: 0x0600BDAC RID: 48556 RVA: 0x00462352 File Offset: 0x00460552
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateSelectManyIterator<TSource, TCollection, TResult>(IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> selector)
		{
			foreach (TSource element in source)
			{
				foreach (TCollection tcollection in collectionSelector(element))
				{
					yield return selector(element, tcollection);
				}
				IEnumerator<TCollection> enumerator2 = null;
				element = default(TSource);
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BDAD RID: 48557 RVA: 0x00462370 File Offset: 0x00460570
		public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
		{
			Check.SourceAndCollectionSelectors(source, collectionSelector, resultSelector);
			return Enumerable.CreateSelectManyIterator<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		// Token: 0x0600BDAE RID: 48558 RVA: 0x00462382 File Offset: 0x00460582
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateSelectManyIterator<TSource, TCollection, TResult>(IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> selector)
		{
			int counter = 0;
			foreach (TSource element in source)
			{
				TSource tsource = element;
				int num = counter;
				counter = num + 1;
				foreach (TCollection tcollection in collectionSelector(tsource, num))
				{
					yield return selector(element, tcollection);
				}
				IEnumerator<TCollection> enumerator2 = null;
				element = default(TSource);
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BDAF RID: 48559 RVA: 0x004623A0 File Offset: 0x004605A0
		[PublicizedFrom(EAccessModifier.Private)]
		public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Enumerable.Fallback fallback)
		{
			bool flag = false;
			TSource tsource = default(TSource);
			foreach (TSource tsource2 in source)
			{
				if (predicate(tsource2))
				{
					if (flag)
					{
						throw Enumerable.MoreThanOneMatchingElement();
					}
					flag = true;
					tsource = tsource2;
				}
			}
			if (!flag && fallback == Enumerable.Fallback.Throw)
			{
				throw Enumerable.NoMatchingElement();
			}
			return tsource;
		}

		// Token: 0x0600BDB0 RID: 48560 RVA: 0x00462410 File Offset: 0x00460610
		public static TSource Single<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			bool flag = false;
			TSource tsource = default(TSource);
			foreach (TSource tsource2 in source)
			{
				if (flag)
				{
					throw Enumerable.MoreThanOneElement();
				}
				flag = true;
				tsource = tsource2;
			}
			if (!flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return tsource;
		}

		// Token: 0x0600BDB1 RID: 48561 RVA: 0x00462478 File Offset: 0x00460678
		public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.Single<TSource>(predicate, Enumerable.Fallback.Throw);
		}

		// Token: 0x0600BDB2 RID: 48562 RVA: 0x0046248C File Offset: 0x0046068C
		public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			bool flag = false;
			TSource tsource = default(TSource);
			foreach (TSource tsource2 in source)
			{
				if (flag)
				{
					throw Enumerable.MoreThanOneMatchingElement();
				}
				flag = true;
				tsource = tsource2;
			}
			return tsource;
		}

		// Token: 0x0600BDB3 RID: 48563 RVA: 0x004624EC File Offset: 0x004606EC
		public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.Single<TSource>(predicate, Enumerable.Fallback.Default);
		}

		// Token: 0x0600BDB4 RID: 48564 RVA: 0x004624FD File Offset: 0x004606FD
		public static IEnumerable<TSource> Skip<TSource>(this IEnumerable<TSource> source, int count)
		{
			Check.Source(source);
			return Enumerable.CreateSkipIterator<TSource>(source, count);
		}

		// Token: 0x0600BDB5 RID: 48565 RVA: 0x0046250C File Offset: 0x0046070C
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateSkipIterator<TSource>(IEnumerable<TSource> source, int count)
		{
			IEnumerator<TSource> enumerator = source.GetEnumerator();
			try
			{
				do
				{
					int num = count;
					count = num - 1;
					if (num <= 0)
					{
						goto Block_5;
					}
				}
				while (enumerator.MoveNext());
				yield break;
				Block_5:
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					yield return tsource;
				}
			}
			finally
			{
				enumerator.Dispose();
			}
			yield break;
			yield break;
		}

		// Token: 0x0600BDB6 RID: 48566 RVA: 0x00462523 File Offset: 0x00460723
		public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return Enumerable.CreateSkipWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x0600BDB7 RID: 48567 RVA: 0x00462533 File Offset: 0x00460733
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateSkipWhileIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			bool yield = false;
			foreach (TSource tsource in source)
			{
				if (yield)
				{
					yield return tsource;
				}
				else if (!predicate(tsource))
				{
					yield return tsource;
					yield = true;
				}
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BDB8 RID: 48568 RVA: 0x0046254A File Offset: 0x0046074A
		public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return Enumerable.CreateSkipWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x0600BDB9 RID: 48569 RVA: 0x0046255A File Offset: 0x0046075A
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateSkipWhileIterator<TSource>(IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			int counter = 0;
			bool yield = false;
			foreach (TSource tsource in source)
			{
				if (yield)
				{
					yield return tsource;
				}
				else if (!predicate(tsource, counter))
				{
					yield return tsource;
					yield = true;
				}
				int num = counter;
				counter = num + 1;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BDBA RID: 48570 RVA: 0x00462574 File Offset: 0x00460774
		public static int Sum(this IEnumerable<int> source)
		{
			Check.Source(source);
			int num = 0;
			checked
			{
				foreach (int num2 in source)
				{
					num += num2;
				}
				return num;
			}
		}

		// Token: 0x0600BDBB RID: 48571 RVA: 0x004625C4 File Offset: 0x004607C4
		public static int? Sum(this IEnumerable<int?> source)
		{
			Check.Source(source);
			int num = 0;
			checked
			{
				foreach (int? num2 in source)
				{
					if (num2 != null)
					{
						num += num2.Value;
					}
				}
				return new int?(num);
			}
		}

		// Token: 0x0600BDBC RID: 48572 RVA: 0x00462628 File Offset: 0x00460828
		public static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
		{
			Check.SourceAndSelector(source, selector);
			int num = 0;
			checked
			{
				foreach (TSource tsource in source)
				{
					num += selector(tsource);
				}
				return num;
			}
		}

		// Token: 0x0600BDBD RID: 48573 RVA: 0x00462680 File Offset: 0x00460880
		public static int? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
		{
			Check.SourceAndSelector(source, selector);
			int num = 0;
			checked
			{
				foreach (TSource tsource in source)
				{
					int? num2 = selector(tsource);
					if (num2 != null)
					{
						num += num2.Value;
					}
				}
				return new int?(num);
			}
		}

		// Token: 0x0600BDBE RID: 48574 RVA: 0x004626EC File Offset: 0x004608EC
		public static long Sum(this IEnumerable<long> source)
		{
			Check.Source(source);
			long num = 0L;
			checked
			{
				foreach (long num2 in source)
				{
					num += num2;
				}
				return num;
			}
		}

		// Token: 0x0600BDBF RID: 48575 RVA: 0x0046273C File Offset: 0x0046093C
		public static long? Sum(this IEnumerable<long?> source)
		{
			Check.Source(source);
			long num = 0L;
			checked
			{
				foreach (long? num2 in source)
				{
					if (num2 != null)
					{
						num += num2.Value;
					}
				}
				return new long?(num);
			}
		}

		// Token: 0x0600BDC0 RID: 48576 RVA: 0x004627A0 File Offset: 0x004609A0
		public static long Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
		{
			Check.SourceAndSelector(source, selector);
			long num = 0L;
			checked
			{
				foreach (TSource tsource in source)
				{
					num += selector(tsource);
				}
				return num;
			}
		}

		// Token: 0x0600BDC1 RID: 48577 RVA: 0x004627F8 File Offset: 0x004609F8
		public static long? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
		{
			Check.SourceAndSelector(source, selector);
			long num = 0L;
			checked
			{
				foreach (TSource tsource in source)
				{
					long? num2 = selector(tsource);
					if (num2 != null)
					{
						num += num2.Value;
					}
				}
				return new long?(num);
			}
		}

		// Token: 0x0600BDC2 RID: 48578 RVA: 0x00462864 File Offset: 0x00460A64
		public static double Sum(this IEnumerable<double> source)
		{
			Check.Source(source);
			double num = 0.0;
			foreach (double num2 in source)
			{
				num += num2;
			}
			return num;
		}

		// Token: 0x0600BDC3 RID: 48579 RVA: 0x004628BC File Offset: 0x00460ABC
		public static double? Sum(this IEnumerable<double?> source)
		{
			Check.Source(source);
			double num = 0.0;
			foreach (double? num2 in source)
			{
				if (num2 != null)
				{
					num += num2.Value;
				}
			}
			return new double?(num);
		}

		// Token: 0x0600BDC4 RID: 48580 RVA: 0x00462928 File Offset: 0x00460B28
		public static double Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
		{
			Check.SourceAndSelector(source, selector);
			double num = 0.0;
			foreach (TSource tsource in source)
			{
				num += selector(tsource);
			}
			return num;
		}

		// Token: 0x0600BDC5 RID: 48581 RVA: 0x00462988 File Offset: 0x00460B88
		public static double? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
		{
			Check.SourceAndSelector(source, selector);
			double num = 0.0;
			foreach (TSource tsource in source)
			{
				double? num2 = selector(tsource);
				if (num2 != null)
				{
					num += num2.Value;
				}
			}
			return new double?(num);
		}

		// Token: 0x0600BDC6 RID: 48582 RVA: 0x004629FC File Offset: 0x00460BFC
		public static float Sum(this IEnumerable<float> source)
		{
			Check.Source(source);
			float num = 0f;
			foreach (float num2 in source)
			{
				num += num2;
			}
			return num;
		}

		// Token: 0x0600BDC7 RID: 48583 RVA: 0x00462A50 File Offset: 0x00460C50
		public static float? Sum(this IEnumerable<float?> source)
		{
			Check.Source(source);
			float num = 0f;
			foreach (float? num2 in source)
			{
				if (num2 != null)
				{
					num += num2.Value;
				}
			}
			return new float?(num);
		}

		// Token: 0x0600BDC8 RID: 48584 RVA: 0x00462AB8 File Offset: 0x00460CB8
		public static float Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
		{
			Check.SourceAndSelector(source, selector);
			float num = 0f;
			foreach (TSource tsource in source)
			{
				num += selector(tsource);
			}
			return num;
		}

		// Token: 0x0600BDC9 RID: 48585 RVA: 0x00462B14 File Offset: 0x00460D14
		public static float? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
		{
			Check.SourceAndSelector(source, selector);
			float num = 0f;
			foreach (TSource tsource in source)
			{
				float? num2 = selector(tsource);
				if (num2 != null)
				{
					num += num2.Value;
				}
			}
			return new float?(num);
		}

		// Token: 0x0600BDCA RID: 48586 RVA: 0x00462B84 File Offset: 0x00460D84
		public static decimal Sum(this IEnumerable<decimal> source)
		{
			Check.Source(source);
			decimal num = 0m;
			foreach (decimal num2 in source)
			{
				num += num2;
			}
			return num;
		}

		// Token: 0x0600BDCB RID: 48587 RVA: 0x00462BDC File Offset: 0x00460DDC
		public static decimal? Sum(this IEnumerable<decimal?> source)
		{
			Check.Source(source);
			decimal num = 0m;
			foreach (decimal? num2 in source)
			{
				if (num2 != null)
				{
					num += num2.Value;
				}
			}
			return new decimal?(num);
		}

		// Token: 0x0600BDCC RID: 48588 RVA: 0x00462C48 File Offset: 0x00460E48
		public static decimal Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
		{
			Check.SourceAndSelector(source, selector);
			decimal num = 0m;
			foreach (TSource tsource in source)
			{
				num += selector(tsource);
			}
			return num;
		}

		// Token: 0x0600BDCD RID: 48589 RVA: 0x00462CA8 File Offset: 0x00460EA8
		public static decimal? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
		{
			Check.SourceAndSelector(source, selector);
			decimal num = 0m;
			foreach (TSource tsource in source)
			{
				decimal? num2 = selector(tsource);
				if (num2 != null)
				{
					num += num2.Value;
				}
			}
			return new decimal?(num);
		}

		// Token: 0x0600BDCE RID: 48590 RVA: 0x00462D20 File Offset: 0x00460F20
		public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count)
		{
			Check.Source(source);
			return Enumerable.CreateTakeIterator<TSource>(source, count);
		}

		// Token: 0x0600BDCF RID: 48591 RVA: 0x00462D2F File Offset: 0x00460F2F
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateTakeIterator<TSource>(IEnumerable<TSource> source, int count)
		{
			if (count <= 0)
			{
				yield break;
			}
			int counter = 0;
			foreach (TSource tsource in source)
			{
				yield return tsource;
				int num = counter + 1;
				counter = num;
				if (num == count)
				{
					yield break;
				}
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BDD0 RID: 48592 RVA: 0x00462D46 File Offset: 0x00460F46
		public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return Enumerable.CreateTakeWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x0600BDD1 RID: 48593 RVA: 0x00462D56 File Offset: 0x00460F56
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateTakeWhileIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			foreach (TSource tsource in source)
			{
				if (!predicate(tsource))
				{
					yield break;
				}
				yield return tsource;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BDD2 RID: 48594 RVA: 0x00462D6D File Offset: 0x00460F6D
		public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return Enumerable.CreateTakeWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x0600BDD3 RID: 48595 RVA: 0x00462D7D File Offset: 0x00460F7D
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateTakeWhileIterator<TSource>(IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			int counter = 0;
			foreach (TSource tsource in source)
			{
				if (!predicate(tsource, counter))
				{
					yield break;
				}
				yield return tsource;
				int num = counter;
				counter = num + 1;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BDD4 RID: 48596 RVA: 0x00462D94 File Offset: 0x00460F94
		public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ThenBy<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BDD5 RID: 48597 RVA: 0x00462D9E File Offset: 0x00460F9E
		public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return (source as OrderedEnumerable<TSource>).CreateOrderedEnumerable<TKey>(keySelector, comparer, false);
		}

		// Token: 0x0600BDD6 RID: 48598 RVA: 0x00462DB5 File Offset: 0x00460FB5
		public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ThenByDescending<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BDD7 RID: 48599 RVA: 0x00462DBF File Offset: 0x00460FBF
		public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return (source as OrderedEnumerable<TSource>).CreateOrderedEnumerable<TKey>(keySelector, comparer, true);
		}

		// Token: 0x0600BDD8 RID: 48600 RVA: 0x00462DD8 File Offset: 0x00460FD8
		public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			ICollection<TSource> collection = source as ICollection<TSource>;
			TSource[] array;
			if (collection == null)
			{
				int num = 0;
				array = new TSource[0];
				foreach (TSource tsource in source)
				{
					if (num == array.Length)
					{
						if (num == 0)
						{
							array = new TSource[4];
						}
						else
						{
							Array.Resize<TSource>(ref array, num * 2);
						}
					}
					array[num++] = tsource;
				}
				if (num != array.Length)
				{
					Array.Resize<TSource>(ref array, num);
				}
				return array;
			}
			if (collection.Count == 0)
			{
				return new TSource[0];
			}
			array = new TSource[collection.Count];
			collection.CopyTo(array, 0);
			return array;
		}

		// Token: 0x0600BDD9 RID: 48601 RVA: 0x00462E90 File Offset: 0x00461090
		public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
		{
			return source.ToDictionary<TSource, TKey, TElement>(keySelector, elementSelector, null);
		}

		// Token: 0x0600BDDA RID: 48602 RVA: 0x00462E9C File Offset: 0x0046109C
		public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeyElementSelectors(source, keySelector, elementSelector);
			if (comparer == null)
			{
				comparer = EqualityComparer<TKey>.Default;
			}
			Dictionary<TKey, TElement> dictionary = new Dictionary<TKey, TElement>(comparer);
			foreach (TSource tsource in source)
			{
				dictionary.Add(keySelector(tsource), elementSelector(tsource));
			}
			return dictionary;
		}

		// Token: 0x0600BDDB RID: 48603 RVA: 0x00462F0C File Offset: 0x0046110C
		public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ToDictionary<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BDDC RID: 48604 RVA: 0x00462F18 File Offset: 0x00461118
		public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			if (comparer == null)
			{
				comparer = EqualityComparer<TKey>.Default;
			}
			Dictionary<TKey, TSource> dictionary = new Dictionary<TKey, TSource>(comparer);
			foreach (TSource tsource in source)
			{
				dictionary.Add(keySelector(tsource), tsource);
			}
			return dictionary;
		}

		// Token: 0x0600BDDD RID: 48605 RVA: 0x00462F80 File Offset: 0x00461180
		public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			return new List<TSource>(source);
		}

		// Token: 0x0600BDDE RID: 48606 RVA: 0x00462F8E File Offset: 0x0046118E
		public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ToLookup<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BDDF RID: 48607 RVA: 0x00462F98 File Offset: 0x00461198
		public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			List<TSource> list = null;
			Dictionary<TKey, List<TSource>> dictionary = new Dictionary<TKey, List<TSource>>(comparer ?? EqualityComparer<TKey>.Default);
			foreach (TSource tsource in source)
			{
				TKey tkey = keySelector(tsource);
				List<TSource> list2;
				if (tkey == null)
				{
					if (list == null)
					{
						list = new List<TSource>();
					}
					list2 = list;
				}
				else if (!dictionary.TryGetValue(tkey, out list2))
				{
					list2 = new List<TSource>();
					dictionary.Add(tkey, list2);
				}
				list2.Add(tsource);
			}
			return new Lookup<TKey, TSource>(dictionary, list);
		}

		// Token: 0x0600BDE0 RID: 48608 RVA: 0x00463040 File Offset: 0x00461240
		public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
		{
			return source.ToLookup<TSource, TKey, TElement>(keySelector, elementSelector, null);
		}

		// Token: 0x0600BDE1 RID: 48609 RVA: 0x0046304C File Offset: 0x0046124C
		public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeyElementSelectors(source, keySelector, elementSelector);
			List<TElement> list = null;
			Dictionary<TKey, List<TElement>> dictionary = new Dictionary<TKey, List<TElement>>(comparer ?? EqualityComparer<TKey>.Default);
			foreach (TSource tsource in source)
			{
				TKey tkey = keySelector(tsource);
				List<TElement> list2;
				if (tkey == null)
				{
					if (list == null)
					{
						list = new List<TElement>();
					}
					list2 = list;
				}
				else if (!dictionary.TryGetValue(tkey, out list2))
				{
					list2 = new List<TElement>();
					dictionary.Add(tkey, list2);
				}
				list2.Add(elementSelector(tsource));
			}
			return new Lookup<TKey, TElement>(dictionary, list);
		}

		// Token: 0x0600BDE2 RID: 48610 RVA: 0x004630FC File Offset: 0x004612FC
		public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			return first.SequenceEqual<TSource>(second, null);
		}

		// Token: 0x0600BDE3 RID: 48611 RVA: 0x00463108 File Offset: 0x00461308
		public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Check.FirstAndSecond(first, second);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			bool flag;
			using (IEnumerator<TSource> enumerator = first.GetEnumerator())
			{
				using (IEnumerator<TSource> enumerator2 = second.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator2.MoveNext())
						{
							return false;
						}
						if (!comparer.Equals(enumerator.Current, enumerator2.Current))
						{
							return false;
						}
					}
					flag = !enumerator2.MoveNext();
				}
			}
			return flag;
		}

		// Token: 0x0600BDE4 RID: 48612 RVA: 0x004631A0 File Offset: 0x004613A0
		public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			Check.FirstAndSecond(first, second);
			return first.Union<TSource>(second, null);
		}

		// Token: 0x0600BDE5 RID: 48613 RVA: 0x004631B1 File Offset: 0x004613B1
		public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Check.FirstAndSecond(first, second);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			return Enumerable.CreateUnionIterator<TSource>(first, second, comparer);
		}

		// Token: 0x0600BDE6 RID: 48614 RVA: 0x004631CC File Offset: 0x004613CC
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateUnionIterator<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			HashSet<TSource> items = new HashSet<TSource>(comparer);
			foreach (TSource tsource in first)
			{
				if (!items.Contains(tsource))
				{
					items.Add(tsource);
					yield return tsource;
				}
			}
			IEnumerator<TSource> enumerator = null;
			foreach (TSource tsource2 in second)
			{
				if (!items.Contains(tsource2))
				{
					items.Add(tsource2);
					yield return tsource2;
				}
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BDE7 RID: 48615 RVA: 0x004631EC File Offset: 0x004613EC
		public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			TSource[] array = source as TSource[];
			if (array != null)
			{
				return Enumerable.CreateWhereIterator<TSource>(array, predicate);
			}
			return Enumerable.CreateWhereIterator<TSource>(source, predicate);
		}

		// Token: 0x0600BDE8 RID: 48616 RVA: 0x00463219 File Offset: 0x00461419
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateWhereIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			foreach (TSource tsource in source)
			{
				if (predicate(tsource))
				{
					yield return tsource;
				}
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BDE9 RID: 48617 RVA: 0x00463230 File Offset: 0x00461430
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateWhereIterator<TSource>(TSource[] source, Func<TSource, bool> predicate)
		{
			int num;
			for (int i = 0; i < source.Length; i = num)
			{
				TSource tsource = source[i];
				if (predicate(tsource))
				{
					yield return tsource;
				}
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x0600BDEA RID: 48618 RVA: 0x00463248 File Offset: 0x00461448
		public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			TSource[] array = source as TSource[];
			if (array != null)
			{
				return Enumerable.CreateWhereIterator<TSource>(array, predicate);
			}
			return Enumerable.CreateWhereIterator<TSource>(source, predicate);
		}

		// Token: 0x0600BDEB RID: 48619 RVA: 0x00463275 File Offset: 0x00461475
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateWhereIterator<TSource>(IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			int counter = 0;
			foreach (TSource tsource in source)
			{
				if (predicate(tsource, counter))
				{
					yield return tsource;
				}
				int num = counter;
				counter = num + 1;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BDEC RID: 48620 RVA: 0x0046328C File Offset: 0x0046148C
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateWhereIterator<TSource>(TSource[] source, Func<TSource, int, bool> predicate)
		{
			int num;
			for (int i = 0; i < source.Length; i = num)
			{
				TSource tsource = source[i];
				if (predicate(tsource, i))
				{
					yield return tsource;
				}
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x0600BDED RID: 48621 RVA: 0x004632A3 File Offset: 0x004614A3
		[PublicizedFrom(EAccessModifier.Private)]
		public static Exception EmptySequence()
		{
			return new InvalidOperationException("Sequence contains no elements");
		}

		// Token: 0x0600BDEE RID: 48622 RVA: 0x004632AF File Offset: 0x004614AF
		[PublicizedFrom(EAccessModifier.Private)]
		public static Exception NoMatchingElement()
		{
			return new InvalidOperationException("Sequence contains no matching element");
		}

		// Token: 0x0600BDEF RID: 48623 RVA: 0x004632BB File Offset: 0x004614BB
		[PublicizedFrom(EAccessModifier.Private)]
		public static Exception MoreThanOneElement()
		{
			return new InvalidOperationException("Sequence contains more than one element");
		}

		// Token: 0x0600BDF0 RID: 48624 RVA: 0x004632C7 File Offset: 0x004614C7
		[PublicizedFrom(EAccessModifier.Private)]
		public static Exception MoreThanOneMatchingElement()
		{
			return new InvalidOperationException("Sequence contains more than one matching element");
		}

		// Token: 0x020017EB RID: 6123
		[PublicizedFrom(EAccessModifier.Private)]
		public enum Fallback
		{
			// Token: 0x04008D91 RID: 36241
			Default,
			// Token: 0x04008D92 RID: 36242
			Throw
		}
	}
}
