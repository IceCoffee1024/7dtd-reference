using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017B9 RID: 6073
	public static class Enumerable
	{
		// Token: 0x0600BC04 RID: 48132 RVA: 0x0045BC80 File Offset: 0x00459E80
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

		// Token: 0x0600BC05 RID: 48133 RVA: 0x0045BCE8 File Offset: 0x00459EE8
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

		// Token: 0x0600BC06 RID: 48134 RVA: 0x0045BD3C File Offset: 0x00459F3C
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

		// Token: 0x0600BC07 RID: 48135 RVA: 0x0045BDA4 File Offset: 0x00459FA4
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

		// Token: 0x0600BC08 RID: 48136 RVA: 0x0045BDFC File Offset: 0x00459FFC
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

		// Token: 0x0600BC09 RID: 48137 RVA: 0x0045BE50 File Offset: 0x0045A050
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

		// Token: 0x0600BC0A RID: 48138 RVA: 0x0012AC85 File Offset: 0x00128E85
		public static IEnumerable<TSource> AsEnumerable<TSource>(this IEnumerable<TSource> source)
		{
			return source;
		}

		// Token: 0x0600BC0B RID: 48139 RVA: 0x0045BEA8 File Offset: 0x0045A0A8
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

		// Token: 0x0600BC0C RID: 48140 RVA: 0x0045BF0C File Offset: 0x0045A10C
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

		// Token: 0x0600BC0D RID: 48141 RVA: 0x0045BF70 File Offset: 0x0045A170
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

		// Token: 0x0600BC0E RID: 48142 RVA: 0x0045BFDC File Offset: 0x0045A1DC
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

		// Token: 0x0600BC0F RID: 48143 RVA: 0x0045C044 File Offset: 0x0045A244
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

		// Token: 0x0600BC10 RID: 48144 RVA: 0x0045C0B8 File Offset: 0x0045A2B8
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

		// Token: 0x0600BC11 RID: 48145 RVA: 0x0045C144 File Offset: 0x0045A344
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

		// Token: 0x0600BC12 RID: 48146 RVA: 0x0045C1C4 File Offset: 0x0045A3C4
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

		// Token: 0x0600BC13 RID: 48147 RVA: 0x0045C244 File Offset: 0x0045A444
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

		// Token: 0x0600BC14 RID: 48148 RVA: 0x0045C2C8 File Offset: 0x0045A4C8
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

		// Token: 0x0600BC15 RID: 48149 RVA: 0x0045C358 File Offset: 0x0045A558
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

		// Token: 0x0600BC16 RID: 48150 RVA: 0x0045C3D8 File Offset: 0x0045A5D8
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

		// Token: 0x0600BC17 RID: 48151 RVA: 0x0045C444 File Offset: 0x0045A644
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

		// Token: 0x0600BC18 RID: 48152 RVA: 0x0045C4CC File Offset: 0x0045A6CC
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

		// Token: 0x0600BC19 RID: 48153 RVA: 0x0045C538 File Offset: 0x0045A738
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

		// Token: 0x0600BC1A RID: 48154 RVA: 0x0045C5C0 File Offset: 0x0045A7C0
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

		// Token: 0x0600BC1B RID: 48155 RVA: 0x0045C634 File Offset: 0x0045A834
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

		// Token: 0x0600BC1C RID: 48156 RVA: 0x0045C6C4 File Offset: 0x0045A8C4
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

		// Token: 0x0600BC1D RID: 48157 RVA: 0x0045C734 File Offset: 0x0045A934
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

		// Token: 0x0600BC1E RID: 48158 RVA: 0x0045C7C0 File Offset: 0x0045A9C0
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

		// Token: 0x0600BC1F RID: 48159 RVA: 0x0045C83C File Offset: 0x0045AA3C
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

		// Token: 0x0600BC20 RID: 48160 RVA: 0x0045C8D4 File Offset: 0x0045AAD4
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

		// Token: 0x0600BC21 RID: 48161 RVA: 0x0045C8F9 File Offset: 0x0045AAF9
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

		// Token: 0x0600BC22 RID: 48162 RVA: 0x0045C909 File Offset: 0x0045AB09
		public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			Check.FirstAndSecond(first, second);
			return Enumerable.CreateConcatIterator<TSource>(first, second);
		}

		// Token: 0x0600BC23 RID: 48163 RVA: 0x0045C919 File Offset: 0x0045AB19
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

		// Token: 0x0600BC24 RID: 48164 RVA: 0x0045C930 File Offset: 0x0045AB30
		public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value)
		{
			ICollection<TSource> collection = source as ICollection<TSource>;
			if (collection != null)
			{
				return collection.Contains(value);
			}
			return source.Contains(value, null);
		}

		// Token: 0x0600BC25 RID: 48165 RVA: 0x0045C958 File Offset: 0x0045AB58
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

		// Token: 0x0600BC26 RID: 48166 RVA: 0x0045C9BC File Offset: 0x0045ABBC
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

		// Token: 0x0600BC27 RID: 48167 RVA: 0x0045CA14 File Offset: 0x0045AC14
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

		// Token: 0x0600BC28 RID: 48168 RVA: 0x0045CA6C File Offset: 0x0045AC6C
		public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source)
		{
			return source.DefaultIfEmpty(default(TSource));
		}

		// Token: 0x0600BC29 RID: 48169 RVA: 0x0045CA88 File Offset: 0x0045AC88
		public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
		{
			Check.Source(source);
			return Enumerable.CreateDefaultIfEmptyIterator<TSource>(source, defaultValue);
		}

		// Token: 0x0600BC2A RID: 48170 RVA: 0x0045CA97 File Offset: 0x0045AC97
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

		// Token: 0x0600BC2B RID: 48171 RVA: 0x0045CAAE File Offset: 0x0045ACAE
		public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source)
		{
			return source.Distinct<TSource>(null);
		}

		// Token: 0x0600BC2C RID: 48172 RVA: 0x0045CAB7 File Offset: 0x0045ACB7
		public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
		{
			Check.Source(source);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			return Enumerable.CreateDistinctIterator<TSource>(source, comparer);
		}

		// Token: 0x0600BC2D RID: 48173 RVA: 0x0045CAD0 File Offset: 0x0045ACD0
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

		// Token: 0x0600BC2E RID: 48174 RVA: 0x0045CAE8 File Offset: 0x0045ACE8
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

		// Token: 0x0600BC2F RID: 48175 RVA: 0x0045CB50 File Offset: 0x0045AD50
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

		// Token: 0x0600BC30 RID: 48176 RVA: 0x0045CB88 File Offset: 0x0045AD88
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

		// Token: 0x0600BC31 RID: 48177 RVA: 0x0045CBD6 File Offset: 0x0045ADD6
		public static IEnumerable<TResult> Empty<TResult>()
		{
			return new TResult[0];
		}

		// Token: 0x0600BC32 RID: 48178 RVA: 0x0045CBDE File Offset: 0x0045ADDE
		public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			return first.Except<TSource>(second, null);
		}

		// Token: 0x0600BC33 RID: 48179 RVA: 0x0045CBE8 File Offset: 0x0045ADE8
		public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Check.FirstAndSecond(first, second);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			return Enumerable.CreateExceptIterator<TSource>(first, second, comparer);
		}

		// Token: 0x0600BC34 RID: 48180 RVA: 0x0045CC03 File Offset: 0x0045AE03
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

		// Token: 0x0600BC35 RID: 48181 RVA: 0x0045CC24 File Offset: 0x0045AE24
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

		// Token: 0x0600BC36 RID: 48182 RVA: 0x0045CC88 File Offset: 0x0045AE88
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

		// Token: 0x0600BC37 RID: 48183 RVA: 0x0045CCF0 File Offset: 0x0045AEF0
		public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.First<TSource>(predicate, Enumerable.Fallback.Throw);
		}

		// Token: 0x0600BC38 RID: 48184 RVA: 0x0045CD04 File Offset: 0x0045AF04
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

		// Token: 0x0600BC39 RID: 48185 RVA: 0x0045CD58 File Offset: 0x0045AF58
		public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.First<TSource>(predicate, Enumerable.Fallback.Default);
		}

		// Token: 0x0600BC3A RID: 48186 RVA: 0x0045CD69 File Offset: 0x0045AF69
		public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.GroupBy<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BC3B RID: 48187 RVA: 0x0045CD73 File Offset: 0x0045AF73
		public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return source.CreateGroupByIterator<TSource, TKey>(keySelector, comparer);
		}

		// Token: 0x0600BC3C RID: 48188 RVA: 0x0045CD84 File Offset: 0x0045AF84
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

		// Token: 0x0600BC3D RID: 48189 RVA: 0x0045CDA2 File Offset: 0x0045AFA2
		public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
		{
			return source.GroupBy<TSource, TKey, TElement>(keySelector, elementSelector, null);
		}

		// Token: 0x0600BC3E RID: 48190 RVA: 0x0045CDAD File Offset: 0x0045AFAD
		public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeyElementSelectors(source, keySelector, elementSelector);
			return source.CreateGroupByIterator<TSource, TKey, TElement>(keySelector, elementSelector, comparer);
		}

		// Token: 0x0600BC3F RID: 48191 RVA: 0x0045CDC0 File Offset: 0x0045AFC0
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

		// Token: 0x0600BC40 RID: 48192 RVA: 0x0045CDE5 File Offset: 0x0045AFE5
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
		{
			return source.GroupBy<TSource, TKey, TElement, TResult>(keySelector, elementSelector, resultSelector, null);
		}

		// Token: 0x0600BC41 RID: 48193 RVA: 0x0045CDF1 File Offset: 0x0045AFF1
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Check.GroupBySelectors(source, keySelector, elementSelector, resultSelector);
			return source.CreateGroupByIterator<TSource, TKey, TElement, TResult>(keySelector, elementSelector, resultSelector, comparer);
		}

		// Token: 0x0600BC42 RID: 48194 RVA: 0x0045CE07 File Offset: 0x0045B007
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

		// Token: 0x0600BC43 RID: 48195 RVA: 0x0045CE34 File Offset: 0x0045B034
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
		{
			return source.GroupBy<TSource, TKey, TResult>(keySelector, resultSelector, null);
		}

		// Token: 0x0600BC44 RID: 48196 RVA: 0x0045CE3F File Offset: 0x0045B03F
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeyResultSelectors(source, keySelector, resultSelector);
			return source.CreateGroupByIterator<TSource, TKey, TResult>(keySelector, resultSelector, comparer);
		}

		// Token: 0x0600BC45 RID: 48197 RVA: 0x0045CE52 File Offset: 0x0045B052
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

		// Token: 0x0600BC46 RID: 48198 RVA: 0x0045CE77 File Offset: 0x0045B077
		public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
		{
			return outer.GroupJoin<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, null);
		}

		// Token: 0x0600BC47 RID: 48199 RVA: 0x0045CE85 File Offset: 0x0045B085
		public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Check.JoinSelectors(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
			if (comparer == null)
			{
				comparer = EqualityComparer<TKey>.Default;
			}
			return outer.CreateGroupJoinIterator<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
		}

		// Token: 0x0600BC48 RID: 48200 RVA: 0x0045CEAA File Offset: 0x0045B0AA
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

		// Token: 0x0600BC49 RID: 48201 RVA: 0x0045CEDF File Offset: 0x0045B0DF
		public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			return first.Intersect<TSource>(second, null);
		}

		// Token: 0x0600BC4A RID: 48202 RVA: 0x0045CEE9 File Offset: 0x0045B0E9
		public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Check.FirstAndSecond(first, second);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			return Enumerable.CreateIntersectIterator<TSource>(first, second, comparer);
		}

		// Token: 0x0600BC4B RID: 48203 RVA: 0x0045CF04 File Offset: 0x0045B104
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

		// Token: 0x0600BC4C RID: 48204 RVA: 0x0045CF22 File Offset: 0x0045B122
		public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Check.JoinSelectors(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
			if (comparer == null)
			{
				comparer = EqualityComparer<TKey>.Default;
			}
			return outer.CreateJoinIterator<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
		}

		// Token: 0x0600BC4D RID: 48205 RVA: 0x0045CF47 File Offset: 0x0045B147
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

		// Token: 0x0600BC4E RID: 48206 RVA: 0x0045CF7C File Offset: 0x0045B17C
		public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
		{
			return outer.Join<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, null);
		}

		// Token: 0x0600BC4F RID: 48207 RVA: 0x0045CF8C File Offset: 0x0045B18C
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

		// Token: 0x0600BC50 RID: 48208 RVA: 0x0045CFF4 File Offset: 0x0045B1F4
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

		// Token: 0x0600BC51 RID: 48209 RVA: 0x0045D088 File Offset: 0x0045B288
		public static TSource Last<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.Last<TSource>(predicate, Enumerable.Fallback.Throw);
		}

		// Token: 0x0600BC52 RID: 48210 RVA: 0x0045D09C File Offset: 0x0045B29C
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

		// Token: 0x0600BC53 RID: 48211 RVA: 0x0045D124 File Offset: 0x0045B324
		public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.Last<TSource>(predicate, Enumerable.Fallback.Default);
		}

		// Token: 0x0600BC54 RID: 48212 RVA: 0x0045D138 File Offset: 0x0045B338
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

		// Token: 0x0600BC55 RID: 48213 RVA: 0x0045D190 File Offset: 0x0045B390
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

		// Token: 0x0600BC56 RID: 48214 RVA: 0x0045D1EC File Offset: 0x0045B3EC
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

		// Token: 0x0600BC57 RID: 48215 RVA: 0x0045D250 File Offset: 0x0045B450
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

		// Token: 0x0600BC58 RID: 48216 RVA: 0x0045D2B8 File Offset: 0x0045B4B8
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

		// Token: 0x0600BC59 RID: 48217 RVA: 0x0045D320 File Offset: 0x0045B520
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

		// Token: 0x0600BC5A RID: 48218 RVA: 0x0045D384 File Offset: 0x0045B584
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

		// Token: 0x0600BC5B RID: 48219 RVA: 0x0045D3EC File Offset: 0x0045B5EC
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

		// Token: 0x0600BC5C RID: 48220 RVA: 0x0045D468 File Offset: 0x0045B668
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

		// Token: 0x0600BC5D RID: 48221 RVA: 0x0045D4E8 File Offset: 0x0045B6E8
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

		// Token: 0x0600BC5E RID: 48222 RVA: 0x0045D568 File Offset: 0x0045B768
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

		// Token: 0x0600BC5F RID: 48223 RVA: 0x0045D5E4 File Offset: 0x0045B7E4
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

		// Token: 0x0600BC60 RID: 48224 RVA: 0x0045D668 File Offset: 0x0045B868
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

		// Token: 0x0600BC61 RID: 48225 RVA: 0x0045D748 File Offset: 0x0045B948
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

		// Token: 0x0600BC62 RID: 48226 RVA: 0x0045D7B4 File Offset: 0x0045B9B4
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

		// Token: 0x0600BC63 RID: 48227 RVA: 0x0045D824 File Offset: 0x0045BA24
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

		// Token: 0x0600BC64 RID: 48228 RVA: 0x0045D894 File Offset: 0x0045BA94
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

		// Token: 0x0600BC65 RID: 48229 RVA: 0x0045D900 File Offset: 0x0045BB00
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

		// Token: 0x0600BC66 RID: 48230 RVA: 0x0045D970 File Offset: 0x0045BB70
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

		// Token: 0x0600BC67 RID: 48231 RVA: 0x0045D9CC File Offset: 0x0045BBCC
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

		// Token: 0x0600BC68 RID: 48232 RVA: 0x0045DA74 File Offset: 0x0045BC74
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

		// Token: 0x0600BC69 RID: 48233 RVA: 0x0045DB1C File Offset: 0x0045BD1C
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

		// Token: 0x0600BC6A RID: 48234 RVA: 0x0045DBC4 File Offset: 0x0045BDC4
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

		// Token: 0x0600BC6B RID: 48235 RVA: 0x0045DC6C File Offset: 0x0045BE6C
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

		// Token: 0x0600BC6C RID: 48236 RVA: 0x0045DD18 File Offset: 0x0045BF18
		public static TResult Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			Check.SourceAndSelector(source, selector);
			return source.Select<TSource, TResult>(selector).Max<TResult>();
		}

		// Token: 0x0600BC6D RID: 48237 RVA: 0x0045DD30 File Offset: 0x0045BF30
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

		// Token: 0x0600BC6E RID: 48238 RVA: 0x0045DD94 File Offset: 0x0045BF94
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

		// Token: 0x0600BC6F RID: 48239 RVA: 0x0045DDFC File Offset: 0x0045BFFC
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

		// Token: 0x0600BC70 RID: 48240 RVA: 0x0045DE64 File Offset: 0x0045C064
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

		// Token: 0x0600BC71 RID: 48241 RVA: 0x0045DEC8 File Offset: 0x0045C0C8
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

		// Token: 0x0600BC72 RID: 48242 RVA: 0x0045DF30 File Offset: 0x0045C130
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

		// Token: 0x0600BC73 RID: 48243 RVA: 0x0045DFAC File Offset: 0x0045C1AC
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

		// Token: 0x0600BC74 RID: 48244 RVA: 0x0045E02C File Offset: 0x0045C22C
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

		// Token: 0x0600BC75 RID: 48245 RVA: 0x0045E0AC File Offset: 0x0045C2AC
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

		// Token: 0x0600BC76 RID: 48246 RVA: 0x0045E128 File Offset: 0x0045C328
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

		// Token: 0x0600BC77 RID: 48247 RVA: 0x0045E1AC File Offset: 0x0045C3AC
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

		// Token: 0x0600BC78 RID: 48248 RVA: 0x0045E28C File Offset: 0x0045C48C
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

		// Token: 0x0600BC79 RID: 48249 RVA: 0x0045E2F8 File Offset: 0x0045C4F8
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

		// Token: 0x0600BC7A RID: 48250 RVA: 0x0045E368 File Offset: 0x0045C568
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

		// Token: 0x0600BC7B RID: 48251 RVA: 0x0045E3D8 File Offset: 0x0045C5D8
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

		// Token: 0x0600BC7C RID: 48252 RVA: 0x0045E444 File Offset: 0x0045C644
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

		// Token: 0x0600BC7D RID: 48253 RVA: 0x0045E4B4 File Offset: 0x0045C6B4
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

		// Token: 0x0600BC7E RID: 48254 RVA: 0x0045E55C File Offset: 0x0045C75C
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

		// Token: 0x0600BC7F RID: 48255 RVA: 0x0045E604 File Offset: 0x0045C804
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

		// Token: 0x0600BC80 RID: 48256 RVA: 0x0045E6AC File Offset: 0x0045C8AC
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

		// Token: 0x0600BC81 RID: 48257 RVA: 0x0045E754 File Offset: 0x0045C954
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

		// Token: 0x0600BC82 RID: 48258 RVA: 0x0045E800 File Offset: 0x0045CA00
		public static TResult Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			Check.SourceAndSelector(source, selector);
			return source.Select<TSource, TResult>(selector).Min<TResult>();
		}

		// Token: 0x0600BC83 RID: 48259 RVA: 0x0045E815 File Offset: 0x0045CA15
		public static IEnumerable<TResult> OfType<TResult>(this IEnumerable source)
		{
			Check.Source(source);
			return Enumerable.CreateOfTypeIterator<TResult>(source);
		}

		// Token: 0x0600BC84 RID: 48260 RVA: 0x0045E823 File Offset: 0x0045CA23
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

		// Token: 0x0600BC85 RID: 48261 RVA: 0x0045E833 File Offset: 0x0045CA33
		public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.OrderBy<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BC86 RID: 48262 RVA: 0x0045E83D File Offset: 0x0045CA3D
		public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return new OrderedSequence<TSource, TKey>(source, keySelector, comparer, SortDirection.Ascending);
		}

		// Token: 0x0600BC87 RID: 48263 RVA: 0x0045E84F File Offset: 0x0045CA4F
		public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.OrderByDescending<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BC88 RID: 48264 RVA: 0x0045E859 File Offset: 0x0045CA59
		public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return new OrderedSequence<TSource, TKey>(source, keySelector, comparer, SortDirection.Descending);
		}

		// Token: 0x0600BC89 RID: 48265 RVA: 0x0045E86B File Offset: 0x0045CA6B
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

		// Token: 0x0600BC8A RID: 48266 RVA: 0x0045E899 File Offset: 0x0045CA99
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

		// Token: 0x0600BC8B RID: 48267 RVA: 0x0045E8B0 File Offset: 0x0045CAB0
		public static IEnumerable<TResult> Repeat<TResult>(TResult element, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return Enumerable.CreateRepeatIterator<TResult>(element, count);
		}

		// Token: 0x0600BC8C RID: 48268 RVA: 0x0045E8C3 File Offset: 0x0045CAC3
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

		// Token: 0x0600BC8D RID: 48269 RVA: 0x0045E8DA File Offset: 0x0045CADA
		public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			return Enumerable.CreateReverseIterator<TSource>(source);
		}

		// Token: 0x0600BC8E RID: 48270 RVA: 0x0045E8E8 File Offset: 0x0045CAE8
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

		// Token: 0x0600BC8F RID: 48271 RVA: 0x0045E8F8 File Offset: 0x0045CAF8
		public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			Check.SourceAndSelector(source, selector);
			return Enumerable.CreateSelectIterator<TSource, TResult>(source, selector);
		}

		// Token: 0x0600BC90 RID: 48272 RVA: 0x0045E908 File Offset: 0x0045CB08
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

		// Token: 0x0600BC91 RID: 48273 RVA: 0x0045E91F File Offset: 0x0045CB1F
		public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
		{
			Check.SourceAndSelector(source, selector);
			return Enumerable.CreateSelectIterator<TSource, TResult>(source, selector);
		}

		// Token: 0x0600BC92 RID: 48274 RVA: 0x0045E92F File Offset: 0x0045CB2F
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

		// Token: 0x0600BC93 RID: 48275 RVA: 0x0045E946 File Offset: 0x0045CB46
		public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
		{
			Check.SourceAndSelector(source, selector);
			return Enumerable.CreateSelectManyIterator<TSource, TResult>(source, selector);
		}

		// Token: 0x0600BC94 RID: 48276 RVA: 0x0045E956 File Offset: 0x0045CB56
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

		// Token: 0x0600BC95 RID: 48277 RVA: 0x0045E96D File Offset: 0x0045CB6D
		public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
		{
			Check.SourceAndSelector(source, selector);
			return Enumerable.CreateSelectManyIterator<TSource, TResult>(source, selector);
		}

		// Token: 0x0600BC96 RID: 48278 RVA: 0x0045E97D File Offset: 0x0045CB7D
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

		// Token: 0x0600BC97 RID: 48279 RVA: 0x0045E994 File Offset: 0x0045CB94
		public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
		{
			Check.SourceAndCollectionSelectors(source, collectionSelector, resultSelector);
			return Enumerable.CreateSelectManyIterator<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		// Token: 0x0600BC98 RID: 48280 RVA: 0x0045E9A6 File Offset: 0x0045CBA6
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

		// Token: 0x0600BC99 RID: 48281 RVA: 0x0045E9C4 File Offset: 0x0045CBC4
		public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
		{
			Check.SourceAndCollectionSelectors(source, collectionSelector, resultSelector);
			return Enumerable.CreateSelectManyIterator<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		// Token: 0x0600BC9A RID: 48282 RVA: 0x0045E9D6 File Offset: 0x0045CBD6
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

		// Token: 0x0600BC9B RID: 48283 RVA: 0x0045E9F4 File Offset: 0x0045CBF4
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

		// Token: 0x0600BC9C RID: 48284 RVA: 0x0045EA64 File Offset: 0x0045CC64
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

		// Token: 0x0600BC9D RID: 48285 RVA: 0x0045EACC File Offset: 0x0045CCCC
		public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.Single<TSource>(predicate, Enumerable.Fallback.Throw);
		}

		// Token: 0x0600BC9E RID: 48286 RVA: 0x0045EAE0 File Offset: 0x0045CCE0
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

		// Token: 0x0600BC9F RID: 48287 RVA: 0x0045EB40 File Offset: 0x0045CD40
		public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.Single<TSource>(predicate, Enumerable.Fallback.Default);
		}

		// Token: 0x0600BCA0 RID: 48288 RVA: 0x0045EB51 File Offset: 0x0045CD51
		public static IEnumerable<TSource> Skip<TSource>(this IEnumerable<TSource> source, int count)
		{
			Check.Source(source);
			return Enumerable.CreateSkipIterator<TSource>(source, count);
		}

		// Token: 0x0600BCA1 RID: 48289 RVA: 0x0045EB60 File Offset: 0x0045CD60
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

		// Token: 0x0600BCA2 RID: 48290 RVA: 0x0045EB77 File Offset: 0x0045CD77
		public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return Enumerable.CreateSkipWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x0600BCA3 RID: 48291 RVA: 0x0045EB87 File Offset: 0x0045CD87
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

		// Token: 0x0600BCA4 RID: 48292 RVA: 0x0045EB9E File Offset: 0x0045CD9E
		public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return Enumerable.CreateSkipWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x0600BCA5 RID: 48293 RVA: 0x0045EBAE File Offset: 0x0045CDAE
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

		// Token: 0x0600BCA6 RID: 48294 RVA: 0x0045EBC8 File Offset: 0x0045CDC8
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

		// Token: 0x0600BCA7 RID: 48295 RVA: 0x0045EC18 File Offset: 0x0045CE18
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

		// Token: 0x0600BCA8 RID: 48296 RVA: 0x0045EC7C File Offset: 0x0045CE7C
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

		// Token: 0x0600BCA9 RID: 48297 RVA: 0x0045ECD4 File Offset: 0x0045CED4
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

		// Token: 0x0600BCAA RID: 48298 RVA: 0x0045ED40 File Offset: 0x0045CF40
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

		// Token: 0x0600BCAB RID: 48299 RVA: 0x0045ED90 File Offset: 0x0045CF90
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

		// Token: 0x0600BCAC RID: 48300 RVA: 0x0045EDF4 File Offset: 0x0045CFF4
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

		// Token: 0x0600BCAD RID: 48301 RVA: 0x0045EE4C File Offset: 0x0045D04C
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

		// Token: 0x0600BCAE RID: 48302 RVA: 0x0045EEB8 File Offset: 0x0045D0B8
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

		// Token: 0x0600BCAF RID: 48303 RVA: 0x0045EF10 File Offset: 0x0045D110
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

		// Token: 0x0600BCB0 RID: 48304 RVA: 0x0045EF7C File Offset: 0x0045D17C
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

		// Token: 0x0600BCB1 RID: 48305 RVA: 0x0045EFDC File Offset: 0x0045D1DC
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

		// Token: 0x0600BCB2 RID: 48306 RVA: 0x0045F050 File Offset: 0x0045D250
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

		// Token: 0x0600BCB3 RID: 48307 RVA: 0x0045F0A4 File Offset: 0x0045D2A4
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

		// Token: 0x0600BCB4 RID: 48308 RVA: 0x0045F10C File Offset: 0x0045D30C
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

		// Token: 0x0600BCB5 RID: 48309 RVA: 0x0045F168 File Offset: 0x0045D368
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

		// Token: 0x0600BCB6 RID: 48310 RVA: 0x0045F1D8 File Offset: 0x0045D3D8
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

		// Token: 0x0600BCB7 RID: 48311 RVA: 0x0045F230 File Offset: 0x0045D430
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

		// Token: 0x0600BCB8 RID: 48312 RVA: 0x0045F29C File Offset: 0x0045D49C
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

		// Token: 0x0600BCB9 RID: 48313 RVA: 0x0045F2FC File Offset: 0x0045D4FC
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

		// Token: 0x0600BCBA RID: 48314 RVA: 0x0045F374 File Offset: 0x0045D574
		public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count)
		{
			Check.Source(source);
			return Enumerable.CreateTakeIterator<TSource>(source, count);
		}

		// Token: 0x0600BCBB RID: 48315 RVA: 0x0045F383 File Offset: 0x0045D583
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

		// Token: 0x0600BCBC RID: 48316 RVA: 0x0045F39A File Offset: 0x0045D59A
		public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return Enumerable.CreateTakeWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x0600BCBD RID: 48317 RVA: 0x0045F3AA File Offset: 0x0045D5AA
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

		// Token: 0x0600BCBE RID: 48318 RVA: 0x0045F3C1 File Offset: 0x0045D5C1
		public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return Enumerable.CreateTakeWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x0600BCBF RID: 48319 RVA: 0x0045F3D1 File Offset: 0x0045D5D1
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

		// Token: 0x0600BCC0 RID: 48320 RVA: 0x0045F3E8 File Offset: 0x0045D5E8
		public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ThenBy<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BCC1 RID: 48321 RVA: 0x0045F3F2 File Offset: 0x0045D5F2
		public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return (source as OrderedEnumerable<TSource>).CreateOrderedEnumerable<TKey>(keySelector, comparer, false);
		}

		// Token: 0x0600BCC2 RID: 48322 RVA: 0x0045F409 File Offset: 0x0045D609
		public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ThenByDescending<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BCC3 RID: 48323 RVA: 0x0045F413 File Offset: 0x0045D613
		public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return (source as OrderedEnumerable<TSource>).CreateOrderedEnumerable<TKey>(keySelector, comparer, true);
		}

		// Token: 0x0600BCC4 RID: 48324 RVA: 0x0045F42C File Offset: 0x0045D62C
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

		// Token: 0x0600BCC5 RID: 48325 RVA: 0x0045F4E4 File Offset: 0x0045D6E4
		public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
		{
			return source.ToDictionary<TSource, TKey, TElement>(keySelector, elementSelector, null);
		}

		// Token: 0x0600BCC6 RID: 48326 RVA: 0x0045F4F0 File Offset: 0x0045D6F0
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

		// Token: 0x0600BCC7 RID: 48327 RVA: 0x0045F560 File Offset: 0x0045D760
		public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ToDictionary<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BCC8 RID: 48328 RVA: 0x0045F56C File Offset: 0x0045D76C
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

		// Token: 0x0600BCC9 RID: 48329 RVA: 0x0045F5D4 File Offset: 0x0045D7D4
		public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			return new List<TSource>(source);
		}

		// Token: 0x0600BCCA RID: 48330 RVA: 0x0045F5E2 File Offset: 0x0045D7E2
		public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ToLookup<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BCCB RID: 48331 RVA: 0x0045F5EC File Offset: 0x0045D7EC
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

		// Token: 0x0600BCCC RID: 48332 RVA: 0x0045F694 File Offset: 0x0045D894
		public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
		{
			return source.ToLookup<TSource, TKey, TElement>(keySelector, elementSelector, null);
		}

		// Token: 0x0600BCCD RID: 48333 RVA: 0x0045F6A0 File Offset: 0x0045D8A0
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

		// Token: 0x0600BCCE RID: 48334 RVA: 0x0045F750 File Offset: 0x0045D950
		public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			return first.SequenceEqual<TSource>(second, null);
		}

		// Token: 0x0600BCCF RID: 48335 RVA: 0x0045F75C File Offset: 0x0045D95C
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

		// Token: 0x0600BCD0 RID: 48336 RVA: 0x0045F7F4 File Offset: 0x0045D9F4
		public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			Check.FirstAndSecond(first, second);
			return first.Union<TSource>(second, null);
		}

		// Token: 0x0600BCD1 RID: 48337 RVA: 0x0045F805 File Offset: 0x0045DA05
		public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Check.FirstAndSecond(first, second);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			return Enumerable.CreateUnionIterator<TSource>(first, second, comparer);
		}

		// Token: 0x0600BCD2 RID: 48338 RVA: 0x0045F820 File Offset: 0x0045DA20
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

		// Token: 0x0600BCD3 RID: 48339 RVA: 0x0045F840 File Offset: 0x0045DA40
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

		// Token: 0x0600BCD4 RID: 48340 RVA: 0x0045F86D File Offset: 0x0045DA6D
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

		// Token: 0x0600BCD5 RID: 48341 RVA: 0x0045F884 File Offset: 0x0045DA84
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

		// Token: 0x0600BCD6 RID: 48342 RVA: 0x0045F89C File Offset: 0x0045DA9C
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

		// Token: 0x0600BCD7 RID: 48343 RVA: 0x0045F8C9 File Offset: 0x0045DAC9
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

		// Token: 0x0600BCD8 RID: 48344 RVA: 0x0045F8E0 File Offset: 0x0045DAE0
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

		// Token: 0x0600BCD9 RID: 48345 RVA: 0x0045F8F7 File Offset: 0x0045DAF7
		[PublicizedFrom(EAccessModifier.Private)]
		public static Exception EmptySequence()
		{
			return new InvalidOperationException("Sequence contains no elements");
		}

		// Token: 0x0600BCDA RID: 48346 RVA: 0x0045F903 File Offset: 0x0045DB03
		[PublicizedFrom(EAccessModifier.Private)]
		public static Exception NoMatchingElement()
		{
			return new InvalidOperationException("Sequence contains no matching element");
		}

		// Token: 0x0600BCDB RID: 48347 RVA: 0x0045F90F File Offset: 0x0045DB0F
		[PublicizedFrom(EAccessModifier.Private)]
		public static Exception MoreThanOneElement()
		{
			return new InvalidOperationException("Sequence contains more than one element");
		}

		// Token: 0x0600BCDC RID: 48348 RVA: 0x0045F91B File Offset: 0x0045DB1B
		[PublicizedFrom(EAccessModifier.Private)]
		public static Exception MoreThanOneMatchingElement()
		{
			return new InvalidOperationException("Sequence contains more than one matching element");
		}

		// Token: 0x020017BA RID: 6074
		[PublicizedFrom(EAccessModifier.Private)]
		public enum Fallback
		{
			// Token: 0x04008CA8 RID: 36008
			Default,
			// Token: 0x04008CA9 RID: 36009
			Throw
		}
	}
}
